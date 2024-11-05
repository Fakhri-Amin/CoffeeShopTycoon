using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelWaveDatabaseSO", menuName = "Farou/Level Wave Database")]
public class LevelWaveDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public class MapLevelReference
    {
        public MapType MapType;
        public List<LevelWaveSO> Levels = new();
    }
    public List<MapLevelReference> MapLevelReferences = new List<MapLevelReference>();
}
