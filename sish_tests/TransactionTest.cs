using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using sish;

namespace sish_tests
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void testVolumeSoldPurchase()
        {
            Transaction transaction = new Transaction("XYZ", 1, 1.0f, 0.0f, 0.0f, DateTime.Parse("2022-09-30 21:31:00"), Transaction.TransactionType.PURCHASE);
            Assert.AreEqual(0, transaction.getVolumeSold());

            transaction.addToVolumeSold(1);
            Assert.AreEqual(1, transaction.getVolumeSold());

            bool threw = false;

            try
            {
                transaction.addToVolumeSold(1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual("Specified argument was out of the range of valid values.\r\nParameter name: Cannot sell more than was originally purchased", ex.Message);
                threw = true;
            }
            Assert.IsTrue(threw);
        }

        [TestMethod]
        public void testVolumeSoldOther()
        {
            Transaction transaction = new Transaction("XYZ", 1, 1.0f, 0.0f, 0.0f, DateTime.Parse("2022-09-30 21:31:00"), Transaction.TransactionType.SALE);

            bool threw = false;

            try
            {
                transaction.getVolumeSold();
            }
            catch (NotSupportedException ex)
            {
                Assert.AreEqual("Only purchases can be sold", ex.Message);
                threw = true;
            }
            Assert.IsTrue(threw);

            threw = false;

            try
            {
                transaction.addToVolumeSold(1);
            }
            catch (NotSupportedException ex)
            {
                Assert.AreEqual("Only purchases can be sold", ex.Message);
                threw = true;
            }
            Assert.IsTrue(threw);

            transaction = new Transaction("XYZ", 1, 1.0f, 0.0f, 0.0f, DateTime.Parse("2022-09-30 21:31:00"), Transaction.TransactionType.DIVIDEND);

            threw = false;

            try
            {
                transaction.getVolumeSold();
            }
            catch (NotSupportedException ex)
            {
                Assert.AreEqual("Only purchases can be sold", ex.Message);
                threw = true;
            }
            Assert.IsTrue(threw);

            threw = false;

            try
            {
                transaction.addToVolumeSold(1);
            }
            catch (NotSupportedException ex)
            {
                Assert.AreEqual("Only purchases can be sold", ex.Message);
                threw = true;
            }
            Assert.IsTrue(threw);
        }

        [TestMethod]
        public void TestToString()
        {
            Transaction transaction = new Transaction("XYZ", 1, 1.0f, 0.0f, 0.0f, DateTime.Parse("2022-09-30 21:31:00"), Transaction.TransactionType.PURCHASE);
            Assert.AreEqual("Code: XYZ, Volume: 1, Price: $1, Fee: $0, Tax: $0, Time: 9/30/2022 9:31:00 PM, Type: PURCHASE", transaction.ToString());

            transaction = new Transaction("ABC", 3, 1.5f, 0.0f, 0.1f, DateTime.Parse("2022-09-30 21:31:00"), Transaction.TransactionType.SALE);
            Assert.AreEqual("Code: ABC, Volume: 3, Price: $1.5, Fee: $0, Tax: $0.1, Time: 9/30/2022 9:31:00 PM, Type: SALE", transaction.ToString());

            transaction = new Transaction("123", 3, 0.01f, 0.0f, 0.1f, DateTime.Parse("2022-09-30 21:32:00"), Transaction.TransactionType.DIVIDEND);
            Assert.AreEqual("Code: 123, Volume: 3, Price: $0.01, Fee: $0, Tax: $0.1, Time: 9/30/2022 9:32:00 PM, Type: DIVIDEND", transaction.ToString());
        }
    }
}
