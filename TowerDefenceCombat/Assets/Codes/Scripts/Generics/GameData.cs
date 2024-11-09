using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public float GoldCoin = 200;
    public int CurrentDay = 1;
    public float BonusCoinRewardLevel = 1;
    public float ResearchLevel = 1;
    public List<PlayerUnitHero> UnlockedUnitList = new List<PlayerUnitHero>() { PlayerUnitHero.Bow };
}

