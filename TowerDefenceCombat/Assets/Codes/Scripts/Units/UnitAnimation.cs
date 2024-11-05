using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private const string IDLE_PARAMETER = "Idle";
    private const string ATTACK_PARAMETER = "Attack";
    private const string UNIT_HERO_PARAMETER = "UnitHero";

    private Animator animator;
    private Unit unit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();
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
