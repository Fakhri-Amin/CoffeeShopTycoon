using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EnemyUnitData : UnitData
{
    [TabGroup("General")]
    [EnumPaging]
    public EnemyUnitHero UnitHero;

    [TabGroup("Stat")]
    public UnitMoveSpeedType MoveSpeedType;
    [TabGroup("Stat")]
    public float CoinReward;
}
