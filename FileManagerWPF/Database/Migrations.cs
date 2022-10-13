using DbUp;
using Newtonsoft.Json;
using System.IO;

namespace FileManagerWPF.Database
{
    public static class Migrations
    {
        public static bool RunMigrations()
        {
            var json = File.ReadAllText("../../appsettings.json");
            var connectionString = (JsonConvert.DeserializeObject<DBConnectionString>(json)).ConnectionString;

            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsFromFileSystem(@"../../Database/Scripts/")
                    .Build();

            var result = upgrader.PerformUpgrade();
            return result.Successful;
        }
    }
}
