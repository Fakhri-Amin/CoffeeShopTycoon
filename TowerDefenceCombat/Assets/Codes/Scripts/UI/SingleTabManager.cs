using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleTabManager : MonoBehaviour
{
    [SerializeField] private Transform destinationPage;
    public event Action OnTabEnabled = delegate { };
    public event Action OnTabDisabled = delegate { };
    public event Action<SingleTabManager> OnClick = delegate { };

    private SingleTabUI singleTabUI;

    private void Awake()
    {
        singleTabUI = GetComponent<SingleTabUI>();
    }

    private void OnEnable()
    {
        singleTabUI.OnClick += HandleClick;
    }

    private void OnDisable()
    {
        singleTabUI.OnClick -= HandleClick;
    }

    private void HandleClick()
    {
        OnClick(this);
    }

    public void SetTabSelected()
    {
        if (destinationPage) destinationPage.gameObject.SetActive(true);

        singleTabUI.Selected();
        OnTabEnabled();
    }

    public void SetTabUnselected()
    {
        if (destinationPage) destinationPage.gameObject.SetActive(false);

        singleTabUI.Unselected();
        OnTabDisabled();
        // EventManager<PlayerCardFrameUI>.Publish(EventType.OnSelectButtonPressed, null);
    }




}
