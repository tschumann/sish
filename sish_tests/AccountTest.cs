using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void TestSetTimePeriod()
        {
            Account account = new Account();
            Assert.AreEqual(7, account.timePeriod);

            account.setTimePeriod(30);
            Assert.AreEqual(30, account.timePeriod);
        }

        [TestMethod]
        public void TestCanBuy()
        {
            Account account = new Account();
            account.setStartingBalance(100.0f);

            Assert.IsTrue(account.CanBuy(1, 20));
            Assert.IsFalse(account.CanBuy(1, 101));

            account.buyFeePercent = 10;

            Assert.IsTrue(account.CanBuy(1, 20));
            Assert.IsFalse(account.CanBuy(1, 95));
        }

        [TestMethod]
        public void TestCanSell()
        {
            Account account = new Account()
            {
                shareCount = 5
            };
            account.setStartingBalance(1);

            Assert.IsTrue(account.CanSell(1));
            Assert.IsFalse(account.CanSell(10));
        }

        [TestMethod]
        public void TestBuy()
        {
            Account account = new Account();
            account.setStartingBalance(5.0f);

            account.Buy("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:35:00"));

            Assert.AreEqual(1, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:35:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(0, transaction.getVolumeSold());

            account.Buy("XYZ", 2, 2, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(2, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:35:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(0, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(0, transaction.getVolumeSold());
        }

        [TestMethod]
        public void TestBuyWithFee()
        {
            Account account = new Account();
            account.setStartingBalance(10.0f);
            account.buyFeePercent = 5;

            account.Buy("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(1, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(0, transaction.getVolumeSold());

            account.Buy("XYZ", 2, 2, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(2, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(0, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual("0.20", transaction.fee.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(0, transaction.getVolumeSold());
        }

        [TestMethod]
        public void TestSell()
        {
            Account account = new Account();
            account.setStartingBalance(5);
            account.Buy("XYZ", 5, 1, DateTime.Parse("2021-09-30 21:32:00"));

            account.Sell("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(2, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(5, transaction.volume);
            Assert.AreEqual(5.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(1, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);

            account.Sell("XYZ", 2, 2, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(3, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(5, transaction.volume);
            Assert.AreEqual(5.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(3, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
            transaction = account.transactions[2];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
        }

        [TestMethod]
        public void TestSellWithFee()
        {
            Account account = new Account();
            account.setStartingBalance(5);
            account.Buy("XYZ", 5, 1, DateTime.Parse("2021-09-30 21:32:00"));
            account.sellFeePercent = 5;

            account.Sell("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(2, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(5, transaction.volume);
            Assert.AreEqual(5.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(1, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);

            account.Sell("XYZ", 2, 2, DateTime.Parse("2022-09-30 21:36:00"));

            Assert.AreEqual(3, account.transactions.Count);
            transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(5, transaction.volume);
            Assert.AreEqual(5.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(3, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
            transaction = account.transactions[2];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(2, transaction.volume);
            Assert.AreEqual(4.0, transaction.price);
            Assert.AreEqual("0.20", transaction.fee.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:36:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
        }

        [TestMethod]
        public void TestSellWithSmallBuyVolumeHighSellVolume()
        {
            Account account = new Account();
            account.setStartingBalance(3);
            account.Buy("XYZ", 1, 1, DateTime.Parse("2021-09-30 21:32:00"));
            account.Buy("XYZ", 1, 1, DateTime.Parse("2021-09-30 21:32:00"));
            account.Buy("XYZ", 1, 1, DateTime.Parse("2021-09-30 21:32:00"));

            account.Sell("XYZ", 3, 1, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(4, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(1, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(1, transaction.getVolumeSold());
            transaction = account.transactions[2];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(1, transaction.getVolumeSold());
            transaction = account.transactions[3];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(3, transaction.volume);
            Assert.AreEqual(3.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
        }

        [TestMethod]
        public void TestSellWithEqualBuySellVolume()
        {
            Account account = new Account();
            account.setStartingBalance(1);
            account.Buy("XYZ", 1, 1, DateTime.Parse("2021-09-30 21:32:00"));

            account.Sell("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(2, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(1, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
        }

        [TestMethod]
        public void TestSellWithHighBuyVolumeLowSellVolume()
        {
            Account account = new Account();
            account.setStartingBalance(3);
            account.Buy("XYZ", 3, 1, DateTime.Parse("2021-09-30 21:32:00"));

            account.Sell("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:32:00"));
            account.Sell("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:32:00"));
            account.Sell("XYZ", 1, 1, DateTime.Parse("2022-09-30 21:32:00"));

            Assert.AreEqual(4, account.transactions.Count);
            Transaction transaction = account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(3, transaction.volume);
            Assert.AreEqual(3.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2021-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            Assert.AreEqual(3, transaction.getVolumeSold());
            transaction = account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
            transaction = account.transactions[2];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
            transaction = account.transactions[3];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual(1.0, transaction.price);
            Assert.AreEqual(0.0, transaction.fee);
            Assert.AreEqual("0.00", transaction.tax.ToString("0.00"));
            Assert.AreEqual("2022-09-30T21:32:00", transaction.dateTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);
        }

        [TestMethod]
        public void TestToString()
        {
            Account account = new Account();
            account.setStartingBalance(1.0f);
            Assert.AreEqual("Balance: $1, Share Count: 0", account.ToString());
        }
    }
}
