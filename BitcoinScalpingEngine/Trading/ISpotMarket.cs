namespace BitcoinScalpingEngine.Trading;

public interface ISpotMarket
{
    public TradeResult BuyMarket(decimal quantity);
    public TradeResult SellMarket(decimal quantity);
}