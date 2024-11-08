using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static event Action<Projectile> OnProjectileAreaHit;

    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 1.8f;
    [SerializeField] private Rigidbody2D rb;
    private PlayerUnit sourceUnit;
    private IAttackable targetUnit;
    private ProjectileType projectileType;
    private SpriteRenderer spriteRenderer;
    private Vector3 targetPosition;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component is missing.");
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (targetUnit == null)
        {
            ReturnToPool();
            return;
        }

        if (sourceUnit.UnitData.UnitRangeType == UnitRangeType.RangeStraight)
        {
            // Move towards the target position
            rb.position = Vector2.MoveTowards(rb.position, targetPosition, sourceUnit.UnitData.ProjectileSpeed * Time.deltaTime);


        }
    }

    private void LateUpdate()
    {
        // Check if the projectile has reached the target position
        if (Vector2.Distance(rb.position, targetPosition) <= 0.02f) // Adjust tolerance as needed
        {
            // ApplySingleTargetDamage();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ProjectileObjectPool.Instance.ReturnToPool(projectileType, this);
    }

    public IEnumerator CurveMovementRoutine(Vector2 start, Vector2 target, float projectileSpeed)
    {
        float timePassed = 0f;
        Vector2 end = target;
        targetPosition = target;

        while (timePassed < projectileSpeed)
        {
            timePassed += Time.deltaTime;

            float linearT = timePassed / projectileSpeed;
            float heightT = animationCurve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, heightY, heightT);
            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }

        ApplyAreaOfEffectDamage();
        ReturnToPool();
    }

    public void Initialize(PlayerUnit sourceUnit, IAttackable targetUnit, ProjectileType projectileType)
    {
        this.sourceUnit = sourceUnit;
        this.targetUnit = targetUnit;
        this.projectileType = projectileType;

        gameObject.layer = sourceUnit.gameObject.layer;

        spriteRenderer.flipY = sourceUnit.UnitType == UnitType.Player ? false : true;

        if (sourceUnit.UnitData.UnitRangeType == UnitRangeType.RangeStraight)
        {
            InitializeStraightProjectile();
        }
        else if (sourceUnit.UnitData.UnitRangeType == UnitRangeType.RangeCurve)
        {
            StartCoroutine(CurveMovementRoutine(transform.position, targetUnit.GameObject.transform.position, 1 / sourceUnit.UnitData.ProjectileSpeed));
        }
    }

    private void InitializeStraightProjectile()
    {
        targetPosition = targetUnit.GameObject.transform.position;

        // Calculate the direction toward the target
        Vector2 direction = (Vector2)(targetPosition - transform.position);

        // Rotate the projectile to face the target
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void ApplySingleTargetDamage()
    {
        // Ensure the target unit is still valid before applying damage
        if (targetUnit != null && sourceUnit.UnitData.UnitAttackType == UnitAttackType.Single)
        {
            targetUnit.Damage(sourceUnit.UnitData.DamageAmount);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IAttackable singleUnit = other.GetComponent<IAttackable>();

        // Ensure the unit is valid and belongs to a different faction
        if (singleUnit == null || singleUnit.UnitType == sourceUnit.UnitType)
        {
            return;
        }

        // Handle single target attack
        if (sourceUnit.UnitData.UnitAttackType == UnitAttackType.Single)
        {
            singleUnit.Damage(sourceUnit.UnitData.DamageAmount);
        }
        // Handle area of effect attack
        else if (sourceUnit.UnitData.UnitAttackType == UnitAttackType.Area)
        {
            ApplyAreaOfEffectDamage();
        }

        ReturnToPool();
    }

    private void ApplyAreaOfEffectDamage()
    {
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(transform.position, sourceUnit.UnitData.AreaOfEffectRadius, sourceUnit.TargetMask);

        foreach (var item in targetsInRadius)
        {
            if (item.TryGetComponent<IAttackable>(out IAttackable unit) && unit.UnitType != sourceUnit.UnitType)
            {
                OnProjectileAreaHit?.Invoke(this);
                unit.Damage(sourceUnit.UnitData.DamageAmount);
            }
        }
    }
}
