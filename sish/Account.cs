namespace sish
{
    public class Account
    {
        int shareCount { get; set; }
        float balance { get; set; }

        public Account(int startingCount, float startingBalance)
        {
            shareCount = startingCount;
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

        public void Buy(int count, float price)
        {
            if (!CanBuy(count, price))
            {
                throw new System.Exception("Not enough funds to buy");
            }

            shareCount += count;
            balance -= (price * count);
        }

        public void Sell(int count, float price)
        {
            if (!CanSell(count))
            {
                throw new System.Exception("Not enough shares to sell");
            }

            shareCount -= count;
            balance += (price * count);
        }

        public override string ToString()
        {
            return $"Balance: {balance}, Share Count: {shareCount}";
        }
    }
}
