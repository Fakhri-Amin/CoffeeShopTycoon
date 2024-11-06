using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class UnitData
{
    [Title("@UnitHero", titleAlignment: TitleAlignments.Centered)]
    [TabGroup("General")]
    [EnumPaging]
    public UnitHero UnitHero;
    [TabGroup("General")]
    [EnumPaging]
    public UnitRangeType UnitRangeType;
    [TabGroup("General")]
    [EnumPaging]
    public UnitAttackType UnitAttackType;
    [TabGroup("General")]
    public Unit Prefab;
    [TabGroup("General")]
    public Sprite Sprite;
    [TabGroup("General")]
    public string Name;

    [Title("@UnitHero", titleAlignment: TitleAlignments.Centered)]
    [TabGroup("Stat")]
    public int SeedCost;
    [TabGroup("Stat")]
    public float Health;
    [TabGroup("Stat")]
    public float DamageAmount;
    [TabGroup("Stat")]
    public UnitMoveSpeedType MoveSpeedType;
    [TabGroup("Stat")]
    public UnitAttackSpeedType AttackSpeedType;
    [TabGroup("Stat")]
    public float AttackRadius;
    [TabGroup("Stat")]
    [HideIf("UnitRangeType", UnitRangeType.Melee)] public float ProjectileSpeed;
    [TabGroup("Stat")]
    [ShowIf("UnitAttackType", UnitAttackType.Area)] public float AreaOfEffectRadius;
    [TabGroup("Stat")]
    public float CoinReward;
}
