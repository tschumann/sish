using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using sish;

namespace sish_tests
{
    [TestClass]
    public class SimulatorTest
    {
        [TestMethod]
        public void TestRun()
        {
            Simulator simulator = new Simulator();
            List<(string, float)> prices = new List<(string, float)>
            {
                ("2022-06-24T00:00:00", 14.2f),
                ("2022-06-27T00:00:00", 15.25f),
                ("2022-06-28T00:00:00", 14.8f),
                ("2022-06-29T00:00:00", 14.65f),
                ("2022-06-30T00:00:00", 14.5f),
                ("2022-07-01T00:00:00", 14.4f),
                ("2022-07-04T00:00:00", 14.67f),
                ("2022-07-05T00:00:00", 14.29f),
                ("2022-07-06T00:00:00", 15.0f),
                ("2022-07-07T00:00:00", 15.86f),
                ("2022-07-08T00:00:00", 15.64f)
            };
            simulator.Run("XYZ", prices);

            Assert.AreEqual(3, simulator.account.transactions.Count);
            Transaction transaction = simulator.account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("14.29", transaction.price.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            transaction = simulator.account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("15.86", transaction.price.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
            transaction = simulator.account.transactions[2];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("15.64", transaction.price.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
        }
    }
}
