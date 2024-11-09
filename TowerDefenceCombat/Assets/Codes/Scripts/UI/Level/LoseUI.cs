using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class LoseUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameAssetSO gameAssetSO;
    [SerializeField] private CanvasGroup popup;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button quitGameButton;

    [Header("Gold Coin")]
    [SerializeField] private Color goldCoinOutlineColor;
    [SerializeField] private Color goldCoinButtonColor;

    public void Show(Action onEnd)
    {
        AudioManager.Instance.PlayCoinSound();

        popup.gameObject.SetActive(true);
        popup.DOFade(1, 0.3f);

        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            GameDataManager.Instance.ClearDatabase();
            // GameSceneManager.Instance.LoadCurrentScene();
            Hide();
            onEnd?.Invoke();
        });

        quitGameButton.onClick.RemoveAllListeners();
        quitGameButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayClickSound();
            GameSceneManager.Instance.QuitGame();
        });
    }

    public void Hide()
    {
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
