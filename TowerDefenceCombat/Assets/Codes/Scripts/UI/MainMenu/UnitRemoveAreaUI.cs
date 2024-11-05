using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitRemoveAreaUI : MonoBehaviour, IDropHandler
{
    private UnitSelectionUI unitSelectionUI;

    public void Initialize(UnitSelectionUI unitSelectionUI)
    {
        this.unitSelectionUI = unitSelectionUI;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggableItem = eventData.pointerDrag.GetComponent<DraggableItemUI>();

        if (draggableItem != null)
        {
            draggableItem.Reset();
            unitSelectionUI.RemoveUnitFromSelectedUnitSlot(draggableItem.UnitData);
            unitSelectionUI.SetSelectedUnit();
            unitSelectionUI.HideRemoveArea();
            EventManager.Publish(Farou.Utility.EventType.OnUnitSelected);
        }
    }
}

