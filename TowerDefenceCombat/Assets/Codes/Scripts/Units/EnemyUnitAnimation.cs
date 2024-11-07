using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitAnimation : UnitAnimation
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation(int paramterIndex)
    {
        animator.SetInteger(UNIT_HERO_PARAMETER, paramterIndex);
        animator.SetTrigger(ATTACK_PARAMETER);
    }

    public void PlayIdleAnimation(int paramterIndex)
    {
        animator.SetInteger(UNIT_HERO_PARAMETER, paramterIndex);
    }
}
