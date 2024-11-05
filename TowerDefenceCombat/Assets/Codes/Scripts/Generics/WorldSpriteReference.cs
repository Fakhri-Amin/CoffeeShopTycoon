using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldSpriteReference
{
    public MapType MapType;
    public Color GroundColor;
    public List<Sprite> LevelMapSprites = new();
}
