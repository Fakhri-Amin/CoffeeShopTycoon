using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IAttackable
{
    public event Action OnDead;

    [SerializeField] protected UnitType unitType;
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected Transform visual;
    [SerializeField] protected SpriteRenderer bodySprite;
    [SerializeField] protected SpriteRenderer weaponSprite;

    protected bool isIdle;
    protected float attackDamageBoost;
    protected float moveSpeed;
    protected float attackSpeed;
    protected float attackCooldown;
    protected IAttackable targetUnit;
    protected HealthSystem healthSystem;
    protected UnitParticle unitParticle;
    protected UnitAudio unitAudio;
    protected Vector3 moveDirection;
    protected Vector3 basePosition;

    protected bool canAttack = true;
    public float AttackDamageBoost => attackDamageBoost;
    public IAttackable TargetUnit => targetUnit;

    public UnitType UnitType => unitType;
    public LayerMask TargetMask => targetMask;

    public GameObject GameObject => gameObject;


    public virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        unitParticle = GetComponent<UnitParticle>();
        unitAudio = GetComponent<UnitAudio>();
    }

    private void OnEnable()
    {
        healthSystem.OnDead += HandleOnDead;
    }

    private void OnDisable()
    {
        healthSystem.OnDead -= HandleOnDead;
    }

    public virtual void HandleOnDead()
    {
        unitParticle.PlayDeadParticle();
        unitAudio.PlayDeadSound();

        OnDead?.Invoke();
    }

    protected void Update()
    {
        if (isIdle) return;

        // Handle attack cooldown
        if (!canAttack)
        {
            attackCooldown -= Time.deltaTime;
            canAttack = attackCooldown <= 0;
        }
    }

    public void ResetState()
    {

    }



    public void Damage(float damageAmount)
    {
        unitParticle.PlayHitParticle();
        unitAudio.PlayHitSound();

        healthSystem.Damage(damageAmount);
    }
}
