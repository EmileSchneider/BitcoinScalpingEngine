namespace BitcoinScalpingEngine;

public interface ITradingModel
{
    public TradeSignal GenerateSignal(decimal value);
}