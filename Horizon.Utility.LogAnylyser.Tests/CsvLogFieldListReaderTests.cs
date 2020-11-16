using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Horizon.Utility.LogAnylyser.Tests
{
    /*
            [UnitOfWork]_[Method]_[Behaviour]
    */

    [TestClass]
    public class CsvLogFieldListReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))] // Assert
        public void CsvLogFieldListReaderTests_Ctor_Failure()
        {
            // Arrange
            var logStreamReaderMock = new Mock<ILogStreamReader>();

            logStreamReaderMock.Setup(p => p.EndOfStream).Returns(true);

            // Act
            var uut = new CsvLogFieldListReader(logStreamReaderMock.Object);
        }

        [TestMethod]
        public void CsvLogFieldListReaderTests_Ctor_Success()
        {
            // Arrange
            var logStreamReaderMock = new Mock<ILogStreamReader>();

            logStreamReaderMock.Setup(p => p.EndOfStream).Returns(false);
            logStreamReaderMock.Setup(p => p.ReadLine()).Returns("date,     time,cs-method"); // Additional spacing intended

            // Act
            var uut = new CsvLogFieldListReader(logStreamReaderMock.Object);

            // Assert
            Assert.AreEqual("date", uut[0]);
            Assert.AreEqual("time", uut[1]);
            Assert.AreEqual("cs-method", uut[2]);
        }
    }
}