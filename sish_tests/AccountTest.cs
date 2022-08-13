using Microsoft.VisualStudio.TestTools.UnitTesting;

using sish;

namespace sish_tests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void TestSetStartingBalance()
        {
            Account account = new Account();
            Assert.AreEqual(0.0f, account.balance);

            account.setStartingBalance(100.0f);
            Assert.AreEqual(100.0f, account.balance);
        }

        [TestMethod]
        public void TestCanBuy()
        {
            Account account = new Account(0, 100);

            Assert.IsTrue(account.CanBuy(1, 20));
            Assert.IsFalse(account.CanBuy(1, 101));

            account.buyFeePercent = 10;

            Assert.IsTrue(account.CanBuy(1, 20));
            Assert.IsFalse(account.CanBuy(1, 95));
        }

        [TestMethod]
        public void TestCanSell()
        {
            Account account = new Account(5, 1);

            Assert.IsTrue(account.CanSell(1));
            Assert.IsFalse(account.CanSell(10));
        }

        [TestMethod]
        public void TestBuy()
        {
            Account account = new Account(0, 5);

            account.Buy("XYZ", 1, 1);

            Assert.AreEqual(1, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);

            account.Buy("ABC", 2, 2);

            Assert.AreEqual(2, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            transaction = account.transactions[1];
            Assert.AreEqual("ABC", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
        }

        [TestMethod]
        public void TestBuyWithFee()
        {
            Account account = new Account(0, 10);
            account.buyFeePercent = 5;

            account.Buy("XYZ", 1, 1);

            Assert.AreEqual(1, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);

            account.Buy("ABC", 2, 2);

            Assert.AreEqual(2, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            transaction = account.transactions[1];
            Assert.AreEqual("ABC", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual("0.20", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
        }

        [TestMethod]
        public void TestSell()
        {
            Account account = new Account(5, 0);

            account.Sell("XYZ", 1, 1);

            Assert.AreEqual(1, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);

            account.Sell("ABC", 2, 2);

            Assert.AreEqual(2, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
            transaction = account.transactions[1];
            Assert.AreEqual("ABC", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
        }

        [TestMethod]
        public void TestSellWithFee()
        {
            Account account = new Account(5, 0);
            account.sellFeePercent = 5;

            account.Sell("XYZ", 1, 1);

            Assert.AreEqual(1, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);

            account.Sell("ABC", 2, 2);

            Assert.AreEqual(2, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
            transaction = account.transactions[1];
            Assert.AreEqual("ABC", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual("0.20", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
        }

        [TestMethod]
        public void TestToString()
        {
            Account account = new Account(1, 1.0f);
            Assert.AreEqual("Balance: $1, Share Count: 1", account.ToString());
        }
    }
}
