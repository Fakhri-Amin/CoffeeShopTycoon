using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitLevelRewardUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup popup;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Image unitImage;

    public void Show(UnitData unitData, Action onContinueButtonClicked)
    {
        popup.alpha = 0;
        popup.gameObject.SetActive(true);
        popup.DOFade(1, 0.1f);

        unitNameText.text = unitData.Name;
        unitImage.sprite = unitData.Sprite;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            onContinueButtonClicked?.Invoke();
        });
    }

    public void Hide()
    {
        popup.alpha = 1;
        popup.DOFade(0, 0.1f).OnComplete(() =>
        {
            popup.gameObject.SetActive(false);
        });
    }
}
