using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyUnitAttackSystem : MonoBehaviour
{
    [SerializeField] private UnitRangeType unitRangeType;
    [SerializeField] protected Transform shootingTransform;
    [HideIf("unitRangeType", UnitRangeType.Melee)]
    [SerializeField] protected ProjectileType projectileType;
    [ShowIf("unitRangeType", UnitRangeType.Melee)]

    private EnemyUnit unit;

    private void Awake()
    {
        unit = GetComponent<EnemyUnit>();
    }

    public virtual void HandleAttack()
    {
        if (unit.TargetUnit == null) return; // No target, no attack

        if (unitRangeType == UnitRangeType.Melee)
        {
            HandleMeleeAttack();
        }
    }

    private void HandleMeleeAttack()
    {
        if (unit.UnitData.UnitAttackType == UnitAttackType.Single)
        {
            PerformSingleTargetMeleeAttack();
        }
    }

    private void PerformSingleTargetMeleeAttack()
    {
        unit.TargetUnit.Damage(unit.UnitData.DamageAmount);
    }
}
