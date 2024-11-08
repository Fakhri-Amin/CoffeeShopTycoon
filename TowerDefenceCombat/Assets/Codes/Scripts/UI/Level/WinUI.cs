using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CanvasGroup popup;
    [SerializeField] private TMP_Text coinCollectedText;
    [SerializeField] private Image coinImage;
    [SerializeField] private Image coinOutline;
    [SerializeField] private Button continueButton;

    [Header("Gold Coin")]
    [SerializeField] private Color goldCoinOutlineColor;
    [SerializeField] private Color goldCoinButtonColor;

    [Header("Azure Coin")]
    [SerializeField] private Color azureCoinOutlineColor;
    [SerializeField] private Color azureCoinButtonColor;


    public void Show(float coinCollectedAmount, Action onContinueButtonClicked)
    {
        // AudioManager.Instance.PlayCoinFeedbacks();

        // popup.alpha = 0;
        popup.gameObject.SetActive(true);
        popup.DOFade(1, 0.1f);

        coinCollectedText.text = "+" + coinCollectedAmount;

        coinImage.sprite = gameAssetSO.GoldCoinSprite;
        coinOutline.color = goldCoinOutlineColor;

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() =>
        {
            // AudioManager.Instance.PlayClickFeedbacks();
            Hide();
            onContinueButtonClicked?.Invoke();
        });
    }

    public void Hide()
    {
        // popup.alpha = 1;
        popup.DOFade(0, 0.1f).OnComplete(() =>
        {
            popup.gameObject.SetActive(false);
        });
    }

    public void InstantHide()
    {
        popup.alpha = 0;
        popup.gameObject.SetActive(false);
    }
}
