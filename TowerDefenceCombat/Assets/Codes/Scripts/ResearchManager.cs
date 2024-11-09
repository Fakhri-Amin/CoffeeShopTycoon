using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    [SerializeField] private ResearchUI researchUI;
    private float dayWhenIsPaidToResearch;
    private bool isAnyBeingResearched;

    private void Start()
    {
        researchUI.Initialize(this);
    }

    public void HandleResearchDayChecking(int currentDay)
    {
        if (!isAnyBeingResearched) return;

        var gameDataManager = GameDataManager.Instance;

        if (dayWhenIsPaidToResearch < currentDay)
        {
            gameDataManager.UpgradeResearchLevel();
            EventManager.Publish(Farou.Utility.EventType.OnUIRefresh);

            researchUI.UnlockButton();
            isAnyBeingResearched = false;
        }
    }

    public void SetUpgradeDay(float dayWhenIsPaidToResearch)
    {
        this.dayWhenIsPaidToResearch = dayWhenIsPaidToResearch;
        researchUI.LockButton();
        isAnyBeingResearched = true;
    }

}
