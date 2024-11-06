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
    public List<UnitHero> SelectedUnitList = new List<UnitHero>(3);
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
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

        SetInitialDefaultData();

        UpdateSeedProductionRate();
        UpdateBaseHealth();
    }

    private void SetInitialDefaultData()
    {
        if (SelectedUnitList.Count <= 1)
        {
            // Set default data
            AddDefaultUnlockedUnit(UnitHero.Sword);

            List<UnitHero> unitHeroes = new List<UnitHero>
            {
                UnitHero.Sword,
                UnitHero.None,
                UnitHero.None
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
        Data.Get<GameData>().GoldCoin += (int)amount;
        GoldCoin = Data.Get<GameData>().GoldCoin;
        OnGoldCoinUpdated?.Invoke(GoldCoin);
        Save();
    }

    public void AddDefaultUnlockedUnit(UnitHero unitHero)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        if (gameData.UnlockedUnitList.Contains(unitHero)) return;

        gameData.UnlockedUnitList.Add(unitHero);
        UnlockedUnitList = gameData.UnlockedUnitList;

        Save();
    }

    public void AddUnlockedUnit(UnitHero unitHero)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        if (gameData.UnlockedUnitList.Contains(unitHero)) return;

        gameData.UnlockedUnitList.Add(unitHero);
        UnlockedUnitList = gameData.UnlockedUnitList;

        Save();
    }

    public bool IsUnitAlreadyUnlocked(UnitHero unitHero)
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
