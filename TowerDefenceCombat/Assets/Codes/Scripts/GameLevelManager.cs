using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelManager : MonoBehaviour
{
    public enum GameState
    {
        Day,
        Night
    }

    public static GameLevelManager Instance { get; private set; }
    public GameState gameState;

    private CoinManager coinManager;
    private LevelManager levelManager;
    private PauseManager pauseManager;

    private void Awake()
    {
        Instance = this;

        coinManager = GetComponent<CoinManager>();
        levelManager = GetComponent<LevelManager>();
        pauseManager = GetComponent<PauseManager>();
    }

    private void Start()
    {
        SetDay();

        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, OnLevelWin);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, OnLevelLose);
        EventManager<float>.Subscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
        EventManager.Subscribe(Farou.Utility.EventType.OnEnemyBaseDestroyed, HandleEnemyBaseDestroyed);

        GameDataManager.Instance.OnGoldCoinUpdated += HandleCoinUpdate;
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, OnLevelWin);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, OnLevelLose);
        EventManager<float>.UnSubscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnEnemyBaseDestroyed, HandleEnemyBaseDestroyed);

        GameDataManager.Instance.OnGoldCoinUpdated -= HandleCoinUpdate;
    }

    public void SetDay()
    {
        gameState = GameState.Day;
        levelManager.ShowInGameHUD();
        HandleCoinUpdate(GameDataManager.Instance.GoldCoin);

        if (coinManager.CoinCollected > 0)
            CoinEffectManager.Instance.StartSpawnCoins(coinManager.CoinCollected);
    }

    public void SetNight()
    {
        gameState = GameState.Night;
        levelManager.StartGame();
        coinManager.Reset();
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

    private void HandleCoinUpdate(float amount)
    {
        coinManager.UpdateCoinUI(amount);
    }

    private void HandleEnemyCoinDropped(float amount)
    {
        coinManager.AddCoins(amount);
    }

    private void HandleEnemyBaseDestroyed()
    {

    }
}

