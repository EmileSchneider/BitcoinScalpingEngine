namespace BitcoinScalpingEngine.Trading;

public class TradeResult
{
    public decimal AveragePrice { get; set; }
    public decimal FillQuantity { get; set; }
}

public class SpotMarket : ISpotMarket
{
    private IOrderBook orderBook;

    public SpotMarket(IOrderBook orderBook)
    {
        this.orderBook = orderBook;
    }

    public TradeResult BuyMarket(decimal quantity)
    {
        return TradeResult(quantity, orderBook.Asks());
    }

    private TradeResult TradeResult(decimal quantity, List<OrderBookLevel> prices)
    {
        var rest = quantity;
        var averagePrice = 0m;
        var totalQuantity = 0m;
        var i = 0;
        while (rest > 0 && i < prices.Count)
        {
            if (rest < prices[i].Quantity)
            {
                averagePrice += rest * prices[i].Price;
                totalQuantity += rest;
                rest = 0;
            }
            else
            {
                rest -= prices[i].Quantity;
                averagePrice += prices[i].Quantity * prices[i].Price;
                totalQuantity += prices[i].Quantity;
            }
            i++;
        }

        return new TradeResult()
        {
            AveragePrice = averagePrice / totalQuantity,
            FillQuantity = quantity - rest
        };
    }

    public TradeResult SellMarket(decimal quantity)
    {
        return TradeResult(quantity, orderBook.Bids());
    }
}