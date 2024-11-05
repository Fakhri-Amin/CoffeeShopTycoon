using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] private SingleTabManager defaultTabManager;
    [SerializeField] private List<SingleTabManager> availableTabs = new();

    private SingleTabManager lastOpenedTab;

    private void Start()
    {
        DisplayDefaultTab();
    }

    private void OnEnable()
    {
        SubscribeChild();
    }

    private void OnDisable()
    {
        // DisableAllTabs();
        UnSubscribeChild();
    }

    private void SubscribeChild()
    {
        foreach (var singleTab in availableTabs)
        {
            singleTab.OnClick += HandleClick;
        }
    }

    private void UnSubscribeChild()
    {
        foreach (var singleTab in availableTabs)
        {
            singleTab.OnClick -= HandleClick;
        }
    }

    private void HandleClick(SingleTabManager singleTabManager)
    {
        DisplayTab(singleTabManager);
    }

    private void DisplayDefaultTab()
    {
        if (!defaultTabManager) return;
        DisplayTab(defaultTabManager);
    }

    private void CloseLastTab()
    {
        if (lastOpenedTab) lastOpenedTab.SetTabUnselected();
    }

    private void DisplayTab(SingleTabManager singleTabManager)
    {
        foreach (var singleTab in availableTabs)
        {
            if (singleTab == singleTabManager)
            {
                lastOpenedTab = singleTab;
                singleTab.SetTabSelected();
            }
            else
            {
                singleTab.SetTabUnselected();
            }

        }
    }

    private void DisableAllTabs()
    {
        foreach (var singleTab in availableTabs)
        {
            singleTab.SetTabUnselected();
        }
    }

    public List<SingleTabManager> GetAvailableTabs => availableTabs;
}
