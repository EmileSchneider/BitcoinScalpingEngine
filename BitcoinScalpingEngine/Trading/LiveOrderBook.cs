using Binance.Net.Interfaces;
using Binance.Net.SymbolOrderBooks;
using CryptoExchange.Net.CommonObjects;

namespace BitcoinScalpingEngine.Trading;

public class LiveOrderBook : IOrderBook
{
    private BinanceSpotSymbolOrderBook binanceOrderBook;
    public LiveOrderBook(BinanceSpotSymbolOrderBook binanceSpotSymbolOrderBook)
    {
        this.binanceOrderBook = binanceSpotSymbolOrderBook;
    }

    public List<OrderBookLevel> Bids()
    {
        return binanceOrderBook.Asks.Select(b => new OrderBookLevel(b.Price, b.Quantity)).ToList();
    }

    public List<OrderBookLevel> Asks()
    {
        return binanceOrderBook.Bids.Select(b => new OrderBookLevel(b.Price, b.Quantity)).ToList();
    }
}