using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectStateMonitor : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.LogWarning($"GameObject {gameObject.name} has been set to active!", this);
        Debug.LogWarning("Stack Trace: " + System.Environment.StackTrace);
    }

    private void OnDisable()
    {
        Debug.LogWarning($"GameObject {gameObject.name} has been set to inactive!", this);
        Debug.LogWarning("Stack Trace: " + System.Environment.StackTrace);
    }
}
