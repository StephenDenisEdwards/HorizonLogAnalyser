using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Horizon.Utility.LogAnylyser.Tests
{
    [TestClass]
    public class W3cLogFieldListReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void W3cLogFieldListReader_Ctor_Failure()
        {
            // Arrange
            var logStreamReaderMock = new Mock<ILogStreamReader>();

            logStreamReaderMock.Setup(p => p.EndOfStream).Returns(true);

            // Act
            var uut = new W3cLogFieldListReader(logStreamReaderMock.Object);
        }

        [TestMethod]
        public void W3cLogFieldListReader_Ctor_Success()
        {
            // Arrange
            var logStreamReaderMock = new Mock<ILogStreamReader>();

            logStreamReaderMock.Setup(p => p.EndOfStream).Returns(false);
            logStreamReaderMock.Setup(p => p.ReadLine()).Returns("#Fields: date time     cs-method"); // Additional spacing intended

            // Act
            var uut = new W3cLogFieldListReader(logStreamReaderMock.Object);

            // Assert
            Assert.AreEqual("date", uut[0]);
            Assert.AreEqual("time", uut[1]);
            Assert.AreEqual("cs-method", uut[2]);
        }
    }
}