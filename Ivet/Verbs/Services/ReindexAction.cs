using Ivet.Model.Database;
using Ivet.Services;
using Ivet.Verbs.Model;
using Microsoft.Extensions.Logging;

namespace Ivet.Verbs.Services
{
    public class ReindexAction
    {
        private const string EnabledStatus = "ENABLED";

        public static void Do(ReindexOptions options, ILoggerFactory loggerFactory)
        {
            CliArgumentValidator.ValidatePort(options.Port);
            if (options.TimeoutSeconds < 1)
                throw new ArgumentOutOfRangeException(nameof(options.TimeoutSeconds), $"Timeout must be positive, got {options.TimeoutSeconds}");

            var logger = loggerFactory.CreateLogger<ReindexAction>();

            if (!string.IsNullOrEmpty(options.IndexName))
                GremlinIdentifierValidator.Validate(options.IndexName, "reindex --index");

            using var database = new DatabaseService(options.IpAddress, options.Port, options.UseSsl);

            var statuses = new Parser().GetIndexStatusRows(database.GetIndexStatusSchema());
            var targets = ResolveTargets(statuses, options.IndexName, logger);

            if (targets.Count == 0) return;

            var timeoutMs = options.TimeoutSeconds * 1000;
            foreach (var target in targets)
            {
                logger.LogInformation("Reindexing '{Index}'...", target);
                var script = BuildReindexScript(target, options.TimeoutSeconds, options.AwaitEnabled);
                try
                {
                    database.Execute(script, timeoutMs + 30_000);
                    logger.LogInformation("'{Index}' reindex script completed", target);
                }
                catch (Exception ex)
                {
                    logger.LogWarning("'{Index}' reindex failed or timed out: {Message}", target, ex.Message);
                }
            }
        }

        public static List<string> ResolveTargets(IReadOnlyList<IndexStatusRow> statuses, string? indexName, ILogger logger)
        {
            if (!string.IsNullOrEmpty(indexName))
            {
                var rowsForIndex = statuses.Where(r => string.Equals(r.IndexName, indexName, StringComparison.Ordinal)).ToList();
                if (rowsForIndex.Count == 0)
                {
                    logger.LogError("Index '{Index}' not found", indexName);
                    Environment.ExitCode = 1;
                    return new List<string>();
                }
                if (rowsForIndex.All(r => string.Equals(r.Status, EnabledStatus, StringComparison.OrdinalIgnoreCase)))
                {
                    logger.LogInformation("Index '{Index}' is already ENABLED on all keys, nothing to do", indexName);
                    return new List<string>();
                }
                return new List<string> { indexName };
            }

            var stuck = statuses
                .GroupBy(r => r.IndexName)
                .Where(g => g.Any(r => !string.Equals(r.Status, EnabledStatus, StringComparison.OrdinalIgnoreCase)))
                .Select(g => g.Key)
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (stuck.Count == 0)
                logger.LogInformation("All indices are ENABLED, nothing to do");
            else
                logger.LogInformation("Reindexing {Count} stuck indices: {Indices}", stuck.Count, string.Join(", ", stuck));

            return stuck;
        }

        public static string BuildReindexScript(string indexName, long timeoutSeconds, bool awaitEnabled)
        {
            var sanitized = GremlinIdentifierValidator.Validate(indexName, "reindex");
            var awaitRegistered = $"org.janusgraph.graphdb.database.management.ManagementSystem.awaitGraphIndexStatus(graph, '{sanitized}').status(SchemaStatus.REGISTERED).timeout({timeoutSeconds}, java.time.temporal.ChronoUnit.SECONDS).call();";
            var awaitEnabledCall = awaitEnabled
                ? $"org.janusgraph.graphdb.database.management.ManagementSystem.awaitGraphIndexStatus(graph, '{sanitized}').status(SchemaStatus.ENABLED).timeout({timeoutSeconds}, java.time.temporal.ChronoUnit.SECONDS).call();"
                : string.Empty;

            return $"mgmt = graph.openManagement();" +
                $"idx = mgmt.getGraphIndex('{sanitized}');" +
                $"allEnabled = idx.getFieldKeys().every {{ pk -> idx.getIndexStatus(pk) == SchemaStatus.ENABLED }};" +
                $"if (!allEnabled) {{" +
                    $"hasInstalled = idx.getFieldKeys().any {{ pk -> idx.getIndexStatus(pk) == SchemaStatus.INSTALLED }};" +
                    $"if (hasInstalled) {{ mgmt.updateIndex(idx, SchemaAction.REGISTER_INDEX).get(); mgmt.commit(); {awaitRegistered} mgmt = graph.openManagement(); }}" +
                    $"mgmt.updateIndex(mgmt.getGraphIndex('{sanitized}'), SchemaAction.REINDEX).get();" +
                    $"mgmt.commit();" +
                    $"mgmt = graph.openManagement();" +
                    $"{awaitEnabledCall}" +
                $"}};" +
                $"return 'ok';";
        }
    }
}
