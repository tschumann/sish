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
        public int timePeriod { get; private set; }
        public List<Transaction> transactions { get; }

        public Account()
        {
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

            int volumeToProcess = volume;

            foreach (Transaction transaction in transactions)
            {
                if (transaction.transactionType == Transaction.TransactionType.PURCHASE)
                {
                    // if all shares in the original purchase have been sold
                    if (transaction.getVolumeSold() == transaction.volume)
                    {
                        // keep looking for another purchase
                        continue;
                    }
                    else
                    {
                        // work out how many more shares are left to be sold from this purchase
                        int volumeLeftInPurchase = transaction.volume - transaction.getVolumeSold();

                        // if the number of shares left to process from this sale fits into this purchase
                        if (volumeToProcess <= volumeLeftInPurchase)
                        {
                            // account for the number of shares left to process
                            transaction.addToVolumeSold(volumeToProcess);
                            volumeToProcess -= volumeToProcess;
                        }
                        else
                        {
                            // account for the number of shares that will fit
                            transaction.addToVolumeSold(volumeLeftInPurchase);
                            volumeToProcess -= volumeLeftInPurchase;
                        }
                    }
                }
            }

            if (volumeToProcess != 0)
            {
                throw new InvalidOperationException("Trying to sell more shares than are held");
            }
        }

        public override string ToString()
        {
            return $"Balance: ${balance}, Share Count: {shareCount}";
        }
    }
}
