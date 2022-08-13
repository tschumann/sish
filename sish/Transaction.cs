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
            SALE
        }

        public string code { get; set; }
        public int volume { get; set; }
        public float price { get; set; }
        public float fee { get; set; }
        public TransactionType transactionType { get; set; }

        public Transaction(string code, int volume, float price, float fee, TransactionType transactionType)
        {
            this.code = code;
            this.volume = volume;
            this.price = price;
            this.fee = fee;
            this.transactionType = transactionType;
        }

        public override string ToString()
        {
            return $"Code: {code}, Volume: {volume}, Price: ${price}, Fee: ${fee}, Type: {transactionType.ToString()}";
        }
    }
}
