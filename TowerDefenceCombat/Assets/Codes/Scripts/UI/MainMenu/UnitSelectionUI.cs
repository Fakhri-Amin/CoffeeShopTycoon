using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using DG.Tweening;

public class UnitSelectionUI : MonoBehaviour
{
    [Serializable]
    public class UnitSelectionSlotReference
    {
        public UnitHero UnitHero;
        public UnitSelectionSlotUI Slot;
    }

    [SerializeField] private CanvasGroup panel;
    [SerializeField] private UnitDataSO unitDataSO;
    [SerializeField] private UnitDetailInfoUI unitDetailInfoUI;


    [Header("Unit Selected UI")]
    [SerializeField] private List<UnitSelectedSlotUI> selectedSlotUIs = new List<UnitSelectedSlotUI>();

    [Header("Unit Selection UI")]
    [SerializeField] private UnitSelectionSlotUI unitSlotTemplate;
    [SerializeField] private Transform parent;
    [SerializeField] private UnitRemoveAreaUI unitRemoveAreaUI;
    [SerializeField] private UnitSelectionGuideUI unitSelectionGuideUI;
    [SerializeField] private Button unitSelectionGuideButton;


    private void Awake()
    {
        unitSelectionGuideButton.onClick.AddListener(() =>
        {
            unitSelectionGuideUI.Show();
        });
    }

    private void Start()
    {
        // Hide();
        panel.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        var gameDataManager = GameDataManager.Instance;
        List<UnitHero> selectedUnitList = gameDataManager.SelectedUnitList;
        List<UnitHero> unlockedUnitList = gameDataManager.UnlockedUnitList;

        unitSlotTemplate.gameObject.SetActive(false);
        HideRemoveArea();

        InitializeUnitSlots(unlockedUnitList);

        InitializeUnitSelectedSlots(selectedUnitList);

        unitRemoveAreaUI.Initialize(this);
    }

    private void InitializeUnitSlots(List<UnitHero> unlockedUnitList)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<UnitSelectionSlotUI>() == unitSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (var item in unlockedUnitList)
        {
            UnitData unitData = unitDataSO.UnitStatDataList.First(i => i.UnitHero == item);
            var slotUI = Instantiate(unitSlotTemplate, parent);
            slotUI.gameObject.SetActive(true);
            slotUI.Initialize(this, unitData, () =>
            {
                unitDetailInfoUI.Select(unitData);
                AudioManager.Instance.PlayClickFeedbacks();
            });
        }
    }

    public void InitializeUnitSelectedSlots(List<UnitHero> selectedUnitList)
    {
        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            UnitData unitData = unitDataSO.UnitStatDataList.Find(x => x.UnitHero == selectedUnitList[i]);
            selectedSlotUIs[i].Initialize(this, unitData);
        }
    }

    public bool IsUnitAlreadyInUse(UnitData unitData)
    {
        if (unitData == null) return false;

        return selectedSlotUIs.Exists(i => i.UnitData == unitData);
    }

    public void RemoveUnitFromSelectedUnitSlot(UnitData unitData)
    {
        var selectedSlot = selectedSlotUIs.Find(i => i.UnitData == unitData);
        if (selectedSlot != null)
        {
            selectedSlot.RemoveUnit();
        }
    }

    public UnitSelectedSlotUI GetSelectedSlotWithUnit(UnitData unitData)
    {
        return selectedSlotUIs.Find(slot => slot.UnitData == unitData);
    }

    public void SetSelectedUnit()
    {
        // Convert selectedSlotUIs to UnitHero list, using UnitHero.None for null UnitData
        List<UnitHero> selectedUnitList = selectedSlotUIs
            .Select(slot => slot.UnitData != null && slot.UnitData.UnitHero != UnitHero.None
                ? slot.UnitData.UnitHero // If UnitData and UnitHero are valid, use the UnitHero
                : UnitHero.None) // Otherwise, assign UnitHero.None
            .ToList();

        // Save the selected units to the game manager
        GameDataManager.Instance.SetSelectedUnit(selectedUnitList);
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
    }

    public void ShowRemoveArea()
    {
        unitRemoveAreaUI.gameObject.SetActive(true);
    }

    public void HideRemoveArea()
    {
        unitRemoveAreaUI.gameObject.SetActive(false);
    }
}

