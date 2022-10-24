using BitcoinScalpingEngine.Trading;
using NuGet.Frameworks;
using NUnit.Framework;

namespace ScalpingEngineTests;

class MockOrderBook : IOrderBook
{
    public List<OrderBookLevel> Bids()
    {
        throw new NotImplementedException();
    }

    public List<OrderBookLevel> Asks()
    {
        return new List<OrderBookLevel>()
        {
            new OrderBookLevel(1.0m, 1),
            new OrderBookLevel(1.1m, 2),
            new OrderBookLevel(1.2m, 3),
            new OrderBookLevel(1.3m, 2)
        };
    }
}

[TestFixture]
public class TradingTests
{
    [Test]
    public void TestSportMarket()
    {
        var sm = new SpotMarket(new MockOrderBook());
        var tr = sm.BuyMarket(1);
        Assert.AreEqual(1, tr.AveragePrice);
        Assert.AreEqual(1, tr.FillQuantity);
    }

    [Test]
    public void Test_Buy_Partial_2nd_Level()
    {
        var sm = new SpotMarket(new MockOrderBook());
        var tr = sm.BuyMarket(2m);
        Assert.AreEqual(1.05m, tr.AveragePrice);
        Assert.AreEqual(2m, tr.FillQuantity);
    }

    [Test]
    public void TestBuy_Partial()
    {
        var sm = new SpotMarket(new MockOrderBook());
        var tr = sm.BuyMarket(1.7m);
        Assert.AreEqual(1.04118m, Math.Round(tr.AveragePrice, 5));
        Assert.AreEqual(1.7, tr.FillQuantity);
    }
    
}