namespace BitcoinScalpingEngine;

public interface ITrader
{
    public void LimitBuy(string symbol, decimal quantity, decimal price);
    public void LimitSell(string symbol, decimal quantity, decimal price);
}