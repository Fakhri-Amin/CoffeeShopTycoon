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
    public event Action<float> OnGoldCoinUpdated;
    public event Action<float, float> OnSeedProductionRateChanged;
    public event Action<float, float> OnBaseHealthChanged;

    public UnitDataSO UnitDataSO;
    public LevelWaveDatabaseSO LevelWaveDatabaseSO;
    public List<PlayerUnitHero> SelectedUnitList = new List<PlayerUnitHero>(3);
    public List<PlayerUnitHero> UnlockedUnitList = new List<PlayerUnitHero>();
    public float GoldCoinCollected;
    public float GoldCoin;
    public float SeedProductionRate;
    public float BaseHealth;
    public float UpgradeSeedProductionRatePrice;
    public float UpgradeBaseHealthPrice;
    public bool IsThereNewPotato;

    public new void Awake()
    {
        base.Awake();

        var gameData = Data.Get<GameData>();

        GoldCoin = gameData.GoldCoin;
        OnGoldCoinUpdated?.Invoke(GoldCoin);

        UnlockedUnitList = gameData.UnlockedUnitList;

        SetInitialDefaultData();

        UpdateSeedProductionRate();
        UpdateBaseHealth();
    }

    private void SetInitialDefaultData()
    {
        if (SelectedUnitList.Count <= 1)
        {
            // Set default data
            AddDefaultUnlockedUnit(PlayerUnitHero.Bow);

            List<PlayerUnitHero> unitHeroes = new List<PlayerUnitHero>
            {
                PlayerUnitHero.Bow,
                PlayerUnitHero.None,
                PlayerUnitHero.None
            };
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

    public void UpdateSeedProductionRate()
    {
        var gameData = Data.Get<GameData>();
        OnSeedProductionRateChanged?.Invoke(SeedProductionRate, UpgradeSeedProductionRatePrice);

        Save();
    }

    public void UpgradeBaseHealth()
    {
        var gameData = Data.Get<GameData>();
        gameData.BaseHealthLevel++;

        UpdateBaseHealth();
    }

    public void UpdateBaseHealth()
    {
        var gameData = Data.Get<GameData>();
        OnBaseHealthChanged?.Invoke(BaseHealth, UpgradeBaseHealthPrice);

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

}
