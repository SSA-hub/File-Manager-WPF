using System.IO;
using System.Threading.Tasks;
using Dommel;
using Newtonsoft.Json;
using Npgsql;

namespace FileManagerWPF.Database
{
    public class DBRepository
    {
        private readonly string _connectionString;

        public DBRepository()
        {
            var json = File.ReadAllText("../../appsettings.json");
            _connectionString = (JsonConvert.DeserializeObject<DBConnectionString>(json)).ConnectionString;
        }

        public async Task<int> InsertAsync(FileOpeningHistory newElem)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var result = await connection.InsertAsync(newElem);
                return (int)result;
            }
        }
    }
}
