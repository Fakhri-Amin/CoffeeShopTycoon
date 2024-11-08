using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public float GoldCoin = 0;
    public int CurrentDay;
    public int BaseHealthLevel = 1;
    public List<PlayerUnitHero> UnlockedUnitList = new List<PlayerUnitHero>();
}

