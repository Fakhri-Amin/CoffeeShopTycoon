using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    public static EnemyUnitSpawner Instance { get; private set; }
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private List<EnemyUnit> spawnedUnits = new List<EnemyUnit>();
    [SerializeField] private int spawnXAxisOffset = 16;
    [SerializeField] private float spawnYAxisOffset = 0.2f;

    private List<Transform> spawnPoints;
    private LevelWaveSO levelWaveSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }


    public void Initialize(LevelWaveSO levelWaveSO, List<Transform> spawnPoints)
    {
        this.levelWaveSO = levelWaveSO;
        this.spawnPoints = spawnPoints;
        StartCoroutine(SpawnUnitWaveRoutine());
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        EnemyUnit.OnAnyUnitDead += EnemyUnit_OnAnyEnemyUnitDead;
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void UnsubscribeFromEvents()
    {
        EnemyUnit.OnAnyUnitDead -= EnemyUnit_OnAnyEnemyUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void OnDestroy()
    {
        EnemyUnit.OnAnyUnitDead -= EnemyUnit_OnAnyEnemyUnitDead;
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, HandleLevelEnd);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, HandleLevelEnd);
    }

    private void HandleLevelEnd()
    {
        foreach (var unit in spawnedUnits)
        {
            UnitObjectPool.Instance.ReturnToPool(unit.UnitData.UnitHero, unit);
        }
        spawnedUnits.Clear();
        StopAllCoroutines();
    }

    private IEnumerator SpawnUnitWaveRoutine()
    {
        float delayAtStart = levelWaveSO.DelayAtStart;
        float delayBetweenWaves = levelWaveSO.DelayBetweenWaves;
        List<WaveData> waveDatas = levelWaveSO.WaveDatas;

        yield return new WaitForSeconds(delayAtStart); // Initial delay before waves

        // Iterate through all waves
        foreach (WaveData wave in waveDatas)
        {
            yield return StartCoroutine(SpawnUnitsForWave(wave));
            yield return new WaitForSeconds(delayBetweenWaves); // Delay between waves
        }

    }

    private IEnumerator SpawnUnitsForWave(WaveData waveData)
    {
        List<WaveHeroData> waveUnitDatas = waveData.WaveHeroDatas;
        float delayBetweenUnitSpawn = levelWaveSO.DelayBetweenWaves * 0.05f;

        foreach (WaveHeroData unitData in waveUnitDatas)
        {
            // Cache unit data once to avoid redundant Find() calls
            EnemyUnitData unitStatData = unitDataSO.EnemyUnitStatDataList.Find(i => i.UnitHero == unitData.UnitType);
            if (unitStatData == null)
            {
                Debug.LogWarning($"Unit data not found for hero: {unitData.UnitType}");
                continue;
            }

            for (int i = 0; i < unitData.Count; i++)
            {
                Vector3 spawnPos = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].position +
                                new Vector3(spawnXAxisOffset, UnityEngine.Random.Range(-spawnYAxisOffset, spawnYAxisOffset));
                SpawnUnit(unitData.UnitType, unitStatData, spawnPos);
                yield return new WaitForSeconds(delayBetweenUnitSpawn);
            }
        }
    }

    private void EnemyUnit_OnAnyEnemyUnitDead(EnemyUnit unit)
    {
        if (unit && unit.UnitType == UnitType.Enemy)
        {
            EventManager<float>.Publish(Farou.Utility.EventType.OnEnemyCoinDropped, unit.UnitData.CoinReward);
            spawnedUnits.Remove(unit);
            UnitObjectPool.Instance.ReturnToPool(unit.UnitData.UnitHero, unit);
            if (spawnedUnits.Count <= 0)
            {
                EventManager.Publish(Farou.Utility.EventType.OnLevelWin);
            }
        }
    }

    public Vector3 GetUnitPosition(Unit unit)
    {
        Unit foundUnit = spawnedUnits.Find(i => i == unit);
        return foundUnit ? foundUnit.transform.position : Vector3.zero;
    }

    private void SpawnUnit(EnemyUnitHero unitHero, EnemyUnitData unitData, Vector3 spawnPosition)
    {
        Vector3 offset = new Vector3(0, UnityEngine.Random.Range(-0.5f, 0.5f), 0);
        EnemyUnit spawnedUnit = UnitObjectPool.Instance.GetPooledObject(unitHero);

        if (spawnedUnit == null)
        {
            Debug.LogWarning($"No available pooled object for unit: {unitHero}");
            return;
        }

        // Get all necessary data in one go
        var unitStats = unitDataSO.EnemyUnitStatDataList.Find(i => i.UnitHero == unitHero);
        if (unitStats == null) return;

        unitData.DamageAmount = unitStats.DamageAmount;
        unitData.Health = unitStats.Health;

        float moveSpeed = unitDataSO.MoveSpeedDataList.Find(i => i.UnitMoveSpeedType == unitData.MoveSpeedType)?.MoveSpeed ?? 0f;
        float attackSpeed = unitDataSO.AttackSpeedDataList.Find(i => i.UnitAttackSpeedType == unitData.AttackSpeedType)?.AttackSpeed ?? 0f;

        spawnedUnit.transform.position = spawnPosition;
        spawnedUnit.InitializeUnit(UnitType.Enemy, unitData,
            0 /* attackDamageBoost */, 0 /* unitHealthBoost */, moveSpeed, attackSpeed);

        spawnedUnits.Add(spawnedUnit);
    }

}
