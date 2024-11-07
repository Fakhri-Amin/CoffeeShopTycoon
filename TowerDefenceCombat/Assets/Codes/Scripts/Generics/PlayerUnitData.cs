using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class PlayerUnitData : UnitData
{
    [TabGroup("General")]
    [EnumPaging]
    public PlayerUnitHero UnitHero;

    [TabGroup("Stat")]
    public int CoinCost;
    [TabGroup("Stat")]
    [HideIf("UnitRangeType", UnitRangeType.Melee)] public float ProjectileSpeed;
    [TabGroup("Stat")]
    [ShowIf("UnitAttackType", UnitAttackType.Area)] public float AreaOfEffectRadius;
}
