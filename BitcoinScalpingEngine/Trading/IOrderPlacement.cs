namespace BitcoinScalpingEngine.Trading;

public interface IOrderPlacement
{
    public void MarketOrderLong(string symbol, decimal quantity);
    public void MarketOrderShort(string symbol, decimal quantity);
    public void LimitOrderLong(string symbol, decimal quantity, decimal limitPrice);
    public void LimitOrderShort(string symbol, decimal quantity, decimal limitPrice);
}