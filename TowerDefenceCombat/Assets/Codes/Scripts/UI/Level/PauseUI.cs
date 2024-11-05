using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Transform popup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button giveUpButton;

    public void InitializeButtonFunction(Action onResumeButtonClicked, Action onGiveUpButtonClicked)
    {
        resumeButton.onClick.RemoveAllListeners();
        giveUpButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            onResumeButtonClicked();
        });
        giveUpButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickFeedbacks();
            onGiveUpButtonClicked();
        });
    }

    public void Show()
    {
        popup.gameObject.SetActive(true);
    }

    public void Hide()
    {
        popup.gameObject.SetActive(false);
    }
}
