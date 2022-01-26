using Microsoft.Extensions.Configuration;
using System;

namespace HallOfFame_backend
{
    public class AppSettings
    {
        public static string GetConnectionString()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                return Environment.GetEnvironmentVariable("DefaultConnection");
            }

            var dbServer = Environment.GetEnvironmentVariable("DBSERVER");
            var dbUser = Environment.GetEnvironmentVariable("DBUSER");
            var dbPass = Environment.GetEnvironmentVariable("DBPASS");
            var dbPort = Environment.GetEnvironmentVariable("DBPORT");
            var dbName = Environment.GetEnvironmentVariable("DBNAME");

            return $"Server={dbServer},{dbPort};Database={dbName};User={dbUser};Password={dbPass};MultipleActiveResultSets=true;";
        }
    }
}
