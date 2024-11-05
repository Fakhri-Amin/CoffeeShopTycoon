using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SingleTabUI : MonoBehaviour
{
    [SerializeField] private bool isMainMenuTab;
    [SerializeField] private float tabExtraWidth;
    [SerializeField] private float duration;
    [SerializeField] private float iconSize;
    [SerializeField] private float iconYMoveValue = 0.3f;
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Transform iconGroup;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Transform focusBar;
    [SerializeField] private RectTransform saveAreaTransform;
    public event Action OnClick = delegate { };

    private LayoutElement layoutElement;
    private Image backgroundImage;
    private Button button;
    private Color normalColor;
    private Vector3 normalPosition;
    private RectTransform rectTransform;

    private void Awake()
    {
        button = GetComponent<Button>();
        backgroundImage = GetComponent<Image>();
        layoutElement = GetComponent<LayoutElement>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (iconGroup) normalPosition = iconGroup.transform.position;

        if (focusBar) focusBar.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        OnClick();
    }

    public void Selected()
    {
        if (isMainMenuTab)
        {
            backgroundImage.color = selectedColor;
            DOVirtual.Float(0, tabExtraWidth, duration, (x) => layoutElement.minWidth = x);
            iconGroup.transform.DOScale(iconSize, duration);
            iconGroup.transform.DOLocalMoveY(saveAreaTransform.rect.height * iconYMoveValue, duration);
            label.DOFade(1, duration);
        }
        else
        {
            focusBar.gameObject.SetActive(true);
            label.color = selectedColor;
        }
    }

    public void Unselected()
    {
        if (isMainMenuTab)
        {
            backgroundImage.color = unselectedColor;
            DOVirtual.Float(layoutElement.minWidth, 0, duration, (x) => layoutElement.minWidth = x);
            iconGroup.transform.DOScale(1, duration);
            iconGroup.transform.DOLocalMoveY(0, duration);
            label.DOFade(0, 0);
        }
        else
        {
            focusBar.gameObject.SetActive(false);
            label.color = unselectedColor;
        }
    }
}
