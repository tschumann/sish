using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SISH.Tests
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void TestTrace()
        {
            Logger.Trace("test");
        }

        [TestMethod]
        public void TestInfo()
        {
            Logger.Info("test");
        }

        [TestMethod]
        public void TestError()
        {
            Logger.Error("test");
        }
    }
}
