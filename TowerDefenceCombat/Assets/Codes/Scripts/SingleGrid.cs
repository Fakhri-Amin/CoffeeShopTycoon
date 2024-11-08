using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleGrid : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameLevelManager.Instance.IsGameStateDay())
        {
            PlayerUnitSpawner.Instance.TryPlaceUnitOnGrid(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
