using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Farou.Utility;

public class UnitSelectionSlotUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text seedCostText;
    [SerializeField] private Transform inUse;
    [SerializeField] private DraggableItemUI draggableItemUI;

    private UnitData unitData;
    private UnitSelectionUI unitSelectionUI;

    public UnitData UnitData => unitData;

    private void OnEnable()
    {
        EventManager.Subscribe(Farou.Utility.EventType.OnUnitSelected, HandleUnitSelection);
    }

    private void OnDisable()
    {
        EventManager.UnSubscribe(Farou.Utility.EventType.OnUnitSelected, HandleUnitSelection);
    }

    public void Initialize(UnitSelectionUI unitSelectionUI, UnitData unitData, Action onButtonClicked)
    {
        this.unitSelectionUI = unitSelectionUI;
        this.unitData = unitData;

        unitImage.sprite = unitData.Sprite;
        unitNameText.text = unitData.Name;
        seedCostText.text = unitData.SeedCost.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onButtonClicked());

        draggableItemUI.Initialize(unitSelectionUI, unitData);
    }

    private void HandleUnitSelection()
    {
        if (unitSelectionUI.IsUnitAlreadyInUse(unitData))
        {
            SelectUnitOnList();
        }
        else
        {
            DeselectUnitOnList();
        }
    }

    public void SelectUnitOnList()
    {
        inUse.gameObject.SetActive(true);
    }

    public void DeselectUnitOnList()
    {
        inUse.gameObject.SetActive(false);
    }
}


