using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float CoinCollected { get; private set; }
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CoinCollectedUI coinCollectedUI;

    public void Reset()
    {
        CoinCollected = 0;
        UpdateCoinUI(CoinCollected);
    }

    public void SetFinalCoinCollected(float amount)
    {
        CoinCollected = amount;
    }

    public void AddCoins(float amount)
    {
        CoinCollected += amount;
        UpdateCoinUI(CoinCollected);
    }

    public void UpdateCoinUI(float amount)
    {
        coinCollectedUI.UpdateCoinCollectedUI(amount);
    }
}

