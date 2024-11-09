using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class QuitConfirmationUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        yesButton.onClick.AddListener(YesQuit);
        noButton.onClick.AddListener(NoCancel);
    }

    private void Start()
    {
        panel.gameObject.SetActive(false);
    }

    public void YesQuit()
    {
        AudioManager.Instance.PlayClickSound();
        Application.Quit();
    }

    public void NoCancel()
    {
        AudioManager.Instance.PlayClickSound();
        Hide();
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

        AudioManager.Instance.PlayClickSound();
    }
}
