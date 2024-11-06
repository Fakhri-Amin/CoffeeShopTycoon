using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private LevelWaveDatabaseSO levelWaveDatabaseSO;
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private WinUI winUI;
    [SerializeField] private LoseUI loseUI;
    [SerializeField] private float waitTimeBeforeShowingUI = 3f;

    [Header("Other Reference")]
    [SerializeField] private GameObject grid;
    [SerializeField] private Transform gridLayout;
    [SerializeField] private Color gridColor1;
    [SerializeField] private Color gridColor2;
    [SerializeField] private CanvasGroup inGameHUD;
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private Slider waveProgressionBar;

    private LevelWaveSO currentLevelWave;
    private CoinManager coinManager;
    private float timePassed;

    public LevelWaveSO CurrentLevelWave => currentLevelWave;

    private void Awake()
    {
        coinManager = GetComponent<CoinManager>();
    }

    private void Start()
    {
        // InitializeLevelGraphics();
        waveProgressionBar.maxValue = currentLevelWave.DelayBetweenWaves * currentLevelWave.WaveDatas.Count;

        // Hide UI initially
        winUI.Hide();
        loseUI.Hide();

        List<int> angleList = new List<int>() { 0, 90, 180, 270 };
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject newGrid = Instantiate(grid, gridLayout);
                newGrid.transform.localPosition = new Vector3(i, -j);

                newGrid.transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angleList[Random.Range(0, angleList.Count)]);

                if ((i + j) % 2 == 0)
                {
                    newGrid.GetComponent<SpriteRenderer>().color = gridColor1;
                }
                else
                {
                    newGrid.GetComponent<SpriteRenderer>().color = gridColor2;
                }
            }
        }
    }

    private void Update()
    {
        if (waveProgressionBar.value < waveProgressionBar.maxValue)
        {
            timePassed += Time.deltaTime;
            waveProgressionBar.value = timePassed;
        }
    }

    public IEnumerator HandleLevelWin()
    {
        yield return HideInGameHUDAndWait();

        ShowWinUI();

        // GameDataManager.Instance.AddNewCompletedLevel();

        CollectCurrencyRewards();
    }

    public IEnumerator HandleLevelLose()
    {
        yield return HideInGameHUDAndWait();

        CurrencyType currencyType = currentLevelWave.MapType == MapType.Dungeon ? CurrencyType.AzureCoin : CurrencyType.GoldCoin;
        loseUI.Show(currencyType, coinManager.CoinCollected, LoadMainMenu);

        CollectCurrencyRewards();
    }

    private void CollectCurrencyRewards()
    {
        GameDataManager.Instance.SetCoinCollected(coinManager.CoinCollected);
    }

    private void ShowWinUI()
    {
        winUI.Show(coinManager.CoinCollected, OnWinUIContinue);
    }

    private void OnWinUIContinue()
    {
        // Go to Day time
    }

    private IEnumerator HideInGameHUDAndWait()
    {
        HideInGameHUD();
        yield return new WaitForSeconds(waitTimeBeforeShowingUI);
    }

    private void HideInGameHUD()
    {
        inGameHUD.blocksRaycasts = false;
        inGameHUD.DOFade(0, 0.1f);
    }

    private void LoadMainMenu()
    {
        // loadMainMenuFeedbacks.PlayFeedbacks();
    }
}
