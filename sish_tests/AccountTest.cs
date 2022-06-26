using Microsoft.VisualStudio.TestTools.UnitTesting;

using sish;

namespace sish_tests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void TestCanBuy()
        {
            Account account = new Account(0, 100);

            Assert.IsTrue(account.CanBuy(1, 20));
            Assert.IsFalse(account.CanBuy(1, 101));
        }

        [TestMethod]
        public void TestCanSell()
        {
            Account account = new Account(5, 0);

            Assert.IsTrue(account.CanSell(1));
            Assert.IsFalse(account.CanSell(10));
        }
    }
}
