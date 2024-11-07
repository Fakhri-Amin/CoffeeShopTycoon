using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitAnimation : UnitAnimation
{
    private Animator animator;
    private PlayerUnit unit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<PlayerUnit>();
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
