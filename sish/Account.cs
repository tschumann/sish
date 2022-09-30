﻿using System;
using System.Collections.Generic;

namespace sish
{
    public class Account
    {
        public int shareCount { get; set; }
        public float balance { get; private set; }
        public int buyFeePercent { private get; set; }
        public int sellFeePercent { private get; set; }
        public int buyMargin { get; set; }
        public int sellMargin { get; set; }
        public int timePeriod { get; private set; }
        public List<Transaction> transactions { get; }

        public Account()
        {
            timePeriod = 7;
            transactions = new List<Transaction>();
        }

        [Obsolete("Use argumentless constructor instead")]
        public Account(int startingShareCount, float startingBalance)
        {
            shareCount = startingShareCount;
            balance = startingBalance;
            timePeriod = 7;
            transactions = new List<Transaction>();
        }

        public void setStartingBalance(float startingBalance)
        {
            if (transactions.Count > 0)
            {
                throw new Exception("Cannot set starting balance after starting");
            }
            balance = startingBalance;
        }

        public void setTimePeriod(int timePeriod)
        {
            if (timePeriod <= 0)
            {
                throw new Exception("Cannot set timePeriod to less than 1 day");
            }
            this.timePeriod = timePeriod;
        }

        public bool CanBuy(int count, float price)
        {
            float buyPrice = count * price;
            float fee = buyPrice * (buyFeePercent / 100.0f);
            return (buyPrice + fee) <= balance;
        }

        public bool CanSell(int count)
        {
            return shareCount >= count;
        }

        public void Buy(string code, int volume, float sharePrice, DateTime dateTime)
        {
            if (!CanBuy(volume, sharePrice))
            {
                throw new Exception("Not enough funds to buy");
            }

            float transactionPrice = (sharePrice * volume);
            float transactionFee = transactionPrice * (buyFeePercent / 100.0f);
            shareCount += volume;
            balance -= (transactionPrice + transactionFee);
            transactions.Add(new Transaction(code, volume, transactionPrice, transactionFee, 0.0f, dateTime, Transaction.TransactionType.PURCHASE));
        }

        public void Sell(string code, int volume, float sharePrice, DateTime dateTime)
        {
            if (!CanSell(volume))
            {
                throw new Exception("Not enough shares to sell");
            }

            float transactionPrice = (sharePrice * volume);
            float transactionFee = transactionPrice * (sellFeePercent / 100.0f);
            float tax = 0.0f;
            shareCount -= volume;
            balance += (transactionPrice - transactionFee);
            transactions.Add(new Transaction(code, volume, transactionPrice, transactionFee, tax, dateTime, Transaction.TransactionType.SALE));
        }

        public override string ToString()
        {
            return $"Balance: ${balance}, Share Count: {shareCount}";
        }
    }
}
