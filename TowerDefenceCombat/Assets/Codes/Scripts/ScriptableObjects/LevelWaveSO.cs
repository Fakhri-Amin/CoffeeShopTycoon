using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelWaveSO", menuName = "Farou/Level Wave")]
public class LevelWaveSO : ScriptableObject
{
    public float DelayAtStart;
    public float DelayBetweenWaves;
    [TableList(ShowIndexLabels = true)] public List<WaveData> WaveDatas = new();
    public float TotalCoinReward;
    public UnitDataSO UnitDataSO;

    private void OnValidate()
    {
#if UNITY_EDITOR

        float totalCoinReward = 0;

        // Iterate through each wave
        for (int waveIndex = 0; waveIndex < WaveDatas.Count; waveIndex++)
        {
            var currentWave = WaveDatas[waveIndex];
            var waveUnitDatas = currentWave.WaveHeroDatas;

            // Iterate through each hero data in the wave
            for (int heroIndex = 0; heroIndex < waveUnitDatas.Count; heroIndex++)
            {
                var waveUnitData = waveUnitDatas[heroIndex];
                var unitType = waveUnitData.UnitType;

                // Find the UnitData for the current UnitType
                UnitData unitData = UnitDataSO.UnitStatDataList.Find(i => i.UnitHero == unitType);

                if (unitData != null)
                {
                    // Assuming there is a CoinReward field or property in UnitData
                    float coinRewardPerUnit = unitData.CoinReward;
                    float totalRewardForCurrentHero = coinRewardPerUnit * waveUnitData.Count;

                    // Add this to the total coin reward
                    totalCoinReward += totalRewardForCurrentHero;
                }
            }

        }

        TotalCoinReward = totalCoinReward;
        UnityEditor.EditorUtility.SetDirty(this);

#endif
    }
}
