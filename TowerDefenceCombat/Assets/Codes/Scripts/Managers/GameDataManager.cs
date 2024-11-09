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
    public event Action<float, float> OnResearchLevelChanged;

    public UnitDataSO UnitDataSO;
    public LevelWaveDatabaseSO LevelWaveDatabaseSO;
    public UpgradeDatabaseSO UpgradeDatabaseSO;
    public List<PlayerUnitHero> UnlockedUnitList = new List<PlayerUnitHero>();
    public int CurrentDay;
    public float ResearchLevel;
    public float GoldCoinCollected;
    public float GoldCoin;
    public float BonusCoinRewardPercentage;
    public float UpgradeBonusCoinRewardPrice;
    public float UpgradeResearchPrice;

    public new void Awake()
    {
        base.Awake();

        var gameData = Data.Get<GameData>();

        CurrentDay = gameData.CurrentDay;
        OnDayChanged?.Invoke(CurrentDay);

        GoldCoin = gameData.GoldCoin;
        OnGoldCoinUpdated?.Invoke(GoldCoin);

        UnlockedUnitList = gameData.UnlockedUnitList;

        SetInitialDefaultData();

        UpdateBonusCoinRewardPercentage();
        UpdateResearchLevel();
    }

    private void SetInitialDefaultData()
    {
        if (CurrentDay <= 1)
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

    public void AddRandomUnit()
    {
        // Ensure there are units available to choose from
        if (UnitDataSO.PlayerUnitStatDataList.Count == 0)
        {
            Debug.LogWarning("No units available to unlock.");
            return;
        }

        // Filter the list to only include units that haven't been unlocked
        var unobtainedUnits = UnitDataSO.PlayerUnitStatDataList
            .Where(unitData => !IsUnitAlreadyUnlocked(unitData.UnitHero))
            .ToList();

        // Check if there are any unobtained units left
        if (unobtainedUnits.Count == 0)
        {
            Debug.LogWarning("All units have already been unlocked.");
            return;
        }

        // Select a random unit from the unobtained units
        var randomUnitHero = unobtainedUnits[
            UnityEngine.Random.Range(0, unobtainedUnits.Count)
        ].UnitHero;

        // Unlock the selected unit
        AddUnlockedUnit(randomUnitHero);
    }


    public bool IsUnitAlreadyUnlocked(PlayerUnitHero unitHero)
    {
        if (UnlockedUnitList.Contains(unitHero))
        {
            return true;
        }
        return false;
    }

    public void UpgradeResearchLevel()
    {
        var gameData = Data.Get<GameData>();
        gameData.ResearchLevel++;

        UpdateResearchLevel();
        AddRandomUnit();
    }

    private void UpdateResearchLevel()
    {
        var gameData = Data.Get<GameData>();

        ResearchLevel = gameData.ResearchLevel;
        UpgradeResearchPrice = UpgradeDatabaseSO.UpgradeResearchPriceList[(int)(gameData.ResearchLevel - 1)];
        OnResearchLevelChanged?.Invoke(ResearchLevel, UpgradeResearchPrice);

        Save();
    }

    public void UpgradeBonusCoinRewardPercentage()
    {
        var gameData = Data.Get<GameData>();
        gameData.BonusCoinRewardLevel++;

        UpdateBonusCoinRewardPercentage();
    }

    private void UpdateBonusCoinRewardPercentage()
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

    public void ClearDatabase()
    {
        var gameData = Data.Get<GameData>();

        gameData.GoldCoin = 200;
        GoldCoin = gameData.GoldCoin;

        gameData.CurrentDay = 1;
        CurrentDay = gameData.CurrentDay;

        gameData.BonusCoinRewardLevel = 1;

        gameData.ResearchLevel = 1;
        ResearchLevel = gameData.ResearchLevel;

        gameData.UnlockedUnitList = new List<PlayerUnitHero>() { PlayerUnitHero.Bow };
        UnlockedUnitList = gameData.UnlockedUnitList;

        OnGoldCoinUpdated?.Invoke(GoldCoin);
        OnDayChanged?.Invoke(CurrentDay);
        UpdateBonusCoinRewardPercentage();
        UpdateResearchLevel();

        Save();
    }
}
