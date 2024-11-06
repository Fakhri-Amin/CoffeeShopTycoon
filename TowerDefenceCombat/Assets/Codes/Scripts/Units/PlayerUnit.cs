using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();

        // Handle movement and attack
        if (canAttack && targetUnit != null)
        {
            unitAnimation.PlayAttackAnimation();
            attackCooldown = attackSpeed;
            canAttack = false;
        }

        DetectEnemiesAndHandleAttack(); // Ensure enemy detection and handling is checked every frame
    }

    private void DetectEnemiesAndHandleAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, unitData.AttackRadius, TargetMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent<IAttackable>(out IAttackable unit))
            {
                targetUnit = unit;
                return;
            }
        }

        // Reset state if no valid targets are found
        targetUnit = null;
    }
}
