using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUnitSpawner : MonoBehaviour
{
    public static PlayerUnitSpawner Instance { get; private set; }
    public event Action<float> OnSeedCountChanged;
    public float SeedCount = 0;

    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private List<Unit> spawnedUnits = new List<Unit>();
    [SerializeField] private Transform gridLayout;
    [SerializeField] private UnitData selectedUnit;

    private List<UnitHero> selectedUnitHeroList = new List<UnitHero>();
    private float seedProductionRate;
    private Coroutine seedProductionCoroutine;

    public List<UnitHero> SelectedUnitTypeList => selectedUnitHeroList;

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

    private void Start()
    {
        ModifySeedCount(0);
        // seedProductionCoroutine = StartCoroutine(ProduceSeedRoutine());

        selectedUnit = unitDataSO.UnitStatDataList[1];
    }

    private void OnEnable()
    {
        Unit.OnAnyUnitDead += PlayerUnit_OnAnyPlayerUnitDead;
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void Update()
    {

    }

    public void Initialize(List<UnitHero> selectedUnitHerolist, float seedProductionRate)
    {
        this.selectedUnitHeroList = selectedUnitHerolist;
        this.seedProductionRate = seedProductionRate;
    }

    private void HandleLevelEnd()
    {
        if (seedProductionCoroutine != null)
        {
            StopCoroutine(seedProductionCoroutine);
            seedProductionCoroutine = null;
        }
    }

    private IEnumerator ProduceSeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / seedProductionRate);
            ProduceSeeds();
        }
    }

    private void ProduceSeeds()
    {
        ModifySeedCount(1);
    }

    private void PlayerUnit_OnAnyPlayerUnitDead(Unit unit)
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

    public void OnUnitSpawn(UnitHero unitHero, Vector2 position)
    {
        UnitData unitData = unitDataSO?.UnitStatDataList.Find(i => i.UnitHero == unitHero);
        if (unitData == null)
        {
            Debug.LogWarning("Unit data not found for unitHero: " + unitHero);
            return;
        }

        // var unitSeedCost = unitData.SeedCost;
        // if (SeedCount < unitSeedCost) return;

        SpawnUnit(unitData, position);
    }

    private void SpawnUnit(UnitData unitData, Vector2 position)
    {
        Unit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitData.UnitHero);
        if (spawnedUnit == null)
        {
            Debug.LogWarning("No available pooled object for unit: " + unitData.UnitHero);
            return;
        }

        ModifySeedCount(-unitData.SeedCost);
        // spawnedUnit.transform.parent = gridLayout.transform;
        spawnedUnit.transform.position = position;

        InitializeSpawnedUnit(spawnedUnit, unitData);
        spawnedUnits.Add(spawnedUnit);
    }

    private void InitializeSpawnedUnit(Unit unit, UnitData unitData)
    {
        float totalAttackDamage = 0;
        float attackDamageBoost = unitData.DamageAmount * totalAttackDamage / 100;

        float totalUnitHealth = 0;
        float unitHealthBoost = unitData.Health * totalUnitHealth / 100;

        float moveSpeed = unitDataSO.MoveSpeedDataList
            .Find(i => i.UnitMoveSpeedType == unitData.MoveSpeedType).MoveSpeed;
        float attackSpeed = unitDataSO.AttackSpeedDataList
            .Find(i => i.UnitAttackSpeedType == unitData.AttackSpeedType).AttackSpeed;

        unit.InitializeUnit(UnitType.Player, unitData,
            attackDamageBoost, unitHealthBoost, moveSpeed, attackSpeed);
    }

    private void ModifySeedCount(float amount)
    {
        SeedCount += amount;
        OnSeedCountChanged?.Invoke(SeedCount);
    }

    public void SetSelectedDefender(UnitData selectedUnit)
    {
        this.selectedUnit = selectedUnit;
    }

    private void AttemptToPlaceDefenderAt(Vector2 gridPosition)
    {
        if (selectedUnit != null)
        {
            SpawnUnit(selectedUnit, gridPosition);
        }
    }

    private Vector2 GetSquareClicked(Vector2 position)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(position);
        Vector2 gridPos = SnapToGrid(worldPos);
        return gridPos;
    }

    private Vector2 SnapToGrid(Vector2 rawWorldPos)
    {
        float newX = Mathf.RoundToInt(rawWorldPos.x);
        float newY = Mathf.RoundToInt(rawWorldPos.y);
        return new Vector2(newX, newY);
    }
}
