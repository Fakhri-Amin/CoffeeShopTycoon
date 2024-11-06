using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelWaveDatabaseSO", menuName = "Farou/Level Wave Database")]
public class LevelWaveDatabaseSO : ScriptableObject
{
    public List<LevelWaveSO> DayWaves = new List<LevelWaveSO>();
}
