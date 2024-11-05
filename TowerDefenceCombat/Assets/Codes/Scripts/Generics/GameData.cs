using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public int GoldCoin = 0;
    public int AzureCoin = 0;
    public int SeedProductionLevel = 1;
    public int BaseHealthLevel = 1;
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
    public List<UnitHero> SelectedUnitList = new List<UnitHero>();
    public List<CompletedLevelMap> CompletedLevelMapList = new List<CompletedLevelMap>();
    public SelectedLevelMap SelectedLevelMap;
    public bool IsThereNewPotato;
    public List<ObtainedCard> ObtainedCardList = new List<ObtainedCard>();
    public bool IsSFXMute;
    public string LastOpenedDate;
    public List<DungeonLevelData> ClearedDailyDungeonLevels = new List<DungeonLevelData>();
}

[System.Serializable]
public class CompletedLevelMap
{
    public MapType MapType;
    public List<int> CompletedLevelList = new List<int>();
}

[System.Serializable]
public class SelectedLevelMap
{
    public MapType MapType;
    public int SelectedLevelIndex;
}

[System.Serializable]
public class ObtainedCard
{
    public CardData CardData;
    public int Level;
    public int CardAmount;
}

[System.Serializable]
public class DungeonLevelData
{
    public MapType MapType;
    public int LevelIndex;
    public int EntryCount;
}
