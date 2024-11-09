using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabaseSO", menuName = "Farou/Upgrade Database")]
public class UpgradeDatabaseSO : ScriptableObject
{
    public float BonusCoinRewardPercentage = 1; // The initial percentage at start
    public float BaseUpgradeBonusCoinRewardPrice = 100; // The first price of upgrade
    public float BonusCoinRewardUpgradeAmount = 5; // Every upgrade will give +5%
    public float[] UpgradeBonusCoinRewardPriceList = new float[30];

    private void OnValidate()
    {
#if UNITY_EDITOR
        // Calculate the prices for upgrading Seed Production Rate
        CalculateUpgradePrices(BaseUpgradeBonusCoinRewardPrice, UpgradeBonusCoinRewardPriceList);

        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    private void CalculateUpgradePrices(float basePrice, float[] priceList)
    {
        if (priceList == null || priceList.Length == 0) return;

        priceList[0] = basePrice; // Set the first element as the base price

        // Calculate subsequent upgrade prices based on the formula: Current Price * 2 + (Current Price * 0.2)
        for (int i = 1; i < priceList.Length; i++)
        {

            // priceList[i] = Mathf.CeilToInt(priceList[i - 1] * 1.8f); // Adjust 1.15f to tweak progression rate

            // priceList[i] = Mathf.CeilToInt(basePrice * Mathf.Pow(i + 1, 1.5f)); // This will give quadratic growth
            priceList[i] = Mathf.CeilToInt(basePrice * Mathf.Pow(i + 1, 1f)); // This will give quadratic growth

            // priceList[i] = Mathf.CeilToInt(basePrice + (i * 50) + Mathf.Pow(i, 4f)); // Adjust 1.5f for higher or lower exponential growth

            // priceList[i] = Mathf.CeilToInt(basePrice * Mathf.Log(i + 15f)); // Logarithmic growth slows down as the levels increase
        }
    }
}
