using System;
using System.ComponentModel;

namespace sish
{
    public class Transaction
    {
        public enum TransactionType
        {
            [Description("Purchase")]
            PURCHASE,
            [Description("Sale")]
            SALE,
            [Description("Dividend")]
            DIVIDEND
        }

        public string code { get; set; }
        public int volume { get; set; }
        public float price { get; set; }
        public float fee { get; set; }
        public float tax { get; set; }
        public DateTime dateTime { get; set; }
        public TransactionType transactionType { get; set; }

        private int volumeSold;

        public Transaction(string code, int volume, float price, float fee, float tax, DateTime dateTime, TransactionType transactionType)
        {
            this.code = code;
            this.volume = volume;
            this.price = price;
            this.fee = fee;
            this.tax = tax;
            this.dateTime = dateTime;
            this.transactionType = transactionType;
            this.volumeSold = 0;
        }

        public int getVolumeSold()
        {
            if (transactionType != TransactionType.PURCHASE)
            {
                throw new NotSupportedException("Only purchases can be sold");
            }

            return volumeSold;
        }

        public void addToVolumeSold(int volumeSold)
        {
            if (transactionType != TransactionType.PURCHASE)
            {
                throw new NotSupportedException("Only purchases can be sold");
            }

            if (this.volumeSold + volumeSold > this.volume)
            {
                throw new ArgumentOutOfRangeException("Cannot sell more than was originally purchased");
            }

            this.volumeSold += volumeSold;
        }

        public override string ToString()
        {
            return $"Code: {code}, Volume: {volume}, Price: ${price}, Fee: ${fee}, Tax: ${tax}, Time: {dateTime}, Type: {transactionType.ToString()}";
        }
    }
}
