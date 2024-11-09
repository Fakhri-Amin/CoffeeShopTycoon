using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using DG.Tweening;

public class ResearchUI : MonoBehaviour
{
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CanvasGroup panel;

    [Header("Research")]
    [SerializeField] private Button upgradeResearchButton;
    [SerializeField] private TMP_Text currentResearchLevelText;
    [SerializeField] private TMP_Text upgradeResearchPriceText;
    [SerializeField] private Image buttonIcon;

    [Header("Warning Label")]
    [SerializeField] private Image timerIcon;
    [SerializeField] private Transform remainingDayTransform;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;

    private GameDataManager gameDataManager;
    private ResearchManager researchManager;

    private void Awake()
    {
        upgradeResearchButton.onClick.AddListener(UpgradeResearch);
    }

    private void Start()
    {
        gameDataManager = GameDataManager.Instance;
        HandleUpdateUI(gameDataManager.ResearchLevel, gameDataManager.UpgradeResearchPrice);

        // Hide();
        panel.gameObject.SetActive(false);
        UnlockButton();
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnResearchLevelChanged += HandleUpdateUI;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnResearchLevelChanged -= HandleUpdateUI;
    }

    public void Initialize(ResearchManager researchManager)
    {
        this.researchManager = researchManager;
    }

    public void HandleUpdateUI(float level, float price)
    {
        currentResearchLevelText.text = "Level " + level;

        if (price >= 1000000)
        {
            // Format for values over a million (e.g., 1.2M for 1,200,000)
            upgradeResearchPriceText.text = (price / 1000000f).ToString("0.#") + "M";
        }
        else if (price >= 100000)
        {
            // Format for values over 100,000 without decimals (e.g., 123K for 123,456)
            upgradeResearchPriceText.text = (price / 1000f).ToString("0") + "K";
        }
        else if (price >= 1000)
        {
            // Format for values below 100,000 with one decimal (e.g., 1.23K for 1,230)
            upgradeResearchPriceText.text = (price / 1000f).ToString("0.##") + "K";
        }
        else
        {
            // Display the value normally if below 1000
            upgradeResearchPriceText.text = price.ToString();
        }
    }

    private void UpgradeResearch()
    {
        AudioManager.Instance.PlayCoinSound();
        var gameDataManager = GameDataManager.Instance;
        if (gameDataManager.GoldCoin >= gameDataManager.UpgradeResearchPrice)
        {
            gameDataManager.ModifyCoin(CurrencyType.GoldCoin, -gameDataManager.UpgradeResearchPrice);
            researchManager.SetUpgradeDay(GameDataManager.Instance.CurrentDay); // It takes one day to upgrade
        }
        else
        {
            FloatingTextObjectPool.Instance.DisplayInsufficientGoldCoin();
        }
    }

    public void UnlockButton()
    {
        remainingDayTransform.gameObject.SetActive(false);
        buttonIcon.sprite = gameAssetSO.GoldCoinSprite;
        upgradeResearchButton.interactable = true;
        upgradeResearchButton.GetComponent<Image>().color = unlockedColor;
        upgradeResearchPriceText.gameObject.SetActive(true);
        StopAllCoroutines();
    }

    public void LockButton()
    {
        remainingDayTransform.gameObject.SetActive(true);
        buttonIcon.sprite = gameAssetSO.LockedSprite;
        upgradeResearchButton.interactable = false;
        upgradeResearchButton.GetComponent<Image>().color = lockedColor;
        upgradeResearchPriceText.gameObject.SetActive(false);
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
        AudioManager.Instance.PlayClickSound();

        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        panel.DOFade(1, 0.1f);
    }

    public void Hide()
    {
        AudioManager.Instance.PlayClickSound();

        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}
