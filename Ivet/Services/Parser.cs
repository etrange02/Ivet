using CsvHelper;
using CsvHelper.Configuration;
using Ivet.Model.Database;
using System.Globalization;

namespace Ivet.Services
{
    public class Parser
    {
        private CsvConfiguration _csvConfiguration;

        public Parser()
        {
            _csvConfiguration = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = "|",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                Comment = '-',
                AllowComments = true
            };
        }

        private List<T> GetItems<T>(string csv)
        {
            using var textReader = new StringReader(csv);
            using var reader = new CsvReader(textReader, _csvConfiguration);

            var items = reader.GetRecords<T>().ToList();
            return items;
        }

        public List<Vertex> GetVertices(string csv)
        {
            return GetItems<Vertex>(csv);
        }

        public List<Edge> GetEdges(string csv)
        {
            return GetItems<Edge>(csv);
        }

        public List<PropertyKey> GetPropertyKeys(string csv)
        {
            return GetItems<PropertyKey>(csv);
        }

        public List<Connection> GetConnections(string csv)
        {
            return GetItems<Connection>(csv);
        }

        public List<PropertyBinding> GetPropertyBindings(string csv)
        {
            return GetItems<PropertyBinding>(csv);
        }

        public List<Model.Database.Index> GetIndices(string csv)
        {
            return GetItems<Model.Database.Index>(csv);
        }

        public List<IndexBinding> GetIndexBindings(string csv)
        {
            return GetItems<IndexBinding>(csv);
        }
    }
}
