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
    [SerializeField] private TMP_Text seedCountText;

    [Header("Reference To Other Gameobject")]
    [SerializeField] private PlayerUnitSpawner playerUnitSpawner;

    private List<UnitCardUI> unitCardUIList = new List<UnitCardUI>();

    private void OnEnable()
    {
        playerUnitSpawner.OnSeedCountChanged += OnSeedProductionCountChanged;
        playerUnitSpawner.OnSeedCountChanged += CheckForCardClickable;
    }

    private void OnDisable()
    {
        playerUnitSpawner.OnSeedCountChanged -= OnSeedProductionCountChanged;
        playerUnitSpawner.OnSeedCountChanged -= CheckForCardClickable;
    }

    private void Start()
    {
        normalUnitCardTemplate.gameObject.SetActive(false);

        List<UnitHero> unlockedUnitHeroList = GameDataManager.Instance.UnlockedUnitList;

        foreach (var item in unlockedUnitHeroList)
        {
            UnitData unitData = unitDataSO.UnitStatDataList.Find(i => i.UnitHero == item);

            if (unitData == null) continue;

            UnitCardUI unitCardUI = Instantiate(normalUnitCardTemplate, buttonParent);
            unitCardUI.Initialize(unitData);
            unitCardUI.gameObject.SetActive(true);
            unitCardUIList.Add(unitCardUI);
        }
    }

    private void OnSeedProductionCountChanged(float currentSeedCount)
    {
        seedCountText.text = currentSeedCount.ToString();
    }

    private void CheckForCardClickable(float currentSeedCount)
    {
        foreach (var item in unitCardUIList)
        {
            if (item.UnitData.SeedCost > currentSeedCount)
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
