using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : BaseData
{
    public override string Name => "Game Data";
    public override string Key => "GameData";

    public int GoldCoin = 0;
    public int BaseHealthLevel = 1;
    public List<UnitHero> UnlockedUnitList = new List<UnitHero>();
}

