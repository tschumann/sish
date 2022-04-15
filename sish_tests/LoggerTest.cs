using Microsoft.VisualStudio.TestTools.UnitTesting;

using sish;

namespace sish_tests
{
    [TestClass]
    public class LoggerTest
    {
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
