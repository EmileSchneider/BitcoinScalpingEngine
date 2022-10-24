using Binance.Net.Interfaces.Clients;
using BitcoinScalpingEngine.Trading;

namespace BitcoinScalpingEngine;

public class SlippageCalculator
{
    public decimal AveragePriceLong(decimal quantity, OrderBookLevel[] Asks)
    {
        return averagePrice(quantity, Asks);
    }

    private static decimal averagePrice(decimal quantity, OrderBookLevel[] Asks)
    {
        var rest = quantity;
        int i = 0;
        var average = 0m;
        while (rest > 0)
        {
            average += Asks[i].Price * (Asks[i].Quantity > rest ? rest : Asks[i].Quantity);
            rest -= Asks[i].Quantity;
            i++;
        }

        return average / quantity;
    }

    public decimal AveragePriceShort(decimal quantity, OrderBookLevel[] Bids)
    {
        return averagePrice(quantity, Bids);
    }
}


