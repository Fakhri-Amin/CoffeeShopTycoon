using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Farou.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerUnitSpawner : MonoBehaviour
{
    public static PlayerUnitSpawner Instance { get; private set; }

    [Serializable]
    public class UnitGrid
    {
        public PlayerUnit Unit;
        public SingleGrid SingleGrid;
    }

    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private List<UnitGrid> unitGrids = new List<UnitGrid>();

    [Header("Ease and Animation Settings")]
    [SerializeField] private Ease scaleEase;

    private PlayerUnitHero selectedUnit;
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
        GameDataManager.Instance.OnDayChanged += ClearSelectedUnit;
    }

    private void OnDisable()
    {
        PlayerUnit.OnAnyUnitDead -= PlayerUnit_OnAnyPlayerUnitDead;
        GameDataManager.Instance.OnDayChanged -= ClearSelectedUnit;
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
            unitGrids.Remove(unitGrids.Find(i => i.Unit == unit));
            unit.ResetState(); // Reset unit state before returning it to the pool
            UnitObjectPool.Instance.ReturnToPool(unit.UnitData.UnitHero, unit);
        }
    }

    public void OnUnitSpawn(SingleGrid singleGrid, Vector2 position)
    {
        bool isOverUI = EventSystem.current.IsPointerOverGameObject();
        if (isOverUI) return;

        PlayerUnitData unitData = unitDataSO?.PlayerUnitStatDataList.Find(i => i.UnitHero == selectedUnit);
        if (unitData == null)
        {
            FloatingTextObjectPool.Instance.DisplayNoUnitSelected();
            Debug.LogWarning("Unit data not found for unitHero: " + selectedUnit);
            return;
        }

        var unitCoinCost = unitData.CoinCost;
        if (GameDataManager.Instance.GoldCoin < unitCoinCost) return;

        SpawnUnit(singleGrid, unitData, position);
    }

    private void SpawnUnit(SingleGrid singleGrid, PlayerUnitData unitData, Vector2 position)
    {
        AudioManager.Instance.PlayUnitSpawnSound();

        PlayerUnit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitData.UnitHero);
        if (spawnedUnit == null)
        {
            Debug.LogWarning((object)("No available pooled object for unit: " + unitData.UnitHero));
            return;
        }

        Vector3 defaultScale = spawnedUnit.transform.localScale;
        spawnedUnit.transform.localScale = Vector3.zero;
        spawnedUnit.transform.DOScale(defaultScale, 0.3f).SetEase(scaleEase);

        ModifyCoin(-unitData.CoinCost);
        spawnedUnit.transform.position = position;

        InitializeSpawnedUnit(spawnedUnit, unitData);
        UnitGrid unitGrid = new UnitGrid();
        unitGrid.Unit = spawnedUnit;
        unitGrid.SingleGrid = singleGrid;
        unitGrids.Add(unitGrid);
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

    public void ClearAllUnits()
    {
        foreach (var item in unitGrids)
        {
            Destroy(item.Unit.gameObject);
        }
        unitGrids.Clear();
    }

    public void ClearSelectedUnit(int currentDay)
    {
        selectedUnit = PlayerUnitHero.None;
    }

    public bool TryPlaceUnitOnGrid(SingleGrid singleGrid)
    {
        UnitGrid grid = unitGrids.FirstOrDefault(i => i.SingleGrid == singleGrid);

        if (grid == null)
        {
            OnUnitSpawn(singleGrid, singleGrid.transform.position);
            return true;
        }
        return false;
    }
}
