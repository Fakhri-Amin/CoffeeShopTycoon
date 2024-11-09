using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Farou.Utility;

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
    [SerializeField] private ResearchUI researchUI;
    [SerializeField] private Image researchIcon;
    [SerializeField] private TMP_Text researchText;
    [SerializeField] private Image researchCloseIcon;
    [SerializeField] private TMP_Text researchCloseText;
    private bool isResearchMenuOpen;

    [Header("UI : Upgrade")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private Image upgradeCloseIcon;
    [SerializeField] private TMP_Text upgradeCloseText;
    private bool isUpgradeMenuOpen;

    [Header("UI : Battle")]
    [SerializeField] private Button battleButton;

    private List<UnitCardUI> unitCardUIList = new List<UnitCardUI>();

    private void Awake()
    {
        battleButton.onClick.AddListener(() =>
        {
            if (isResearchMenuOpen) ToggleResearchMenu();
            if (isUpgradeMenuOpen) ToggleUpgradeMenu();
            AudioManager.Instance.PlayLevelStartSound();
            GameLevelManager.Instance?.SetNight();
        });

        upgradeButton.onClick.AddListener(ToggleUpgradeMenu);
        researchButton.onClick.AddListener(ToggleResearchMenu);
    }

    private void Start()
    {
        normalUnitCardTemplate.gameObject.SetActive(false);

        UpdateDayUI(GameDataManager.Instance.CurrentDay);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(Farou.Utility.EventType.OnUIRefresh, UpdateUnitCardUI);
        GameDataManager.Instance.OnGoldCoinUpdated += CheckForCardClickable;
        GameDataManager.Instance.OnDayChanged += UpdateDayUI;
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnUIRefresh, UpdateUnitCardUI);
        GameDataManager.Instance.OnGoldCoinUpdated -= CheckForCardClickable;
        GameDataManager.Instance.OnDayChanged -= UpdateDayUI;
    }

    private void UpdateUnitCardUI()
    {
        List<PlayerUnitHero> unlockedUnitHeroList = GameDataManager.Instance.UnlockedUnitList;

        foreach (Transform child in buttonParent)
        {
            if (child.GetComponent<UnitCardUI>() == normalUnitCardTemplate) continue;
            Destroy(child.gameObject);
        }
        unitCardUIList.Clear();

        foreach (var item in unlockedUnitHeroList)
        {
            PlayerUnitData unitData = unitDataSO.PlayerUnitStatDataList.Find(i => i.UnitHero == item);

            if (unitData == null) continue;

            UnitCardUI unitCardUI = Instantiate(normalUnitCardTemplate, buttonParent);
            unitCardUI.Initialize(this, unitData);
            unitCardUI.gameObject.SetActive(true);
            unitCardUIList.Add(unitCardUI);

            if (unitCardUI.UnitData.CoinCost > GameDataManager.Instance.GoldCoin)
            {
                unitCardUI.Disable();
            }
            else
            {
                unitCardUI.Enable();
            }
        }
    }

    private void ToggleMenu(ref bool isMenuOpen, System.Action openMenuAction, System.Action closeMenuAction)
    {
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
            openMenuAction();
        else
            closeMenuAction();
    }

    private void ToggleUpgradeMenu()
    {
        if (isResearchMenuOpen) ToggleResearchMenu();
        ToggleMenu(ref isUpgradeMenuOpen, OpenUpgradeMenu, CloseUpgradeMenu);
    }

    private void ToggleResearchMenu()
    {
        if (isUpgradeMenuOpen) ToggleUpgradeMenu();
        ToggleMenu(ref isResearchMenuOpen, OpenResearchMenu, CloseResearchMenu);
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

    private void OpenResearchMenu()
    {
        researchUI.Show();
        SetResearchUIState(false);
    }
    private void CloseResearchMenu()
    {
        researchUI.Hide();
        SetResearchUIState(true);
    }

    private void UpdateDayUI(int currentDay)
    {
        dayText.text = $"Day {currentDay}";

        UpdateUnitCardUI();
        CheckForCardClickable(GameDataManager.Instance.GoldCoin);
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
        ResetCardSelection();
        unitCardUIList.Find(i => i == unitCardUI)?.Select();
    }

    private void ResetCardSelection()
    {
        foreach (var item in unitCardUIList)
        {
            item.Deselect();
        }
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

    private void SetUpgradeUIState(bool isSelectionClosed)
    {
        upgradeIcon.gameObject.SetActive(isSelectionClosed);
        upgradeText.gameObject.SetActive(isSelectionClosed);
        upgradeCloseIcon.gameObject.SetActive(!isSelectionClosed);
        upgradeCloseText.gameObject.SetActive(!isSelectionClosed);
    }

    private void SetResearchUIState(bool isSelectionClosed)
    {
        researchIcon.gameObject.SetActive(isSelectionClosed);
        researchText.gameObject.SetActive(isSelectionClosed);
        researchCloseIcon.gameObject.SetActive(!isSelectionClosed);
        researchCloseText.gameObject.SetActive(!isSelectionClosed);
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
        panel.DOFade(0, 0.1f).OnComplete(() => panel.gameObject.SetActive(false));
    }
}
