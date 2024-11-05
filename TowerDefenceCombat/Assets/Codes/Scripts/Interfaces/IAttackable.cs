using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public GameObject GameObject { get; }
    public UnitType UnitType { get; }
    public void Damage(float damageAmount);
}
