using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Farou.Utility;

public class DraggableItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isUnitFromInventory = true;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public UnitData UnitData;

    private Image image;
    private UnitSelectionUI unitSelectionUI;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(UnitSelectionUI unitSelectionUI, UnitData unitData)
    {
        UnitData = unitData;
        this.unitSelectionUI = unitSelectionUI;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlayClickFeedbacks();

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        if (!isUnitFromInventory)
        {
            unitSelectionUI.ShowRemoveArea();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Reset();

        if (!isUnitFromInventory)
        {
            unitSelectionUI.HideRemoveArea();
        }

        AudioManager.Instance.PlayClickFeedbacks();
    }

    public void Reset()
    {
        transform.SetParent(parentAfterDrag, false);
        transform.SetAsFirstSibling();
        image.raycastTarget = true;

        if (!isUnitFromInventory)
        {
            unitSelectionUI.HideRemoveArea();
        }
    }
}
