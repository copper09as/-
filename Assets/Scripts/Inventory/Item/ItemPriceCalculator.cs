using UnityEngine;

public static class PriceCalculator
{
    // 价格计算策略（基于初始价格）
    public static int CalculateNewPrice(MarketCondition condition, int basePrice)
    {
        return condition switch
        {
            MarketCondition.Hot => Mathf.RoundToInt(basePrice * Random.Range(1.9f, 2.1f)),   // 大热：190%~210%
            MarketCondition.Cold => Mathf.RoundToInt(basePrice * Random.Range(0.45f, 0.55f)), // 大冷：45%~55%
            _ => Mathf.RoundToInt(basePrice * Random.Range(0.9f, 1.1f))                      // 稳定：90%~110%
        };
    }
}