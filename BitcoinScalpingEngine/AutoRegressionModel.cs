using Microsoft.Extensions.Logging;

namespace BitcoinScalpingEngine;

public class AutoRegressionModel : ITradingModel
{
    private ILogger<AutoRegressionModel> _logger;

    private decimal[] weights;
    private decimal constant;
    private Queue<decimal> closes;
    
    public AutoRegressionModel(ILogger<AutoRegressionModel> logger)
    {
        _logger = logger;
        closes = new();
        weights = new[]
        {
            1.015592m,
            -0.039831m,
            0.013372m,
            0.005618m,
            0.005247m
        };
        constant = 0.110219m;
    }

    public TradeSignal GenerateSignal(decimal value)
    {
        _logger.LogInformation($"inf: {value}");
        
        if (closes.Count == 5)
            closes.Dequeue();
        closes.Enqueue(value);
        
        var pred = Prediction();
        
        if (pred < value)
            return TradeSignal.LONG;
        
        return TradeSignal.SHORT;
    }

    private decimal Prediction()
    {
        var c = closes.ToArray();
        if (c.Length < 5)
            return 0m;
        _logger.LogInformation($"queue => 1: {c[0]} 2: {c[1]} 3: {c[2]} 4: {c[3]} 5: {c[4]}");
        return constant +
            weights[0] * c[0] +
            weights[1] * c[1] +
            weights[2] * c[2] +
            weights[3] * c[3] +
            weights[4] * c[4];
    }
}
