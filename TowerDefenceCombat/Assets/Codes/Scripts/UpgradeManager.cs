using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private UpgradeUI upgradeUI;
    private float dayWhenIsPaidToUpgrade;
    private bool isAnyBeingUpgraded;

    private void Start()
    {
        upgradeUI.Initialize(this);
    }

    public void HandleUpgradeDayChecking(int currentDay)
    {
        if (!isAnyBeingUpgraded) return;

        var gameDataManager = GameDataManager.Instance;

        if (dayWhenIsPaidToUpgrade < currentDay)
        {
            gameDataManager.UpgradeBonusCoinRewardPercentage();
            isAnyBeingUpgraded = false;
        }
    }

    public void SetUpgradeDay(float dayWhenIsPaidToUpgrade)
    {
        this.dayWhenIsPaidToUpgrade = dayWhenIsPaidToUpgrade;
        isAnyBeingUpgraded = true;
    }

}
