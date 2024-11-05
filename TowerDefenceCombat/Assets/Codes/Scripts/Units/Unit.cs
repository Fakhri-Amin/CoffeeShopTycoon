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
    [SerializeField] private Color initialHealthBarColor;

    private bool isIdle;
    private bool canMove = true;
    private float attackDamageBoost;
    private float moveSpeed;
    private float attackSpeed;
    private float attackCooldown;
    private IAttackable targetUnit;
    private HealthSystem healthSystem;
    private UnitAnimation unitAnimation;
    private UnitParticle unitParticle;
    private UnitAudio unitAudio;
    private Vector3 moveDirection;
    private Vector3 basePosition;
    private UnitData unitData;

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

    private bool canAttack = true;

    public virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        unitAnimation = GetComponent<UnitAnimation>();
        unitParticle = GetComponent<UnitParticle>();
        unitAudio = GetComponent<UnitAudio>();
    }

    private void OnEnable()
    {
        healthSystem.OnDead += HandleOnDead;
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDisable()
    {
        healthSystem.OnDead -= HandleOnDead;
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

    private void Update()
    {
        if (isIdle) return;

        // Handle attack cooldown
        if (!canAttack)
        {
            attackCooldown -= Time.deltaTime;
            canAttack = attackCooldown <= 0;
        }

        // Handle movement and attack
        if (canMove)
        {
            Move();
        }
        else if (!canMove && canAttack)
        {
            unitAnimation.PlayAttackAnimation();
            attackCooldown = attackSpeed;
            canAttack = false;
        }

        DetectEnemiesAndHandleAttack(); // Ensure enemy detection and handling is checked every frame
    }


    public void InitializeUnit(UnitType unitType, UnitData unitData, Vector3 basePosition, float attackDamageBoost, float unitHealthBoost, float moveSpeed, float attackSpeed)
    {
        // Set the type
        this.unitType = unitType;

        // Set the unit data
        this.unitData = unitData;

        // Set the base position
        this.basePosition = basePosition;

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
        healthSystem.ResetHealth(this.unitData.Health + unitHealthBoost);

        // Set the move direction
        moveDirection = unitType == UnitType.Player ? Vector3.right : Vector3.left;

        // Set the scale
        visual.localScale = unitType == UnitType.Player
            ? new Vector3(Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z)
            : new Vector3(-Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z);

        // Set the initial canMove variable
        canMove = true;

        // Set the animation to idle 
        unitAnimation.PlayIdleAnimation();
    }

    public void ResetState()
    {
        bodySprite.material.SetFloat("_HitEffectBlend", 0);
        weaponSprite.material.SetFloat("_HitEffectBlend", 0);
        healthBar.color = initialHealthBarColor;
    }

    private void Move()
    {
        // Detect for target in range
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, unitData.DetectRadius, targetMask);

        // If there is target detected
        if (targetInRange.Length > 0)
        {
            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<IAttackable>(out IAttackable attackable))
                {
                    MoveToward(attackable.GameObject.transform.position);
                    return;
                }
            }
        }

        // If there is no target detected
        MoveStraight();
    }

    private void MoveToward(Vector3 targetPosition)
    {
        // Calculate the direction towards the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the unit towards the target
        transform.position += moveSpeed * Time.deltaTime * direction;
    }

    private void MoveStraight()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void DetectEnemiesAndHandleAttack()
    {
        Collider2D[] targetInRange = Physics2D.OverlapCircleAll(transform.position, unitData.AttackRadius, targetMask);

        if (targetInRange.Length > 0)
        {
            float closestDistance = float.MaxValue;

            foreach (var enemy in targetInRange)
            {
                if (enemy.TryGetComponent<IAttackable>(out IAttackable unit))
                {
                    float distanceToBase = Mathf.Abs(Vector3.Distance(enemy.transform.position, basePosition));

                    if (distanceToBase < closestDistance)
                    {
                        closestDistance = distanceToBase;
                        targetUnit = unit;
                    }
                }
            }

            if (targetUnit != null)
            {
                canMove = false;
                return;
            }
        }

        // Reset state if no valid targets are found
        targetUnit = null;
        canMove = true;
    }

    public void Damage(float damageAmount)
    {
        unitParticle.PlayHitParticle();
        unitAudio.PlayHitSound();

        healthSystem.Damage(damageAmount);
    }
}
