using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    [TableList] public List<WaveHeroData> WaveHeroDatas = new List<WaveHeroData>();
}
