using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUnitSpawner : MonoBehaviour
{
    public static PlayerUnitSpawner Instance { get; private set; }

    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();
    [SerializeField] private Transform gridLayout;
    [SerializeField] private PlayerUnitHero selectedUnit;

    private List<PlayerUnitHero> unlockedUnitHeroList = new List<PlayerUnitHero>();

    public List<PlayerUnitHero> UnlockedUnitHeroList => unlockedUnitHeroList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerUnit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
    }

    private void OnDisable()
    {
        PlayerUnit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
    }

    private void OnDestroy()
    {
        PlayerUnit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
    }

    private void Update()
    {

    }

    public void Initialize(List<PlayerUnitHero> unlockedUnitHeroList)
    {
        this.unlockedUnitHeroList = unlockedUnitHeroList;
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(PlayerUnit unit)
    {
        if (unit && unit.UnitType == UnitType.Player)
        {
            spawnedUnits.Remove(unit);
            unit.ResetState(); // Reset unit state before returning it to the pool
            UnitObjectPool.Instance.ReturnToPool(unit.UnitData.UnitHero, unit);
        }
    }

    public Vector3 GetUnitPosition(Unit unit)
    {
        Unit foundUnit = spawnedUnits.Find(i => i == unit);
        return foundUnit ? foundUnit.transform.position : Vector3.zero;
    }

    public void OnUnitSpawn(Vector2 position)
    {
        PlayerUnitData unitData = unitDataSO?.PlayerUnitStatDataList.Find(i => i.UnitHero == selectedUnit);
        if (unitData == null)
        {
            Debug.LogWarning("Unit data not found for unitHero: " + selectedUnit);
            return;
        }

        // var unitSeedCost = unitData.SeedCost;
        // if (SeedCount < unitSeedCost) return;

        SpawnUnit(unitData, position);
    }

    private void SpawnUnit(PlayerUnitData unitData, Vector2 position)
    {
        PlayerUnit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitData.UnitHero);
        if (spawnedUnit == null)
        {
            Debug.LogWarning((object)("No available pooled object for unit: " + unitData.UnitHero));
            return;
        }

        ModifyCoin(-unitData.CoinCost);
        spawnedUnit.transform.position = position;

        InitializeSpawnedUnit(spawnedUnit, unitData);
        spawnedUnits.Add(spawnedUnit);
    }

    private void InitializeSpawnedUnit(PlayerUnit unit, PlayerUnitData unitData)
    {
        float totalAttackDamage = 0;
        float attackDamageBoost = unitData.DamageAmount * totalAttackDamage / 100;

        float totalUnitHealth = 0;
        float unitHealthBoost = unitData.Health * totalUnitHealth / 100;

        float moveSpeed = 0;
        float attackSpeed = unitDataSO.AttackSpeedDataList
            .Find(i => i.UnitAttackSpeedType == unitData.AttackSpeedType).AttackSpeed;

        unit.InitializeUnit(UnitType.Player, unitData,
            attackDamageBoost, unitHealthBoost, moveSpeed, attackSpeed);
    }

    private void ModifyCoin(float amount)
    {
        GameDataManager.Instance.ModifyCoin(CurrencyType.GoldCoin, amount);
    }

    public void SetSelectedUnit(PlayerUnitHero selectedUnit)
    {
        this.selectedUnit = selectedUnit;
    }
}
