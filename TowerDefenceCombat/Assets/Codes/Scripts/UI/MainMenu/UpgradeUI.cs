using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using DG.Tweening;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CanvasGroup panel;

    [Header("Bonus Coin Reward Percentage")]
    [SerializeField] private TMP_Text currentBonusCoinRewardText;
    [SerializeField] private Button upgradeBonusCoinRewardButton;
    [SerializeField] private TMP_Text upgradeBonusCoinRewardPriceText;
    [SerializeField] private Image coinIcon;

    [Header("Warning Label")]
    [SerializeField] private Image timerIcon;
    [SerializeField] private Transform remainingDayTransform;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;

    private GameDataManager gameDataManager;
    private UpgradeManager upgradeManager;

    private void Awake()
    {
        upgradeBonusCoinRewardButton.onClick.AddListener(UpgradeBonusCoinRewardPercentage);
    }

    private void Start()
    {
        gameDataManager = GameDataManager.Instance;
        HandleUpdateUI(gameDataManager.BonusCoinRewardPercentage, gameDataManager.UpgradeBonusCoinRewardPrice);

        // Hide();
        panel.gameObject.SetActive(false);
        UnlockButton();
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnBonusCoinRewardPercentageChanged += HandleUpdateUI;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnBonusCoinRewardPercentageChanged -= HandleUpdateUI;
    }

    public void Initialize(UpgradeManager upgradeManager)
    {
        this.upgradeManager = upgradeManager;
    }

    public void HandleUpdateUI(float percentage, float price)
    {
        currentBonusCoinRewardText.text = percentage + "%";

        if (price >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            upgradeBonusCoinRewardPriceText.text = (price / 1000000f).ToString("0.#") + "M";
        }
        else if (price >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            upgradeBonusCoinRewardPriceText.text = (price / 1000f).ToString("0") + "K";
        }
        else if (price >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 1.23K for 1,230)
            upgradeBonusCoinRewardPriceText.text = (price / 1000f).ToString("0.##") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            upgradeBonusCoinRewardPriceText.text = price.ToString();
        }
    }

    private void UpgradeBonusCoinRewardPercentage()
    {
        AudioManager.Instance.PlayCoinFeedbacks();
        var gameDataManager = GameDataManager.Instance;
        if (gameDataManager.GoldCoin >= gameDataManager.UpgradeBonusCoinRewardPrice)
        {
            gameDataManager.ModifyCoin(CurrencyType.GoldCoin, -gameDataManager.UpgradeBonusCoinRewardPrice);
            upgradeManager.SetUpgradeDay(GameDataManager.Instance.CurrentDay); // It takes one day to upgrade
        }
        else
        {

            FloatingTextObjectPool.Instance.DisplayInsufficientGoldCoin();
        }
    }

    public void UnlockButton()
    {
        remainingDayTransform.gameObject.SetActive(false);
        coinIcon.sprite = gameAssetSO.GoldCoinSprite;
        upgradeBonusCoinRewardButton.interactable = true;
        upgradeBonusCoinRewardButton.GetComponent<Image>().color = unlockedColor;
        upgradeBonusCoinRewardPriceText.gameObject.SetActive(true);
        StopAllCoroutines();
    }

    public void LockButton()
    {
        remainingDayTransform.gameObject.SetActive(true);
        coinIcon.sprite = gameAssetSO.LockedSprite;
        upgradeBonusCoinRewardButton.interactable = false;
        upgradeBonusCoinRewardButton.GetComponent<Image>().color = lockedColor;
        upgradeBonusCoinRewardPriceText.gameObject.SetActive(false);
        StartCoroutine(RotateLockSprite());
    }

    private IEnumerator RotateLockSprite()
    {
        while (true)
        {
            if (timerIcon.transform.rotation.z == 0)
            {
                timerIcon.transform.DORotate(new Vector3(0, 0, -180.01f), 1f, RotateMode.FastBeyond360);
            }
            else
            {
                timerIcon.transform.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.FastBeyond360);
            }
            yield return new WaitForSeconds(2);
        }
    }

    public void Show()
    {
        AudioManager.Instance.PlayClickFeedbacks();

        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f);
    }

    public void Hide()
    {
        AudioManager.Instance.PlayClickFeedbacks();

        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}
