using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private LevelWaveDatabaseSO levelWaveDatabaseSO;
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private GameplayUI gameplayUI;
    [SerializeField] private WinUI winUI;
    [SerializeField] private LoseUI loseUI;
    [SerializeField] private UnitLevelRewardUI unitLevelRewardUI;
    [SerializeField] private float waitTimeBeforeShowingUI = 2f;
    [SerializeField] private SpriteRenderer nightLayer;

    [Header("Grid Settings")]
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private Transform gridLayout;
    [SerializeField] private Color gridColor1;
    [SerializeField] private Color gridColor2;

    private LevelWaveSO currentLevelWave;
    private CoinManager coinManager;
    private float timePassed;
    private const int gridRows = 11;
    private const int gridColumns = 5;
    private readonly List<int> gridRotationAngles = new() { 0, 90, 180, 270 };
    private bool isGameStart;
    private List<Transform> enemySpawnPoints = new();

    public LevelWaveSO CurrentLevelWave => currentLevelWave;

    private void Awake()
    {
        coinManager = GetComponent<CoinManager>();
    }

    private void Start()
    {
        PlayerUnitSpawner.Instance.Initialize(GameDataManager.Instance.UnlockedUnitList);

        // Hide UI elements initially
        winUI.InstantHide();
        loseUI.InstantHide();

        gameplayUI.UpdateWaveProgressionUI(0);

        nightLayer.DOFade(0, 0);

        SpawnGrids();
    }

    private void Update()
    {
        if (!isGameStart) return;
        UpdateWaveProgress();
    }

    public void StartGame(int currentDay)
    {
        isGameStart = true;

        // Initialize the current level wave
        currentLevelWave = levelWaveDatabaseSO.DayWaves[currentDay - 1];
        if (currentLevelWave == null)
        {
            Debug.LogError("No level wave data found.");
            return;
        }

        // Set the max value of the wave progress bar based on wave data
        gameplayUI.SetWaveProgressionMaxValue(currentLevelWave.DelayBetweenWaves * currentLevelWave.WaveDatas.Count);
        EnemyUnitSpawner.Instance.Initialize(CurrentLevelWave, enemySpawnPoints);
        nightLayer.DOFade(0.3f, 1f);
        Camera.main.transform.DOMoveY(0.5f, 0.5f);
        Camera.main.DOOrthoSize(5.7f, 0.5f);
        HideInGameHUD();
    }

    public void StopGame()
    {
        isGameStart = false;
        timePassed = 0;
        gameplayUI.UpdateWaveProgressionUI(0);
        nightLayer.gameObject.SetActive(true);
        nightLayer.DOFade(0f, 1f);
        Camera.main.transform.DOMoveY(-0.7f, 0.5f);
        Camera.main.DOOrthoSize(6.5f, 0.5f);
        ShowInGameHUD();
    }

    private void UpdateWaveProgress()
    {
        timePassed += Time.deltaTime;
        gameplayUI.UpdateWaveProgressionUI(timePassed);
    }

    public List<Transform> SpawnGrids()
    {
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridColumns; col++)
            {
                GameObject newGrid = Instantiate(gridPrefab, gridLayout);
                newGrid.transform.localPosition = new Vector3(row, -col);
                newGrid.transform.localRotation = Quaternion.Euler(0, 0, gridRotationAngles[UnityEngine.Random.Range(0, gridRotationAngles.Count)]);

                // Alternate colors for the grid
                newGrid.GetComponent<SpriteRenderer>().color = (row + col) % 2 == 0 ? gridColor1 : gridColor2;

                if (row == 0)
                {
                    enemySpawnPoints.Add(newGrid.transform);
                }
            }
        }

        return enemySpawnPoints;
    }

    public IEnumerator HandleLevelWin()
    {
        yield return DelayTime(waitTimeBeforeShowingUI);
        coinManager.SetFinalCoinCollected(coinManager.CoinCollected + (coinManager.CoinCollected * GameDataManager.Instance.BonusCoinRewardPercentage / 100));
        ShowWinUI();
        CollectCurrencyRewards();
    }

    public IEnumerator HandleLevelLose()
    {
        yield return DelayTime(0);
        coinManager.Reset();
        loseUI.Show(OnDayEnds);
    }

    private void CollectCurrencyRewards()
    {
        GameDataManager.Instance.SetCoinCollected(coinManager.CoinCollected);
    }

    private void ShowWinUI()
    {
        winUI.Show(coinManager.CoinCollected, OnDayEnds);
    }

    private void OnDayEnds()
    {
        // Handle what happens after the win UI continues, e.g., transition to the next level
        GameLevelManager.Instance.SetDay();
    }

    public void ShowInGameHUD()
    {
        gameplayUI.Show();
    }

    private IEnumerator DelayTime(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void HideInGameHUD()
    {
        gameplayUI.Hide();
    }

    private void LoadMainMenu()
    {
        // Implement loading the main menu
        SceneManager.LoadScene("MainMenu");
    }
}
