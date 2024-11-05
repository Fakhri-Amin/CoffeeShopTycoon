using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using DG.Tweening;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;

    [Header("Seed Production Rate")]
    [SerializeField] private TMP_Text currentSeedRateText;
    [SerializeField] private Button upgradeSeedRateButton;
    [SerializeField] private TMP_Text upgradeSeedRatePriceText;

    [Header("Base Health")]
    [SerializeField] private TMP_Text currentBaseHealthText;
    [SerializeField] private Button upgradeBaseHealthButton;
    [SerializeField] private TMP_Text upgradeBaseHealthPriceText;

    private GameDataManager gameDataManager;

    private void Awake()
    {
        upgradeSeedRateButton.onClick.AddListener(UpgradeSeedProductionRate);
        upgradeBaseHealthButton.onClick.AddListener(UpgradeBaseHealth);
    }

    private void Start()
    {
        gameDataManager = GameDataManager.Instance;
        HandleUpdateSeedUI(gameDataManager.SeedProductionRate, gameDataManager.UpgradeSeedProductionRatePrice);
        HandleUpdateBaseHealth(gameDataManager.BaseHealth, gameDataManager.UpgradeBaseHealthPrice);

        // Hide();
        panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnSeedProductionRateChanged += HandleUpdateSeedUI;
        GameDataManager.Instance.OnBaseHealthChanged += HandleUpdateBaseHealth;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnSeedProductionRateChanged -= HandleUpdateSeedUI;
        GameDataManager.Instance.OnBaseHealthChanged -= HandleUpdateBaseHealth;
    }

    public void HandleUpdateSeedUI(float rate, float price)
    {
        currentSeedRateText.text = rate + "/s";

        if (price >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            upgradeSeedRatePriceText.text = (price / 1000000f).ToString("0.#") + "M";
        }
        else if (price >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            upgradeSeedRatePriceText.text = (price / 1000f).ToString("0") + "K";
        }
        else if (price >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 1.23K for 1,230)
            upgradeSeedRatePriceText.text = (price / 1000f).ToString("0.##") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            upgradeSeedRatePriceText.text = price.ToString();
        }
    }

    public void HandleUpdateBaseHealth(float health, float price)
    {
        currentBaseHealthText.text = health.ToString();

        if (price >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            upgradeBaseHealthPriceText.text = (price / 1000000f).ToString("0.#") + "M";
        }
        else if (price >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            upgradeBaseHealthPriceText.text = (price / 1000f).ToString("0") + "K";
        }
        else if (price >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 1.23K for 1,230)
            upgradeBaseHealthPriceText.text = (price / 1000f).ToString("0.##") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            upgradeBaseHealthPriceText.text = price.ToString();
        }
    }

    private void UpgradeSeedProductionRate()
    {
        AudioManager.Instance.PlayCoinFeedbacks();
        var gameDataManager = GameDataManager.Instance;
        if (gameDataManager.GoldCoin >= gameDataManager.UpgradeSeedProductionRatePrice)
        {
            gameDataManager.ModifyCoin(CurrencyType.GoldCoin, -gameDataManager.UpgradeSeedProductionRatePrice);
            gameDataManager.UpgradeSeedProductionRate();
        }
        else
        {

            FloatingTextObjectPool.Instance.DisplayInsufficientGoldCoin();
        }
    }

    private void UpgradeBaseHealth()
    {
        AudioManager.Instance.PlayCoinFeedbacks();
        var gameDataManager = GameDataManager.Instance;
        if (gameDataManager.GoldCoin >= gameDataManager.UpgradeBaseHealthPrice)
        {
            gameDataManager.ModifyCoin(CurrencyType.GoldCoin, -gameDataManager.UpgradeBaseHealthPrice);
            gameDataManager.UpgradeBaseHealth();
        }
        else
        {

            FloatingTextObjectPool.Instance.DisplayInsufficientGoldCoin();
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
