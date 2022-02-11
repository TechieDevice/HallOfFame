using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using HallOfFame_backend.Logger;
using Microsoft.Extensions.Logging;
using System.IO;

namespace HallOfFame_backend.DataBase
{
    public class SetDockerConnection
    {
        public SetDockerConnection()
        {

        }

        public static string GetConnectionString()
        {
            //for using environments in docker
            var dbHost = Environment.GetEnvironmentVariable("dbhost");
            var dbUser = Environment.GetEnvironmentVariable("dbuser");
            var dbPass = Environment.GetEnvironmentVariable("dbpass");
            var dbPort = Environment.GetEnvironmentVariable("dbport");
            var useSsl = Environment.GetEnvironmentVariable("usessl");
            var dbName = Environment.GetEnvironmentVariable("db");

            return $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass}";
        }
    }
}
