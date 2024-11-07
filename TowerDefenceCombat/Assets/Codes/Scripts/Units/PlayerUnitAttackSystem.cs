using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerUnitAttackSystem : MonoBehaviour
{
    [SerializeField] private UnitRangeType unitRangeType;
    [SerializeField] protected Transform shootingTransform;
    [HideIf("unitRangeType", UnitRangeType.Melee)]
    [SerializeField] protected ProjectileType projectileType;
    [ShowIf("unitRangeType", UnitRangeType.Melee)]

    private PlayerUnit unit;

    private void Awake()
    {
        unit = GetComponent<PlayerUnit>();
    }

    public virtual void HandleAttack()
    {
        if (unit.TargetUnit == null) return; // No target, no attack

        if (unitRangeType == UnitRangeType.Melee)
        {
            HandleMeleeAttack();
        }
        else
        {
            HandleRangedAttack();
        }
    }

    private void HandleMeleeAttack()
    {
        if (unit.UnitData.UnitAttackType == UnitAttackType.Single)
        {
            PerformSingleTargetMeleeAttack();
        }
        else if (unit.UnitData.UnitAttackType == UnitAttackType.Area)
        {
            PerformAreaOfEffectAttack();
        }

    }

    private void PerformSingleTargetMeleeAttack()
    {
        unit.TargetUnit.Damage(unit.UnitData.DamageAmount);
    }

    private void PerformAreaOfEffectAttack()
    {
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(
            shootingTransform.position,
            unit.UnitData.AreaOfEffectRadius,
            unit.TargetMask
        );

        foreach (var target in targetsInRadius)
        {
            if (target.TryGetComponent(out IAttackable targetUnit))
            {
                targetUnit.Damage(unit.UnitData.DamageAmount + unit.AttackDamageBoost);
            }
        }
    }

    private void HandleRangedAttack()
    {
        var newProjectile = ProjectileObjectPool.Instance.GetPooledObject(projectileType);

        if (newProjectile != null)
        {
            newProjectile.transform.position = shootingTransform.position;
            newProjectile.Initialize(unit, unit.TargetUnit, projectileType);
        }
    }
}
