using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public class WeaponDecorationReference
    {
        public UnitHero UnitHero;
        public Transform Weapon;
    }

    [Header("Weapon Decoration")]
    [SerializeField] private List<WeaponDecorationReference> weaponDecorationReferences = new List<WeaponDecorationReference>();

    [SerializeField] private Transform panel;

    [Header("Selected Potato UI")]
    [SerializeField] private Image[] selectedPotatoImageArray;

    [Header("Project References")]
    [SerializeField] private UnitDataSO unitDataSO;

    private void Start()
    {
        HandleSelectedUnitListChanged(GameDataManager.Instance.SelectedUnitList);

        var unlockedUnitList = GameDataManager.Instance.UnlockedUnitList;
        foreach (var item in weaponDecorationReferences)
        {
            item.Weapon.gameObject.SetActive(false);
        }

        for (int i = 0; i < weaponDecorationReferences.Count; i++)
        {
            if (unlockedUnitList.Contains(weaponDecorationReferences[i].UnitHero))
            {
                weaponDecorationReferences[i].Weapon.gameObject.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnSelectedUnitListChanged += HandleSelectedUnitListChanged;
        HandleSelectedUnitListChanged(GameDataManager.Instance.SelectedUnitList);
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnSelectedUnitListChanged -= HandleSelectedUnitListChanged;
    }

    private void HandleSelectedUnitListChanged(List<UnitHero> unitHeroes)
    {
        foreach (var item in selectedPotatoImageArray)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < unitHeroes.Count; i++)
        {
            if (unitHeroes[i] == UnitHero.None)
            {
                continue;
            }
            else
            {
                selectedPotatoImageArray[i].gameObject.SetActive(true);
                selectedPotatoImageArray[i].sprite = unitDataSO.UnitStatDataList.Find(x => x.UnitHero == unitHeroes[i]).Sprite;
            }
        }
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}

