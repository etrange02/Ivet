using ExRam.Gremlinq.Core;
using Ivet.Model;
using Ivet.Services;
using Ivet.Verbs.Model;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;

namespace Ivet.Verbs.Services
{
    public class UpgradeAction
    {
        internal static bool IsTimeoutException(Exception ex)
        {
            if (ex is TimeoutException) return true;
            if (ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase)) return true;
            if (ex is AggregateException agg)
                return agg.InnerExceptions.Any(IsTimeoutException);
            if (ex.InnerException != null)
                return IsTimeoutException(ex.InnerException);
            return false;
        }

        public static void Do(UpgradeOptions options, ILoggerFactory loggerFactory)
        {
            CliArgumentValidator.ValidatePort(options.Port);
            CliArgumentValidator.ValidateTimeout(options.Timeout);

            var logger = loggerFactory.CreateLogger<UpgradeAction>();

            var files = new List<string>();

            var input = string.IsNullOrEmpty(options.Input) ? Directory.GetCurrentDirectory() : options.Input;

            if (File.Exists(options.Input))
            {
                if (options.Input.EndsWith(".json", true, CultureInfo.InvariantCulture))
                    files.Add(options.Input);
                else
                    throw new FormatException("Bad extension. Must be a json file or a directory.");
            }
            else
            {
                logger.LogInformation("Directory: {Input}", input);
                files.AddRange(Directory.EnumerateFiles(input, "*.json", SearchOption.AllDirectories).Order());
            }

            using var database = new DatabaseService(options.IpAddress, options.Port, options.UseSsl);

            var appliedMigrations = database.GremlinqClient.V<Migration>()
                .ToArrayAsync()
                .AsTask()
                .GetAwaiter()
                .GetResult()
                .Select(x => x.MigrationName);

            var migrationsToApply = files
                .Select(x => new  {
                    Fullname = x,
                    Object = JsonSerializer.Deserialize<MigrationFile>(File.ReadAllText(x)) ?? throw new FormatException($"File {x} has bad format")
                })
                .SelectMany(x => {
                    var filename = Path.GetFileNameWithoutExtension(x.Fullname);
                    if (x.Object.Scripts?.Any() ?? false)
                        return x.Object.Scripts.Select((y, i) => new MigrationInstance { Name = $"{ filename }_#{ i }", Script = y.Script, RelativePath = Path.GetRelativePath(input, x.Fullname), EvaluationTimeout = y.EvaluationTimeout ?? x.Object.EvaluationTimeout });
                    if (!string.IsNullOrEmpty(x.Object.Content))
                        return new List<MigrationInstance> { new() { Name = filename, Script = x.Object.Content, RelativePath = Path.GetRelativePath(input, x.Fullname), EvaluationTimeout = x.Object.EvaluationTimeout } };
                    return new List<MigrationInstance>();
                })
                .Where(x => !appliedMigrations.Contains(x.Name))
                .OrderBy(x => x.RelativePath)
                .ThenBy(x => x.Name)
                .ToList();

            migrationsToApply.ForEach(x =>
            {
                logger.LogInformation("Applying migration {Name} ({RelativePath})", x.Name, x.RelativePath);
                GremlinScriptValidator.Validate(x.Script);
                var timeout = x.EvaluationTimeout ?? options.Timeout;
                var hasExplicitTimeout = x.EvaluationTimeout.HasValue || options.Timeout.HasValue;

                try
                {
                    database.Execute(x.Script, timeout);
                }
                catch (Exception ex) when (hasExplicitTimeout && IsTimeoutException(ex))
                {
                    logger.LogWarning("Migration {Name} timed out. The index operation was submitted and will be completed by JanusGraph on restart. ({Message})", x.Name, ex.Message);
                }

                var migration = new Migration
                {
                    MigrationName = x.Name,
                    MigrationDate = DateTime.Now,
                };
                database.GremlinqClient.AddV(migration).FirstAsync().AsTask().GetAwaiter().GetResult();
            });
        }
    }
}
