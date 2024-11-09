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
    private UpgradeManager upgradeManager;
    private ResearchManager researchManager;

    private void Awake()
    {
        Instance = this;

        coinManager = GetComponent<CoinManager>();
        levelManager = GetComponent<LevelManager>();
        pauseManager = GetComponent<PauseManager>();
        upgradeManager = GetComponent<UpgradeManager>();
        researchManager = GetComponent<ResearchManager>();
    }

    private void Start()
    {
        SetDay();

        EventManager.Subscribe(Farou.Utility.EventType.OnLevelWin, OnLevelWin);
        EventManager.Subscribe(Farou.Utility.EventType.OnLevelLose, OnLevelLose);
        EventManager<float>.Subscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);

        GameDataManager.Instance.OnGoldCoinUpdated += HandleCoinUpdate;
        GameDataManager.Instance.OnDayChanged += HandleDayChanged;
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelWin, OnLevelWin);
        EventManager.UnSubscribe(Farou.Utility.EventType.OnLevelLose, OnLevelLose);
        EventManager<float>.UnSubscribe(Farou.Utility.EventType.OnEnemyCoinDropped, HandleEnemyCoinDropped);

        GameDataManager.Instance.OnGoldCoinUpdated -= HandleCoinUpdate;
        GameDataManager.Instance.OnDayChanged -= HandleDayChanged;
    }

    public void SetDay()
    {
        gameState = GameState.Day;
        levelManager.StopGame();
        HandleCoinUpdate(GameDataManager.Instance.GoldCoin);

        if (coinManager.CoinCollected > 0) // Meaning if the first time open the game, this condition is not met
        {
            GameDataManager.Instance.IncrementCurrentDay();
            CoinEffectManager.Instance.StartSpawnCoins(coinManager.CoinCollected);
            EventManager.Publish(Farou.Utility.EventType.OnUIRefresh);
        }
    }

    public void SetNight()
    {
        gameState = GameState.Night;
        levelManager.StartGame(GameDataManager.Instance.CurrentDay);
        coinManager.Reset();
    }

    public bool IsGameStateDay()
    {
        return gameState == GameState.Day;
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

    private void HandleDayChanged(int currentDay)
    {
        upgradeManager.HandleUpgradeDayChecking(currentDay);
        researchManager.HandleResearchDayChecking(currentDay);
    }

    private void HandleCoinUpdate(float amount)
    {
        coinManager.UpdateCoinUI(amount);
    }

    private void HandleEnemyCoinDropped(float amount)
    {
        coinManager.AddCoins(amount);
    }
}

