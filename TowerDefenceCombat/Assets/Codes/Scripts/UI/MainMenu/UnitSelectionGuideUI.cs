using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitSelectionGuideUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        // Hide();
        panel.gameObject.SetActive(false);
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
        panel.alpha = 1;
        panel.DOFade(0, 0.1f).OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });

        AudioManager.Instance.PlayClickFeedbacks();
    }
}
