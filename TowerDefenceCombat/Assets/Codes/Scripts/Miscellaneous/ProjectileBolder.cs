using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBolder : MonoBehaviour, IAttackable
{
    public GameObject GameObject => gameObject;

    public UnitType UnitType => UnitType.Enemy;

    public void Damage(float damageAmount)
    {

    }
}
