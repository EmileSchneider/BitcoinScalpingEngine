using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using Binance.Net.SymbolOrderBooks;
using BitcoinScalpingEngine;
using BitcoinScalpingEngine.Trading;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services
            .AddSingleton<ITradingModel, AutoRegressionModel>()
            .AddBinance((restClientOptions, socketClientOptions) =>
            {
                restClientOptions.ApiCredentials = new ApiCredentials(
                    "VHUcinQVOatTlTbc3bragBqcljvw41OatC8BofhM1DbfdVYU878IFgmUqrE6s6xB",
                    "JeIsZMPqEGwy4g4VI5qSC3QsYN8Jt5xfUnZyuo5mDqqF9Sloc3KEjrQG54sMaUli");
                restClientOptions.HttpClient = new HttpClient();
                restClientOptions.HttpClient.BaseAddress = new Uri("https://testnet.binance.vision/api");
                restClientOptions.LogLevel = LogLevel.Trace;
                socketClientOptions.ApiCredentials = new ApiCredentials(
                    "VHUcinQVOatTlTbc3bragBqcljvw41OatC8BofhM1DbfdVYU878IFgmUqrE6s6xB",
                    "JeIsZMPqEGwy4g4VI5qSC3QsYN8Jt5xfUnZyuo5mDqqF9Sloc3KEjrQG54sMaUli");
                socketClientOptions.LogLevel = LogLevel.Trace;
            })
            .AddLogging(cfg =>
            {
                cfg.ClearProviders();
            })
    )
    .Build();


var binanceSocket = host.Services.GetRequiredService<IBinanceSocketClient>();
var binanceClient = host.Services.GetRequiredService<IBinanceClient>();
var tradingModel = host.Services.GetRequiredService<ITradingModel>();

var orderBook = new BinanceSpotSymbolOrderBook("BTCUSDT");
await orderBook.StartAsync();
var spotMarket = new SpotMarket(new LiveOrderBook(orderBook));
var logger = host.Services.GetService<ILogger<Program>>();

var btc = 1m;
var usdt = 50000m;

TradeSignal? previousSignal = null;

var candles = await binanceClient.SpotApi.ExchangeData.GetKlinesAsync("BTCUSDT", KlineInterval.OneMinute, limit: 6);
foreach (var c in candles.Data.SkipLast(1))
{
    tradingModel.GenerateSignal(c.ClosePrice);
}
var subscription = binanceSocket.SpotStreams.SubscribeToKlineUpdatesAsync("BTCUSDT", KlineInterval.OneMinute, data =>
{
    if (data.Data.Data.Final)
    {
        var signal = tradingModel.GenerateSignal(data.Data.Data.ClosePrice);
        if (previousSignal == TradeSignal.LONG)
        {
            var tr = spotMarket.SellMarket(btc);
            btc -= tr.FillQuantity;
            usdt += tr.AveragePrice * tr.FillQuantity;
            Console.WriteLine($"EXIT: {data.Timestamp}\t{signal}\tClosePrice: {data.Data.Data.ClosePrice}\tAverage: {tr.AveragePrice}\tBTC: {btc}\tUSDT: {usdt}");
        }

        if (previousSignal == TradeSignal.SHORT)
        {
            var amountToBuy = usdt / data.Data.Data.ClosePrice;
            Console.WriteLine($"AMOUNT TO BUY: {amountToBuy} USDT: {usdt} BTC: {btc}");
            var tr = spotMarket.BuyMarket(amountToBuy);
            btc += tr.FillQuantity;
            usdt -= tr.AveragePrice * tr.FillQuantity;
            Console.WriteLine($"EXIT: {data.Timestamp}\t{signal}\tClosePrice: {data.Data.Data.ClosePrice}\tAverage: {tr.AveragePrice}\tBTC: {btc}\tUSDT: {usdt}");
        }
        if (signal == TradeSignal.LONG)
        {
            var tr = spotMarket.BuyMarket(usdt / data.Data.Data.ClosePrice);
            btc += tr.FillQuantity;
            usdt -= tr.AveragePrice * tr.FillQuantity;
            Console.WriteLine($"ENTRY: {data.Timestamp}\t{signal}\tClosePrice: {data.Data.Data.ClosePrice}\tAverage: {tr.AveragePrice}\tBTC: {btc}\tUSDT: {usdt}");
        }
        else
        {
            var tr = spotMarket.SellMarket(btc);
            btc -= tr.FillQuantity;
            usdt += tr.AveragePrice * tr.FillQuantity;
            Console.WriteLine($"ENTRY: {data.Timestamp}\t{signal}\tClosePrice: {data.Data.Data.ClosePrice}\tAverage: {tr.AveragePrice}\tBTC: {btc}\tUSDT: {usdt}");
        }
        previousSignal = signal;
    }
});


await host.RunAsync();