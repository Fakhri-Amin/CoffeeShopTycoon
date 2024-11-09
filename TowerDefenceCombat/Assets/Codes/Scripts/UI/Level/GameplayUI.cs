using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplayUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private CanvasGroup panel;

    [Header("UI : Day")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private Slider waveProgressionBar;

    [Header("UI : Unit Card")]
    [SerializeField] private Transform buttonParent;
    [SerializeField] private UnitCardUI normalUnitCardTemplate;

    [Header("UI : Research")]
    [SerializeField] private Button researchButton;

    [Header("UI : Upgrade")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private Image upgradeCloseIcon;
    [SerializeField] private TMP_Text upgradeCloseText;
    public bool isUpgradeMenuOpen;

    [Header("UI : Battle")]
    [SerializeField] private Button battleButton;

    private List<UnitCardUI> unitCardUIList = new List<UnitCardUI>();

    private void Awake()
    {
        battleButton.onClick.AddListener(() =>
        {
            GameLevelManager.Instance.SetNight();
        });
        upgradeButton.onClick.AddListener(() =>
        {
            ToggleUpgradeMenu();
        });
    }

    private void Start()
    {
        normalUnitCardTemplate.gameObject.SetActive(false);

        List<PlayerUnitHero> unlockedUnitHeroList = GameDataManager.Instance.UnlockedUnitList;

        foreach (var item in unlockedUnitHeroList)
        {
            PlayerUnitData unitData = unitDataSO.PlayerUnitStatDataList.Find(i => i.UnitHero == item);

            if (unitData == null) continue;

            UnitCardUI unitCardUI = Instantiate(normalUnitCardTemplate, buttonParent);
            unitCardUI.Initialize(this, unitData);
            unitCardUI.gameObject.SetActive(true);
            unitCardUIList.Add(unitCardUI);
        }

        foreach (var item in unitCardUIList)
        {
            item.Deselect();
        }

        CheckForCardClickable(GameDataManager.Instance.GoldCoin);
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated += CheckForCardClickable;
        GameDataManager.Instance.OnDayChanged += UpdateDayUI;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated -= CheckForCardClickable;
        GameDataManager.Instance.OnDayChanged -= UpdateDayUI;
    }

    private void ToggleUpgradeMenu()
    {
        isUpgradeMenuOpen = !isUpgradeMenuOpen;

        if (isUpgradeMenuOpen)
            OpenUpgradeMenu();
        else
            CloseUpgradeMenu();
    }

    private void OpenUpgradeMenu()
    {
        upgradeUI.Show();
        SetUpgradeUIState(false);
    }

    private void CloseUpgradeMenu()
    {
        upgradeUI.Hide();
        SetUpgradeUIState(true);
    }

    private void SetUpgradeUIState(bool isSelectionClosed)
    {
        upgradeIcon.gameObject.SetActive(isSelectionClosed);
        upgradeText.gameObject.SetActive(isSelectionClosed);
        upgradeCloseIcon.gameObject.SetActive(!isSelectionClosed);
        upgradeCloseText.gameObject.SetActive(!isSelectionClosed);
    }

    private void UpdateDayUI(int currentDay)
    {
        dayText.text = $"Day {currentDay}";
    }

    public void SetWaveProgressionMaxValue(float maxValue)
    {
        waveProgressionBar.maxValue = maxValue;
        UpdateWaveProgressionUI(0);
    }

    public void UpdateWaveProgressionUI(float progressValue)
    {
        waveProgressionBar.value = progressValue;
    }

    public void SelectCard(UnitCardUI unitCardUI)
    {
        foreach (var item in unitCardUIList)
        {
            item.Deselect();
        }

        unitCardUIList.Find(i => i == unitCardUI).Select();
    }

    private void CheckForCardClickable(float currentCoin)
    {
        foreach (var item in unitCardUIList)
        {
            if (item.UnitData.CoinCost > currentCoin)
            {
                item.Disable();
            }
            else
            {
                item.Enable();
            }
        }
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f);
    }

    public void Hide()
    {
        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}
