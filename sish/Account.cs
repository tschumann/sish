using System;
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
            float buyPrice = count * price;
            float fee = buyPrice * (buyFeePercent / 100.0f);
            return (buyPrice + fee) <= balance;
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
            float transactionFee = transactionPrice * (buyFeePercent / 100.0f);
            shareCount += volume;
            balance -= (transactionPrice + transactionFee);
            transactions.Add(new Transaction(code, volume, transactionPrice, transactionFee, Transaction.TransactionType.PURCHASE));
        }

        public void Sell(string code, int volume, float sharePrice)
        {
            if (!CanSell(volume))
            {
                throw new Exception("Not enough shares to sell");
            }

            float transactionPrice = (sharePrice * volume);
            float transactionFee = transactionPrice * (sellFeePercent / 100.0f);
            shareCount -= volume;
            balance += (transactionPrice - transactionFee);
            transactions.Add(new Transaction(code, volume, transactionPrice, transactionFee, Transaction.TransactionType.SALE));
        }

        public override string ToString()
        {
            return $"Balance: ${balance}, Share Count: {shareCount}";
        }
    }
}
