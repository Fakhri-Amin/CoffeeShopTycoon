using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Farou.Utility;
using Sirenix.OdinInspector;
using DG.Tweening;

public class MainHUD : Singleton<MainHUD>
{
    [Header("Buttons")]
    [SerializeField] private Button potatoSelectionButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button quitButton;

    [Header("Currency")]
    [SerializeField] private TMP_Text goldCoinText;
    [SerializeField] private TMP_Text azureCoinText;

    [Header("Upgrade UI")]
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private Image upgradeCloseIcon;
    [SerializeField] private TMP_Text upgradeCloseText;
    public bool isUpgradeMenuOpen;


    [Header("Quit UI")]
    [SerializeField] private QuitConfirmationUI quitConfirmationUI;

    [Header("Others")]
    [SerializeField] private CanvasGroup fader;

    public new void Awake()
    {
        base.Awake();

        upgradeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            ToggleUpgradeMenu();
        });
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        HandleGoldCoinUpdate(GameDataManager.Instance.GoldCoin);
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated += HandleGoldCoinUpdate;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated -= HandleGoldCoinUpdate;
    }

    private void HandleGoldCoinUpdate(float coin)
    {
        if (coin >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            goldCoinText.text = (coin / 1000000f).ToString("0.#") + "M";
        }
        else if (coin >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            goldCoinText.text = (coin / 1000f).ToString("0") + "K";
        }
        else if (coin >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 1.23K for 1,230)
            goldCoinText.text = (coin / 1000f).ToString("0.##") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            goldCoinText.text = coin.ToString();
        }
    }

    private void HandleAzureUpdate(float coin)
    {
        if (coin >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            azureCoinText.text = (coin / 1000000f).ToString("0.#") + "M";
        }
        else if (coin >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            azureCoinText.text = (coin / 1000f).ToString("0") + "K";
        }
        else if (coin >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 1.23K for 1,230)
            azureCoinText.text = (coin / 1000f).ToString("0.##") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            azureCoinText.text = coin.ToString();
        }
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

    private void QuitGame()
    {
        quitConfirmationUI.Show();
    }
}
