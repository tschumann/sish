using System.Collections.Generic;

namespace sish
{
    public class Account
    {
        int shareCount { get; set; }
        float balance { get; set; }
        public List<Transaction> transactions { get; }

        public Account(int startingCount, float startingBalance)
        {
            shareCount = startingCount;
            balance = startingBalance;
            transactions = new List<Transaction>();
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
                throw new System.Exception("Not enough funds to buy");
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
                throw new System.Exception("Not enough shares to sell");
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
