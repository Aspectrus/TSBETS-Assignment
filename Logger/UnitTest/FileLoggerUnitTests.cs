using LogTest;
using LogTest.TimeProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Threading;

namespace UnitTest
{
    [TestClass]
    public class FileLoggerUnitTests
    {
        [TestMethod]
        public void Should_CreateFileWithData_When_WriteMethodIsCalled()
        {
            var methodFolder = TestHelper.GenerateTestMethodFolder();
            ILogWriter logWriter = new FileLogWriter(methodFolder);

            using (var logger = new AsyncLog(logWriter))
            {
                logger.Write("test");
                logger.Write("test");
                Thread.Sleep(300);
            }

            var fileNames = Directory.GetFiles(methodFolder, "*");

            //File count is one
            Assert.AreEqual(1, fileNames.Length);

            var lineCount = File.ReadAllLines(fileNames[0]).Length;

            //Should be 3 lines in the file
            Assert.AreEqual(3, lineCount);

            Directory.Delete(methodFolder, true);
        }


        [TestMethod]
        public void Should_CreateTwoFilesWithData_When_DateCrossesMidnight()
        {
            var methodFolder = TestHelper.GenerateTestMethodFolder();
            ILogWriter logWriter = new FileLogWriter(methodFolder);

            var file1Date = new DateTime(2023, 01, 10, 23, 58, 56);
            var file2Date = new DateTime(2023, 01, 11, 01, 02, 02);
            var timeMock = new Mock<TimeProvider>();

            timeMock.SetupGet(tp => tp.UtcNow).Returns(file1Date);
            TimeProvider.Current = timeMock.Object;

            using (var logger = new AsyncLog(logWriter))
            {
                logger.Write("test");
                Thread.Sleep(300);

                timeMock.SetupGet(tp => tp.UtcNow).Returns(file2Date);
                logger.Write("test");
                logger.Write("test");
                Thread.Sleep(1500);
            }

            var file1Name = Directory.GetFiles(methodFolder, $"*{file1Date.ToString("yyyyMMdd HHmmss fff")}*");
            Assert.AreEqual(file1Name.Length, 1);
            var file2Name = Directory.GetFiles(methodFolder, $"*{file2Date.ToString("yyyyMMdd HHmmss fff")}*");
            Assert.AreEqual(file2Name.Length, 1);

            var lineCount1 = File.ReadAllLines(file1Name[0]).Length;

            Assert.AreEqual(2, lineCount1);
            var lineCount2 = File.ReadAllLines(file2Name[0]).Length;

            Assert.AreEqual(3, lineCount2);
            
            Directory.Delete(methodFolder, true);
        }



        [TestMethod]
        public void Should_WriteAllTheRemainingLogsToFile_When_WhenLoggerIsStoppedWithFlush()
        {
            var methodFolder = TestHelper.GenerateTestMethodFolder();
            ILogWriter logWriter = new FileLogWriter(methodFolder);


            using (var logger = new AsyncLog(logWriter))
            {
                for (int i = 0; i < 30; i++)
                {
                    logger.Write("Number with Flush: " + i.ToString());
                    Thread.Sleep(300);
                }
                logger.StopWithFlush();
            }

            var fileNames = Directory.GetFiles(methodFolder, "*");

            //File count is one
            Assert.AreEqual(1, fileNames.Length);

            var lineCount = File.ReadAllLines(fileNames[0]).Length;

            Assert.AreEqual(31, lineCount);
        }

        [TestMethod]
        public void Should_SkipWritingRemainingLogsToFile_When_WhenLoggerIsStoppedWithoutFlush()
        {
            var methodFolder = TestHelper.GenerateTestMethodFolder();
            ILogWriter logWriter = new FileLogWriter(methodFolder);

            using (var logger = new AsyncLog(logWriter))
            {
                for (int i = 0; i < 30; i++)
                {
                    logger.Write("Numbers Without Flush: " + i.ToString());
                }
                logger.StopWithoutFlush();
            }

            var fileNames = Directory.GetFiles(methodFolder, "*");

            //File count is one
            Assert.AreEqual(1, fileNames.Length);

            var lineCount = File.ReadAllLines(fileNames[0]).Length;

            Assert.IsTrue(31 > lineCount);
        }


        [TestCleanup]
        public void TearDown()
        {
            TimeProvider.ResetToDefault();
        }

    }
}
