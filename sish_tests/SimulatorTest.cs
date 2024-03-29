﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using sish;

namespace sish_tests
{
    [TestClass]
    public class SimulatorTest
    {
        [TestMethod]
        public void TestRunNoFees()
        {
            Simulator simulator = new Simulator();
            simulator.account.setStartingBalance(100.0f);
            List<(DateTime, float)> prices = new List<(DateTime, float)>
            {
                (DateTime.Parse("2022-06-24T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-27T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-28T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-29T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-30T00:00:00"), 1.0f),
                (DateTime.Parse("2022-07-01T00:00:00"), 1.0f),
                (DateTime.Parse("2022-07-04T00:00:00"), 1.0f),
                (DateTime.Parse("2022-07-05T00:00:00"), 0.9f),
                (DateTime.Parse("2022-07-06T00:00:00"), 0.9f),
                (DateTime.Parse("2022-07-07T00:00:00"), 1.1f)
            };
            simulator.Run("XYZ", prices);

            Assert.AreEqual(3, simulator.account.transactions.Count);
            Transaction transaction = simulator.account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("0.90", transaction.price.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            transaction = simulator.account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("0.90", transaction.price.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            transaction = simulator.account.transactions[2];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("1.10", transaction.price.ToString("0.00"));
            Assert.AreEqual("0.00", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);

            Assert.AreEqual("99.30", simulator.account.balance.ToString("0.00"));
            Assert.AreEqual(1, simulator.account.shareCount);
        }

        [TestMethod]

        public void TestRunFee()
        {
            Simulator simulator = new Simulator();
            simulator.account.setStartingBalance(100.0f);
            simulator.account.buyFeePercent = 5;
            simulator.account.sellFeePercent = 5;
            List<(DateTime, float)> prices = new List<(DateTime, float)>
            {
                (DateTime.Parse("2022-06-24T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-27T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-28T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-29T00:00:00"), 1.0f),
                (DateTime.Parse("2022-06-30T00:00:00"), 1.0f),
                (DateTime.Parse("2022-07-01T00:00:00"), 1.0f),
                (DateTime.Parse("2022-07-04T00:00:00"), 1.0f),
                (DateTime.Parse("2022-07-05T00:00:00"), 0.9f),
                (DateTime.Parse("2022-07-06T00:00:00"), 0.9f),
                (DateTime.Parse("2022-07-07T00:00:00"), 1.1f)
            };
            simulator.Run("XYZ", prices);

            Assert.AreEqual(3, simulator.account.transactions.Count);
            Transaction transaction = simulator.account.transactions[0];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("0.90", transaction.price.ToString("0.00"));
            Assert.AreEqual("0.05", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            transaction = simulator.account.transactions[1];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("0.90", transaction.price.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.PURCHASE, transaction.transactionType);
            transaction = simulator.account.transactions[2];
            Assert.AreEqual("XYZ", transaction.code);
            Assert.AreEqual(1, transaction.volume);
            Assert.AreEqual("1.10", transaction.price.ToString("0.00"));
            Assert.AreEqual("0.06", transaction.fee.ToString("0.00"));
            Assert.AreEqual(Transaction.TransactionType.SALE, transaction.transactionType);

            Assert.AreEqual("99.16", simulator.account.balance.ToString("0.00"));
            Assert.AreEqual(1, simulator.account.shareCount);
        }

        [TestMethod]

        public void TestShouldBuy()
        {
            Simulator simulator = new Simulator();
            Assert.AreEqual(false, simulator.ShouldBuy(1, 0.5f));
            Assert.AreEqual(false, simulator.ShouldBuy(1, 1));
            Assert.AreEqual(true, simulator.ShouldBuy(1, 1.5f));
        }

        [TestMethod]

        public void TestShouldSell()
        {
            Simulator simulator = new Simulator();
            Assert.AreEqual(true, simulator.ShouldSell(1, 0.5f));
            Assert.AreEqual(false, simulator.ShouldSell(1, 1));
            Assert.AreEqual(false, simulator.ShouldSell(1, 1.5f));
        }
    }
}
