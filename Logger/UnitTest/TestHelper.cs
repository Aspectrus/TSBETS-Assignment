using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace UnitTest
{
    public static class TestHelper
    {
        public static string GenerateTestMethodFolder()
        {
            var logTestFolder = GetConfigValue("UnitTestLogFolder");

            var directory = Path.Combine(logTestFolder, Guid.NewGuid().ToString());

            while (Directory.Exists(directory))
            {
                directory = Path.Combine(logTestFolder, Guid.NewGuid().ToString());
            }
            return directory;
        }
        private static string GetConfigValue(string key)
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")[key];
        }

    }
}
