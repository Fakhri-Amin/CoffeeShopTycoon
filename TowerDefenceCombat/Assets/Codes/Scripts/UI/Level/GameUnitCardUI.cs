using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUnitCardUI : MonoBehaviour
{
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private UnitCardUI normalUnitCardTemplate;

    [Header("Reference To Other Gameobject")]
    [SerializeField] private PlayerUnitSpawner playerUnitSpawner;

    private List<UnitCardUI> unitCardUIList = new List<UnitCardUI>();

    private void OnEnable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated += CheckForCardClickable;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnGoldCoinUpdated -= CheckForCardClickable;
    }

    private void Start()
    {
        normalUnitCardTemplate.gameObject.SetActive(false);

        List<PlayerUnitHero> unlockedUnitHeroList = GameDataManager.Instance.UnlockedUnitList;

        foreach (var item in unlockedUnitHeroList)
        {
            PlayerUnitData unitData = unitDataSO.PlayerUnitStatDataList.Find(i => i.UnitHero == item);

            if (unitData == null) continue;

            UnitCardUI unitCardUI = Instantiate(normalUnitCardTemplate, buttonParent);
            unitCardUI.Initialize(this, unitData);
            unitCardUI.gameObject.SetActive(true);
            unitCardUIList.Add(unitCardUI);
        }

        CheckForCardClickable(GameDataManager.Instance.GoldCoin);

        foreach (var item in unitCardUIList)
        {
            item.Deselect();
        }
    }

    public void SelectCard(UnitCardUI unitCardUI)
    {
        foreach (var item in unitCardUIList)
        {
            item.Deselect();
        }

        unitCardUIList.Find(i => i == unitCardUI).Select();
    }

    private void CheckForCardClickable(float currentCoin)
    {
        foreach (var item in unitCardUIList)
        {
            if (item.UnitData.CoinCost > currentCoin)
            {
                item.Disable();
            }
            else
            {
                item.Enable();
            }
        }
    }
}
