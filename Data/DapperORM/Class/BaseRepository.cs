﻿using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.IO;

namespace HubUfpr.Data.DapperORM.Class
{
    public class BaseRepository
    {
        public static IConfigurationRoot Configuration { get; set; }

        public MySqlConnection GetMySqlConnection(bool open = true,
            bool convertZeroDatetime = false, bool allowZeroDatetime = false)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string cs = Configuration["Logging:AppSettings:MySqlConnectionString"];

            var csb = new MySqlConnectionStringBuilder(cs)
            {
                AllowZeroDateTime = allowZeroDatetime,
                ConvertZeroDateTime = convertZeroDatetime
            };
            var conn = new MySqlConnection(csb.ConnectionString);
                if (open) conn.Open();
            return conn;
        }
    }
}