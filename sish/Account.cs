using System;
using System.Collections.Generic;

namespace sish
{
    public class Account
    {
        int shareCount { get; set; }
        public float balance { get; private set; }
        int buyFeePercent { get; set; }
        int sellFeePercent { get; set; }
        int buyMargin { get; set; }
        int sellMargin { get; set; }
        public List<Transaction> transactions { get; }

        public Account()
        {
            transactions = new List<Transaction>();
        }

        [Obsolete("Use argumentless constructor instead")]
        public Account(int startingShareCount, float startingBalance)
        {
            shareCount = startingShareCount;
            balance = startingBalance;
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

        public bool CanBuy(int count, float price)
        {
            return (count * price) <= balance;
        }

        public bool CanSell(int count)
        {
            return shareCount >= count;
        }

        public void Buy(string code, int volume, float sharePrice)
        {
            if (!CanBuy(volume, sharePrice))
            {
                throw new Exception("Not enough funds to buy");
            }

            float transactionPrice = (sharePrice * volume);
            shareCount += volume;
            balance -= transactionPrice;
            transactions.Add(new Transaction(code, volume, transactionPrice, Transaction.TransactionType.PURCHASE));
        }

        public void Sell(string code, int volume, float sharePrice)
        {
            if (!CanSell(volume))
            {
                throw new Exception("Not enough shares to sell");
            }

            float transactionPrice = (sharePrice * volume);
            shareCount -= volume;
            balance += transactionPrice;
            transactions.Add(new Transaction(code, volume, transactionPrice, Transaction.TransactionType.SALE));
        }

        public override string ToString()
        {
            return $"Balance: ${balance}, Share Count: {shareCount}";
        }
    }
}
