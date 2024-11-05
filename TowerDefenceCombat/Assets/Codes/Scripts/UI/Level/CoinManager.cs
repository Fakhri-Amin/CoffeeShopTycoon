using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float CoinCollected { get; private set; }
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CoinCollectedUI coinCollectedUI;

    public void SetMapCurrency(MapType mapType)
    {
        Sprite sprite;

        if (mapType == MapType.Dungeon)
        {
            sprite = gameAssetSO.AzureCoinSprite;
        }
        else
        {
            sprite = gameAssetSO.GoldCoinSprite;
        }

        coinCollectedUI.InitializeCoinImage(sprite);
    }

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

