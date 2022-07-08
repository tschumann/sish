using Microsoft.VisualStudio.TestTools.UnitTesting;

using sish;

namespace sish_tests
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void TestToString()
        {
            Transaction transaction = new Transaction("XYZ", 1, 1.0f, Transaction.TransactionType.PURCHASE);
            Assert.AreEqual("Code: XYZ, Volume: 1, Price: $1, Type: PURCHASE", transaction.ToString());
            transaction = new Transaction("ABC", 3, 1.5f, Transaction.TransactionType.SALE);
            Assert.AreEqual("Code: ABC, Volume: 3, Price: $1.5, Type: SALE", transaction.ToString());
        }
    }
}
