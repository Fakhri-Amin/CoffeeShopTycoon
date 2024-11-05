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
    [SerializeField] private UnitLevelRewardUI unitLevelRewardUI;
    // [SerializeField] private MMFeedbacks loadMainMenuFeedbacks;
    [SerializeField] private SpriteRenderer worldRenderer;
    [SerializeField] private SpriteRenderer groundRenderer;

    [SerializeField] private float waitTimeBeforeShowingUI = 3f;

    [Header("Other Reference")]
    [SerializeField] private GameObject grid;
    [SerializeField] private Transform gridLayout;
    [SerializeField] private Color gridColor1;
    [SerializeField] private Color gridColor2;
    [SerializeField] private CanvasGroup inGameHUD;
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private Slider waveProgressionBar;

    private SelectedLevelMap selectedLevelMap;
    private LevelWaveSO currentLevelWave;
    private CoinManager coinManager;
    private int rewardIndex = 0;
    private float timePassed;

    public LevelWaveSO CurrentLevelWave => currentLevelWave;

    private void Awake()
    {
        coinManager = GetComponent<CoinManager>();
        selectedLevelMap = GameDataManager.Instance.SelectedLevelMap;
        currentLevelWave = levelWaveDatabaseSO.MapLevelReferences.Find(i => i.MapType == selectedLevelMap.MapType)
                                                        .Levels[selectedLevelMap.SelectedLevelIndex];
    }

    private void Start()
    {
        // InitializeLevelGraphics();
        waveProgressionBar.maxValue = currentLevelWave.DelayBetweenWaves * currentLevelWave.WaveDatas.Count;

        // Hide UI initially
        winUI.Hide();
        loseUI.Hide();
        unitLevelRewardUI.Hide();

        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject newGrid = Instantiate(grid, gridLayout);
                newGrid.transform.localPosition = new Vector3(i, -j);

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

    private void InitializeLevelGraphics()
    {
        WorldSpriteReference worldSpriteReference = gameAssetSO.WorldSpriteReferences
            .Find(i => i.MapType == currentLevelWave.MapType);
        worldRenderer.sprite = worldSpriteReference.LevelMapSprites[(int)currentLevelWave.LevelMapType];
        groundRenderer.color = worldSpriteReference.GroundColor;
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

        GameDataManager.Instance.AddNewCompletedLevel(selectedLevelMap.MapType, selectedLevelMap.SelectedLevelIndex);

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
        CurrencyType currencyType = currentLevelWave.MapType == MapType.Dungeon ? CurrencyType.AzureCoin : CurrencyType.GoldCoin;
        GameDataManager.Instance.SetCoinCollected(currencyType, coinManager.CoinCollected);
    }

    private void ShowWinUI()
    {
        CurrencyType currencyType = currentLevelWave.MapType == MapType.Dungeon ? CurrencyType.AzureCoin : CurrencyType.GoldCoin;
        winUI.Show(currencyType, coinManager.CoinCollected, OnWinUIContinue);
    }

    private void OnWinUIContinue()
    {
        rewardIndex = 0;
        ShowNextUnitReward();
    }

    private void ShowNextUnitReward()
    {
        if (rewardIndex >= currentLevelWave.UnitRewardList.Count)
        {
            LoadMainMenu();
            return;
        }

        var currentReward = currentLevelWave.UnitRewardList[rewardIndex];

        if (GameDataManager.Instance.IsUnitAlreadyUnlocked(currentReward))
        {
            rewardIndex++;
            ShowNextUnitReward();
        }
        else
        {
            UnlockAndShowReward(currentReward);
        }
    }

    private void UnlockAndShowReward(UnitHero currentReward)
    {
        GameDataManager.Instance.AddUnlockedUnit(currentReward); // Unlock the unit

        fader.DOFade(1, 0.1f).OnComplete(() =>
        {
            winUI.Hide();
            UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == currentReward);
            unitLevelRewardUI.Show(unitData, () =>
            {
                rewardIndex++;
                ShowNextUnitReward();
            });
            fader.DOFade(0, 0.1f);
        });
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
