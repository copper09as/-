using UnityEngine;

public static class PriceCalculator
{
    // �۸������ԣ����ڳ�ʼ�۸�
    public static int CalculateNewPrice(MarketCondition condition, int basePrice)
    {
        return condition switch
        {
            MarketCondition.Hot => Mathf.RoundToInt(basePrice * Random.Range(1.9f, 2.1f)),   // ���ȣ�190%~210%
            MarketCondition.Cold => Mathf.RoundToInt(basePrice * Random.Range(0.45f, 0.55f)), // ���䣺45%~55%
            _ => Mathf.RoundToInt(basePrice * Random.Range(0.9f, 1.1f))                      // �ȶ���90%~110%
        };
    }
}