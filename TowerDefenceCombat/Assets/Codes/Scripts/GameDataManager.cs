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
    public event Action<float> OnAzureCoinUpdated;
    public event Action<List<UnitHero>> OnSelectedUnitListChanged;
    public event Action<float, float> OnSeedProductionRateChanged;
    public event Action<float, float> OnBaseHealthChanged;

    public UnitDataSO UnitDataSO;
    public LevelWaveDatabaseSO LevelWaveDatabaseSO;
    public List<UnitHero> SelectedUnitList = new List<UnitHero>(3);
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
    public List<int> CompletedLevelList = new List<int>();
    public List<CompletedLevelMap> CompletedLevelMapList = new List<CompletedLevelMap>();
    public SelectedLevelMap SelectedLevelMap = new SelectedLevelMap();
    public List<ObtainedCard> ObtainedCardList = new List<ObtainedCard>();
    public float GoldCoinCollected;
    public float AzureCoinCollected;
    public float GoldCoin;
    public float AzureCoin;
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

        AzureCoin = gameData.AzureCoin;
        OnAzureCoinUpdated?.Invoke(AzureCoin);

        SelectedUnitList = gameData.SelectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);

        UnlockedUnitList = gameData.UnlockedUnitList;
        CompletedLevelMapList = gameData.CompletedLevelMapList;

        ObtainedCardList = gameData.ObtainedCardList;

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
            SetSelectedUnit(unitHeroes);
            AddNewCompletedLevel(MapType.Beach);
            AddNewCompletedLevel(MapType.Forest);
        }
    }

    public void Save()
    {
        Data.Save();
    }

    public void SelectLevel(MapType mapType, int levelNumber)
    {
        SelectedLevelMap = new SelectedLevelMap { MapType = mapType, SelectedLevelIndex = levelNumber };
        Data.Get<GameData>().SelectedLevelMap.MapType = mapType;
        Data.Get<GameData>().SelectedLevelMap.SelectedLevelIndex = levelNumber;
        Save();
    }

    public void ModifyCoin(CurrencyType currencyType, float amount)
    {
        if (currencyType == CurrencyType.GoldCoin)
        {
            ModifyGoldCoin(amount);
        }
        else if (currencyType == CurrencyType.AzureCoin)
        {
            ModifyAzureCoin(amount);
        }
    }

    private void ModifyGoldCoin(float amount)
    {
        Data.Get<GameData>().GoldCoin += (int)amount;
        GoldCoin = Data.Get<GameData>().GoldCoin;
        OnGoldCoinUpdated?.Invoke(GoldCoin);
        Save();
    }

    private void ModifyAzureCoin(float amount)
    {
        Data.Get<GameData>().AzureCoin += (int)amount;
        AzureCoin = Data.Get<GameData>().AzureCoin;
        OnAzureCoinUpdated?.Invoke(AzureCoin);
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
        SetNewPotatoStatus(true);

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

    public void SetSelectedUnit(List<UnitHero> selectedUnitList)
    {
        SelectedUnitList = selectedUnitList;
        Data.Get<GameData>().SelectedUnitList = selectedUnitList;
        OnSelectedUnitListChanged?.Invoke(SelectedUnitList);
        Save();
    }

    public void SetSelectedLevel(MapType mapType, int levelIndex)
    {
        SelectedLevelMap = new SelectedLevelMap { MapType = mapType, SelectedLevelIndex = levelIndex };
        Data.Get<GameData>().SelectedLevelMap = SelectedLevelMap;
        Save();
    }

    public void AddNewCompletedLevel(MapType mapType)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        // Find the map or create a new one if it doesn't exist
        CompletedLevelMap completedLevelMap = gameData.CompletedLevelMapList.Find(i => i.MapType == mapType);
        if (completedLevelMap == null)
        {
            completedLevelMap = new CompletedLevelMap
            {
                MapType = mapType,
                CompletedLevelList = new List<int>(),
            };

            // Add the new map to the list
            gameData.CompletedLevelMapList.Add(completedLevelMap);
        }

        // Save the updated data
        Save();
    }

    public void AddNewCompletedLevel(MapType mapType, int levelIndex)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        // Find the map or create a new one if it doesn't exist
        CompletedLevelMap completedLevelMap = gameData.CompletedLevelMapList.Find(i => i.MapType == mapType);
        if (completedLevelMap == null)
        {
            completedLevelMap = new CompletedLevelMap
            {
                MapType = mapType,
                CompletedLevelList = new List<int>(),
            };

            // Add the new map to the list
            gameData.CompletedLevelMapList.Add(completedLevelMap);

        }

        // Check if the level is already completed, if so, return early
        if (completedLevelMap.CompletedLevelList.Contains(levelIndex)) return;

        // Add the new completed level
        completedLevelMap.CompletedLevelList.Add(levelIndex);


        // Save the updated data
        Save();
    }

    public bool IsLevelAlreadyCompleted(MapType mapType, int levelIndex)
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        // Find the map or create a new one if it doesn't exist
        CompletedLevelMap completedLevelMap = gameData.CompletedLevelMapList.Find(i => i.MapType == mapType);
        if (completedLevelMap == null)
        {
            return false;
        }
        else
        {
            if (completedLevelMap.CompletedLevelList.Contains(levelIndex))
            {
                return true;
            }
        }

        return false;
    }

    public DungeonLevelData GetDungeonLevelData(int levelIndex)
    {
        return Data.Get<GameData>().ClearedDailyDungeonLevels.Find(i => i.LevelIndex == levelIndex);
    }

    public void ClearClearedDailyDungeonLevel()
    {
        // Retrieve the GameData instance
        var gameData = Data.Get<GameData>();

        gameData.ClearedDailyDungeonLevels.Clear();

        // Save the updated data
        Save();
    }

    public List<int> GetCompletedLevelsForMap(MapType mapType)
    {
        return CompletedLevelMapList.Find(i => i.MapType == mapType)?.CompletedLevelList;
    }

    public bool IsPreviousMapCompleted(MapType previousMap)
    {
        // Find the completed levels for the previous map
        var completedMap = CompletedLevelMapList.Find(i => i.MapType == previousMap);
        if (completedMap == null || completedMap.CompletedLevelList.Count == 0)
        {
            // If the map is not found or has no completed levels, it's not completed
            return false;
        }

        // Find the total number of levels in the previous map
        var mapReference = LevelWaveDatabaseSO.MapLevelReferences.Find(i => i.MapType == previousMap);
        if (mapReference == null || mapReference.Levels == null || mapReference.Levels.Count == 0)
        {
            // If map reference or levels are invalid, return false
            return false;
        }

        // Get the max completed level and check if it's equal to the total number of levels minus 1
        int maxCompletedLevel = completedMap.CompletedLevelList.Max();
        bool hasCompletedAllLevels = maxCompletedLevel == mapReference.Levels.Count - 1;

        return hasCompletedAllLevels;
    }


    public void UpdateSeedProductionRate()
    {
        var gameData = Data.Get<GameData>();
        // SeedProductionRate = BaseBuildingSO.SeedProductionRate + (gameData.SeedProductionLevel - 1) * BaseBuildingSO.SeedProductionRateUpgradeAmount;
        // UpgradeSeedProductionRatePrice = BaseBuildingSO.UpgradeSeedProductionRatePriceList[gameData.SeedProductionLevel - 1];
        OnSeedProductionRateChanged?.Invoke(SeedProductionRate, UpgradeSeedProductionRatePrice);

        Save();
    }

    public void UpgradeSeedProductionRate()
    {
        var gameData = Data.Get<GameData>();
        gameData.SeedProductionLevel++;

        UpdateSeedProductionRate();
    }

    public void UpdateBaseHealth()
    {
        var gameData = Data.Get<GameData>();
        // BaseHealth = BaseBuildingSO.BaseHealth + (gameData.BaseHealthLevel - 1) * BaseBuildingSO.BaseHealthUpgradeAmount;
        // UpgradeBaseHealthPrice = BaseBuildingSO.UpgradeBaseHealthPriceList[gameData.BaseHealthLevel - 1];
        OnBaseHealthChanged?.Invoke(BaseHealth, UpgradeBaseHealthPrice);

        Save();
    }

    public void UpgradeBaseHealth()
    {
        var gameData = Data.Get<GameData>();
        gameData.BaseHealthLevel++;

        UpdateBaseHealth();
    }

    public void SetCoinCollected(CurrencyType currencyType, float amount)
    {
        if (currencyType == CurrencyType.GoldCoin)
        {
            GoldCoinCollected = amount;
        }
        else if (currencyType == CurrencyType.AzureCoin)
        {
            AzureCoinCollected = amount;
        }
    }

    public void SetCoinCollectedDouble(CurrencyType currencyType)
    {
        if (currencyType == CurrencyType.GoldCoin)
        {
            GoldCoinCollected *= 2;
        }
        else if (currencyType == CurrencyType.AzureCoin)
        {
            AzureCoinCollected *= 2;
        }
    }

    public void ClearCoinCollected()
    {
        GoldCoinCollected = 0;
        AzureCoinCollected = 0;
    }

    public void SetNewPotatoStatus(bool isActive)
    {
        var gameData = Data.Get<GameData>();
        gameData.IsThereNewPotato = isActive;
        IsThereNewPotato = isActive;

        Save();
    }

    public void AddNewObtainedCard(CardData cardData)
    {
        var gameData = Data.Get<GameData>();

        var cardDataInDatabase = gameData.ObtainedCardList.Find(i => i.CardData.Name == cardData.Name);

        if (cardDataInDatabase == null)
        {
            ObtainedCard obtainedCard = new ObtainedCard()
            {
                CardData = cardData,
                Level = 1,
                CardAmount = 1
            };
            gameData.ObtainedCardList.Add(obtainedCard);

            Save();

            return;
        }

        cardDataInDatabase.CardAmount++;
        ObtainedCardList = gameData.ObtainedCardList;

        Save();
    }

    public float GetTotalAttackDamagePercentage()
    {
        var gameData = Data.Get<GameData>();

        float totalAttackDamagePercentage = 0;
        foreach (var item in gameData.ObtainedCardList)
        {
            if (item.CardData.CardType == CardType.AttackDamage)
            {
                totalAttackDamagePercentage += item.CardData.EffectAmount;
            }
        }

        return totalAttackDamagePercentage;
    }

    public float GetTotalUnitHealthPercentage()
    {
        var gameData = Data.Get<GameData>();

        float totalUnitHealthPercentage = 0;
        foreach (var item in gameData.ObtainedCardList)
        {
            if (item.CardData.CardType == CardType.UnitHealth)
            {
                totalUnitHealthPercentage += item.CardData.EffectAmount;
            }
        }

        return totalUnitHealthPercentage;
    }

    public float GetTotalBaseHealthPercentage()
    {
        var gameData = Data.Get<GameData>();

        float totalBaseHealthPercentage = 0;
        foreach (var item in gameData.ObtainedCardList)
        {
            if (item.CardData.CardType == CardType.BaseHealth)
            {
                totalBaseHealthPercentage += item.CardData.EffectAmount;
            }
        }

        return totalBaseHealthPercentage;
    }

    public bool GetSFXSettingState()
    {
        return Data.Get<GameData>().IsSFXMute;
    }

    public void MuteSFX()
    {
        Data.Get<GameData>().IsSFXMute = true;
        Save();
    }

    public void UnMuteSFX()
    {
        Data.Get<GameData>().IsSFXMute = false;
        Save();
    }

}
