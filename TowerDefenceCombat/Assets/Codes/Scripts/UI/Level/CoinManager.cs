using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float CoinCollected { get; private set; }
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CoinCollectedUI coinCollectedUI;

    private void Start()
    {
        CoinCollected = GameDataManager.Instance.GoldCoin;
        UpdateCoinUI(CoinCollected);
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated += UpdateCoinUI;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated -= UpdateCoinUI;
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

