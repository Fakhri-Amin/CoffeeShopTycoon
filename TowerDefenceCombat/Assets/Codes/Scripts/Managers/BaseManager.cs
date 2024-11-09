using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent<IAttackable>(out var singleUnit))
        {
            EventManager.Publish(Farou.Utility.EventType.OnLevelLose);
        }
    }
}
