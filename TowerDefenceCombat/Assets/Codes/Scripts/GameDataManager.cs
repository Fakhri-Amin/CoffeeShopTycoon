using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;
using System.Linq;
using UnityEngine.Assertions.Must;
using Unity.VisualScripting;

// [DefaultExecutionOrder(-99999999)]
public class GameDataManager : PersistentSingleton<GameDataManager>
{
    public event Action<int> OnDayChanged;
    public event Action<float> OnGoldCoinUpdated;
    public event Action<float, float> OnBonusCoinRewardPercentageChanged;

    public UnitDataSO UnitDataSO;
    public LevelWaveDatabaseSO LevelWaveDatabaseSO;
    public UpgradeDatabaseSO UpgradeDatabaseSO;
    public List<PlayerUnitHero> SelectedUnitList = new List<PlayerUnitHero>(3);
    public List<PlayerUnitHero> UnlockedUnitList = new List<PlayerUnitHero>();
    public int CurrentDay;
    public float GoldCoinCollected;
    public float GoldCoin;
    public float BonusCoinRewardPercentage;
    public float UpgradeBonusCoinRewardPrice;

    public new void Awake()
    {
        base.Awake();

        var gameData = Data.Get<GameData>();

        CurrentDay = gameData.CurrentDay;

        GoldCoin = gameData.GoldCoin;
        OnGoldCoinUpdated?.Invoke(GoldCoin);

        UnlockedUnitList = gameData.UnlockedUnitList;

        SetInitialDefaultData();

        UpdateBonusCoinRewardPercentage();
    }

    private void SetInitialDefaultData()
    {
        if (SelectedUnitList.Count <= 1)
        {
            // Set default data
            AddDefaultUnlockedUnit(PlayerUnitHero.Bow);
        }
    }

    public void Save()
    {
        Data.Save();
    }

    public void ModifyCoin(CurrencyType currencyType, float amount)
    {
        if (currencyType == CurrencyType.GoldCoin)
        {
            ModifyGoldCoin(amount);
        }
    }

    private void ModifyGoldCoin(float amount)
    {
        var gameData = Data.Get<GameData>();
        gameData.GoldCoin = Mathf.Clamp(gameData.GoldCoin + amount, 0, Mathf.Infinity);

        GoldCoin = gameData.GoldCoin;
        OnGoldCoinUpdated?.Invoke(GoldCoin);
        Save();
    }

    public void AddDefaultUnlockedUnit(PlayerUnitHero unitHero)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        if (gameData.UnlockedUnitList.Contains(unitHero)) return;

        gameData.UnlockedUnitList.Add(unitHero);
        UnlockedUnitList = gameData.UnlockedUnitList;

        Save();
    }

    public void AddUnlockedUnit(PlayerUnitHero unitHero)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        if (gameData.UnlockedUnitList.Contains(unitHero)) return;

        gameData.UnlockedUnitList.Add(unitHero);
        UnlockedUnitList = gameData.UnlockedUnitList;

        Save();
    }

    public bool IsUnitAlreadyUnlocked(PlayerUnitHero unitHero)
    {
        if (UnlockedUnitList.Contains(unitHero))
        {
            return true;
        }
        return false;
    }

    public void UpgradeBonusCoinRewardPercentage()
    {
        var gameData = Data.Get<GameData>();
        gameData.BonusCoinRewardLevel++;
        OnBonusCoinRewardPercentageChanged?.Invoke(BonusCoinRewardPercentage, UpgradeBonusCoinRewardPrice);

        UpdateBonusCoinRewardPercentage();
    }

    public void UpdateBonusCoinRewardPercentage()
    {
        var gameData = Data.Get<GameData>();
        BonusCoinRewardPercentage = UpgradeDatabaseSO.BonusCoinRewardPercentage + (gameData.BonusCoinRewardLevel - 1) * UpgradeDatabaseSO.BonusCoinRewardUpgradeAmount;
        UpgradeBonusCoinRewardPrice = UpgradeDatabaseSO.UpgradeBonusCoinRewardPriceList[(int)(gameData.BonusCoinRewardLevel - 1)];
        OnBonusCoinRewardPercentageChanged?.Invoke(BonusCoinRewardPercentage, UpgradeBonusCoinRewardPrice);

        Save();
    }

    public void SetCoinCollected(float amount)
    {
        GoldCoinCollected = amount;
    }

    public void ClearCoinCollected()
    {
        GoldCoinCollected = 0;
    }

    public void IncrementCurrentDay()
    {
        var gameData = Data.Get<GameData>();
        gameData.CurrentDay++;
        CurrentDay = gameData.CurrentDay;

        OnDayChanged?.Invoke(CurrentDay);

        Save();
    }

}
