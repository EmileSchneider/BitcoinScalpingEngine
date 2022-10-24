namespace BitcoinScalpingEngine.Trading;


public class TradeManager : IOrderPlacement
{
    private decimal BTC;
    private decimal USDT;
    private ISpotMarket spotMarket;
    
    public void MarketOrderLong(string symbol, decimal quantity)
    {
        var tr = spotMarket.BuyMarket(quantity);
    }

    public void MarketOrderShort(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public void LimitOrderLong(string symbol, decimal quantity, decimal limitPrice)
    {
        throw new NotImplementedException();
    }

    public void LimitOrderShort(string symbol, decimal quantity, decimal limitPrice)
    {
        throw new NotImplementedException();
    }
}