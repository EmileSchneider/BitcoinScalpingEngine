using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;

namespace BitcoinScalpingEngine.Trading;

public class AutoRegressionTrader
{
    private IBinanceSocketClient socketClient;
    private IOrderPlacement orderPlacement;
    private ITradingModel tradingModel;

    
    async public Task StartTrader()
    {
        var subscription = await socketClient.SpotStreams.SubscribeToKlineUpdatesAsync("BTCUSDT",
            KlineInterval.OneMinute,
            data =>
            {
                if (data.Data.Data.Final)
                {
                    var signal = tradingModel.GenerateSignal(data.Data.Data.ClosePrice);
                    if (signal.Equals(TradeSignal.LONG))
                    {
                        
                    }
                }
            });
    }
}