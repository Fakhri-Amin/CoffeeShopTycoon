using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatSO", menuName = "Farou/Unit Stat")]
public class UnitDataSO : ScriptableObject
{
    [TableList] public List<UnitMoveSpeedData> MoveSpeedDataList;
    [TableList] public List<UnitAttackSpeedData> AttackSpeedDataList;
    [TableList(ShowIndexLabels = true)] public List<UnitData> UnitStatDataList;
}
