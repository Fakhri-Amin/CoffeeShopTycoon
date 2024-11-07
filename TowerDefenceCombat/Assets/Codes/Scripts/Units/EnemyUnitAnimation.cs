using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitAnimation : UnitAnimation
{
    private Animator animator;
    private EnemyUnit unit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<EnemyUnit>();
    }

    public void PlayAttackAnimation()
    {
        animator.SetInteger(UNIT_HERO_PARAMETER, (int)unit.UnitHero);
        animator.SetTrigger(ATTACK_PARAMETER);
    }

    public void PlayIdleAnimation()
    {
        animator.SetInteger(UNIT_HERO_PARAMETER, (int)unit.UnitHero);
    }
}
