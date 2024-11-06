using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardUI : MonoBehaviour
{
    [HideInInspector] public UnitData UnitData;

    [SerializeField] private Button button;
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text seedAmountText;
    [SerializeField] private Image frame;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inActiveColor;

    public void Initialize(UnitData unitData)
    {
        UnitData = unitData;
        unitImage.sprite = unitData.Sprite;
        seedAmountText.text = unitData.SeedCost.ToString();

        button.onClick.AddListener(() =>
        {
            // PlayerUnitSpawner.Instance.OnUnitSpawn(unitData.UnitHero);
            AudioManager.Instance.PlayClickFeedbacks();
        });

        Disable();
    }

    public void Enable()
    {
        button.interactable = true;
        unitImage.color = new Color(1, 1, 1, 1);
        frame.color = activeColor;
    }

    public void Disable()
    {
        button.interactable = false;
        unitImage.color = new Color(1, 1, 1, .3f);
        frame.color = inActiveColor;
    }
}
