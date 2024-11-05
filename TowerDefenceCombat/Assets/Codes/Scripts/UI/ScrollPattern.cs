using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPattern : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x,_y;

    void Update()
    {
        // Source Code https://www.youtube.com/watch?v=-6H-uYh80vc&ab_channel=Tarodev
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
    }
}
