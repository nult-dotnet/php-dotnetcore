﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAbstractDBProvider
{
    public static class GetStringAppsetting
    {
        public static string DatabaseDefault()
        {
            var databaseDefault = ConnectString().GetSection("DatabaseDefault").Value;
            return databaseDefault;
        }
        public static IConfigurationRoot ConnectString()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            return config;
        }
    }
}
