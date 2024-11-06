using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IAttackable
{
    public static event Action<Unit> OnAnyUnitDead;
    public event Action OnDead;

    [SerializeField] private UnitType unitType;
    [SerializeField] private UnitHero unitHero;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private Transform visual;
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private Image healthBar;


    protected bool isIdle;
    protected float attackDamageBoost;
    protected float moveSpeed;
    protected float attackSpeed;
    protected float attackCooldown;
    protected IAttackable targetUnit;
    protected HealthSystem healthSystem;
    protected UnitAnimation unitAnimation;
    protected UnitParticle unitParticle;
    protected UnitAudio unitAudio;
    protected Vector3 moveDirection;
    protected Vector3 basePosition;
    protected UnitData unitData;

    protected bool canAttack = true;
    public float AttackDamageBoost => attackDamageBoost;
    public IAttackable TargetUnit => targetUnit;
    public UnitData UnitData
    {
        get
        {
            return unitData;
        }
        set
        {
            unitData = value;
        }
    }
    public UnitType UnitType => unitType;
    public UnitHero UnitHero => unitHero;
    public LayerMask TargetMask => targetMask;

    public GameObject GameObject => gameObject;


    public virtual void Awake()
    {
        // healthSystem = GetComponent<HealthSystem>();
        unitAnimation = GetComponent<UnitAnimation>();
        unitParticle = GetComponent<UnitParticle>();
        unitAudio = GetComponent<UnitAudio>();
    }

    private void OnEnable()
    {
        // healthSystem.OnDead += HandleOnDead;
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDisable()
    {
        // healthSystem.OnDead -= HandleOnDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void HandleOnDead()
    {
        unitParticle.PlayDeadParticle();
        unitAudio.PlayDeadSound();

        OnDead?.Invoke();
        OnAnyUnitDead?.Invoke(this);
    }

    private void HandleLevelEnd()
    {
        isIdle = true;
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


    public void InitializeUnit(UnitType unitType, UnitData unitData, float attackDamageBoost, float unitHealthBoost, float moveSpeed, float attackSpeed)
    {
        // Set the type
        this.unitType = unitType;

        // Set the unit data
        this.unitData = unitData;

        // Set the move speed
        this.moveSpeed = moveSpeed;

        // Set the attack speed
        this.attackSpeed = attackSpeed;

        // Set the attack damage
        this.attackDamageBoost = attackDamageBoost;

        // Set the layer mask and tag
        gameObject.layer = LayerMask.NameToLayer(unitType.ToString());
        gameObject.tag = unitType.ToString();
        targetMask = LayerMask.GetMask(unitType == UnitType.Player ? "Enemy" : "Player");

        // Reset state
        // healthSystem.ResetHealth(this.unitData.Health + unitHealthBoost);

        // Set the move direction
        moveDirection = unitType == UnitType.Player ? Vector3.right : Vector3.left;

        // Set the scale
        visual.localScale = unitType == UnitType.Player
            ? new Vector3(Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z)
            : new Vector3(-Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z);

        // Set the animation to idle 
        unitAnimation.PlayIdleAnimation();
    }

    public void ResetState()
    {

    }



    public void Damage(float damageAmount)
    {
        unitParticle.PlayHitParticle();
        // unitAudio.PlayHitSound();

        // healthSystem.Damage(damageAmount);
    }
}
