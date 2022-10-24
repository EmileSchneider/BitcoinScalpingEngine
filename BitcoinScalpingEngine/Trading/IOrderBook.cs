namespace BitcoinScalpingEngine.Trading;

public interface IOrderBook
{
    public List<OrderBookLevel> Bids();
    public List<OrderBookLevel> Asks();
}




public class OrderBookLevel
{
    public OrderBookLevel(decimal price, decimal quantity)
    {
        Price = price;
        Quantity = quantity;
    }

    public decimal Price { get; }
    public decimal Quantity { get; }
}
