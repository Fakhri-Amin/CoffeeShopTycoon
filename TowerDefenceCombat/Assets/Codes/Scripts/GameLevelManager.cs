using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelManager : MonoBehaviour
{
    public static GameLevelManager Instance { get; private set; }
    private CoinManager coinManager;
    private LevelManager levelManager;
    private PauseManager pauseManager;
    private EnemyUnitSpawner enemyUnitSpawner;
    private PlayerUnitSpawner playerUnitSpawner;

    private void Awake()
    {
        Instance = this;

        coinManager = GetComponent<CoinManager>();
        levelManager = GetComponent<LevelManager>();
        pauseManager = GetComponent<PauseManager>();
        enemyUnitSpawner = FindObjectOfType<EnemyUnitSpawner>();
        playerUnitSpawner = FindObjectOfType<PlayerUnitSpawner>();
    }

    private void Start()
    {
        enemyUnitSpawner.Initialize(levelManager.CurrentLevelWave);
        playerUnitSpawner.Initialize(GameDataManager.Instance.SelectedUnitList, GameDataManager.Instance.SeedProductionRate);

        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, OnLevelWin);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, OnLevelLose);
        EventManager<UnitData>.Subscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
        EventManager.Subscribe(Farou.Utility.EventType.OnEnemyBaseDestroyed, HandleEnemyBaseDestroyed);

        coinManager.SetMapCurrency(levelManager.CurrentLevelWave.MapType);
        coinManager.UpdateCoinUI();

    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, OnLevelWin);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, OnLevelLose);
        EventManager<UnitData>.UnSubscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnEnemyBaseDestroyed, HandleEnemyBaseDestroyed);
    }

    private void OnLevelWin()
    {
        if (this != null && levelManager != null)
        {
            StartCoroutine(levelManager.HandleLevelWin());
        }
    }

    private void OnLevelLose()
    {
        if (this != null && levelManager != null)
        {
            StartCoroutine(levelManager.HandleLevelLose());
        }
    }

    private void HandleEnemyCoinDropped(UnitData unitData)
    {
        if (levelManager.CurrentLevelWave.MapType == MapType.Dungeon)
        {
            coinManager.AddCoins(Mathf.RoundToInt(unitData.CoinReward * 0.1f));
        }
        else
        {
            coinManager.AddCoins(unitData.CoinReward);
        }
    }

    private void HandleEnemyBaseDestroyed()
    {
        coinManager.AddCoins(levelManager.CurrentLevelWave.BaseHealth);
    }
}

