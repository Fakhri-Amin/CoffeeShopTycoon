using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Farou.Utility;
using System;

public class UnitSelectedSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image image; // Unit image
    [SerializeField] private Image seedImage; // Seed image
    [SerializeField] private TMP_Text seedCost; // Cost in seeds
    [SerializeField] private DraggableItemUI draggableItemUI; // Reference to draggable item component

    private UnitSelectionUI unitSelectionUI;
    private UnitData unitData = null;

    public UnitData UnitData => unitData;

    public void Initialize(UnitSelectionUI unitSelectionUI, UnitData unitData)
    {
        this.unitSelectionUI = unitSelectionUI;
        this.unitData = unitData;

        if (unitData != null)
        {
            // If the unit is valid, populate the slot with unit info
            SelectUnit(unitData);
        }
        else
        {
            // If the slot is empty, remove the unit visuals
            RemoveUnit();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggableItem = eventData.pointerDrag.GetComponent<DraggableItemUI>();
        if (draggableItem == null) return;

        // Check if the slot you're trying to drop to already contains a unit
        if (unitSelectionUI.IsUnitAlreadyInUse(draggableItem.UnitData))
        {
            // Find the slot that already has this unit (the slot you're dragging from)
            var slotWithDraggedUnit = unitSelectionUI.GetSelectedSlotWithUnit(draggableItem.UnitData);

            // Swap units between the current slot and the slot you're dragging from
            SwapUnits(slotWithDraggedUnit);
        }
        else
        {
            // If the target slot is empty, place the dragged unit in the new slot
            SelectUnit(draggableItem.UnitData);
        }

        // Update the selected units in GameDataManager after the swap or selection
        unitSelectionUI.SetSelectedUnit();

        // Reset the draggable item to its original parent
        draggableItem.Reset();
    }

    // Swaps units between this slot and the other slot
    private void SwapUnits(UnitSelectedSlotUI otherSlot)
    {
        // If the other slot has no unit, don't perform the swap
        if (otherSlot.UnitData == null || otherSlot.UnitData.UnitHero == UnitHero.None)
        {
            return;
        }

        // Temporarily store the current unit's data
        var tempUnitData = this.unitData;

        // Set the other slot's unit data to this slot
        this.SelectUnit(otherSlot.unitData);

        // Set this slot's original unit data to the other slot
        otherSlot.SelectUnit(tempUnitData);
    }

    // Selects a unit and updates the slot UI
    public void SelectUnit(UnitData unitData)
    {
        this.unitData = unitData;

        // Ensure the unit data is valid, then update UI elements
        if (unitData != null && unitData.UnitHero != UnitHero.None)
        {
            // Initialize the draggable item and update visual elements
            draggableItemUI.Initialize(unitSelectionUI, unitData);
            EventManager.Publish(Farou.Utility.EventType.OnUnitSelected);

            image.gameObject.SetActive(true);
            seedImage.gameObject.SetActive(true);
            seedCost.gameObject.SetActive(true);
            image.sprite = unitData.Sprite;
            seedCost.text = unitData.SeedCost.ToString();
        }
        else
        {
            // If the unit data is null, deactivate UI elements
            image.gameObject.SetActive(false);
            seedImage.gameObject.SetActive(false);
            seedCost.gameObject.SetActive(false);
        }
    }

    // Removes a unit and clears the slot UI
    public void RemoveUnit()
    {
        unitData = null; // Clear the unit data
        image.gameObject.SetActive(false); // Hide the unit image
        seedImage.gameObject.SetActive(false); // Hide the seed image
        seedCost.gameObject.SetActive(false); // Hide the seed cost text
        EventManager.Publish(Farou.Utility.EventType.OnUnitSelected);
    }
}
