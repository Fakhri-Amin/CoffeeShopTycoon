using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float CoinCollected { get; private set; }
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CoinCollectedUI coinCollectedUI;

    public void AddCoins(float amount)
    {
        CoinCollected += amount;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        coinCollectedUI.UpdateCoinCollectedUI(CoinCollected);
    }
}

