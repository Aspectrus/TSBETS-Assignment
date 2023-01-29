using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public static class TestHelper
    {
        private const string _logUnitLocation = @"C:\LogUnitTest";

        public static string GenerateTestMethodFolder()
        {
            var directory = Path.Combine(_logUnitLocation, Guid.NewGuid().ToString());

            while (Directory.Exists(directory))
            {
                directory = Path.Combine(_logUnitLocation, Guid.NewGuid().ToString());
            }
            return directory;
        }

    }
}
