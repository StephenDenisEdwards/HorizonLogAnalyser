using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Horizon.Utility.LogAnylyser.Tests
{
    [TestClass]
    public class LogAnalyserAsyncTests
    {
        [TestMethod]
        public async Task LogAnalyserAsync_GetAsyncEnumerator_ReturnRow_Success()
        {
            // Arrange
            var LogAnalyserAsync = new LogAnalyserAsync();
            var logFieldList = new Mock<ILogFieldList>();
            var logStreamReader = new Mock<ILogStreamReader>();

            logStreamReader.SetupSequence<bool>(p => p.EndOfStream)
                .Returns(false)
                .Returns(false);  // TODO: Sequencing doesn't appear to work
            logStreamReader.Setup(p => p.ReadLineAsync())
                .Returns(Task.FromResult("a b c"));
            logFieldList.SetupGet(p => p[0])
                .Returns("date");
            logFieldList.SetupGet(p => p[1])
                .Returns("time");
            logFieldList.SetupGet(p => p[2])
                .Returns("cs-method");

            logFieldList.Setup(p => p.Values(It.IsAny<string>())).Returns(new [] {"v_date", "v_time", "v_cs-method" });

            var lofFactoryReturn = new Tuple<ILogFieldList, ILogStreamReader>(logFieldList.Object, logStreamReader.Object);

            LogAnalyserAsync.Add(lofFactoryReturn);

            // Act
            Dictionary<string, string> lineItem = null;
            await foreach (Dictionary<string, string> item in LogAnalyserAsync)
            {
                lineItem = item;
                break;
            }

            // Assert
            Assert.AreEqual("v_date", lineItem["date"]);
            Assert.AreEqual("v_time", lineItem["time"]);
            Assert.AreEqual("v_cs-method", lineItem["cs-method"]);
        }
    }
}