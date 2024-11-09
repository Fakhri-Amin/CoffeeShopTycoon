using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCardUI : MonoBehaviour
{
    [HideInInspector] public PlayerUnitData UnitData;

    [SerializeField] private Button button;
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text seedAmountText;
    [SerializeField] private Image outline;
    [SerializeField] private Image frame;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inActiveColor;

    private static readonly Color TransparentWhite = new Color(1, 1, 1, 0.3f);
    private static readonly Color OpaqueWhite = new Color(1, 1, 1, 1f);

    public void Initialize(GameplayUI gameUnitCardUI, PlayerUnitData unitData)
    {
        UnitData = unitData;
        unitImage.sprite = unitData.Sprite;
        seedAmountText.text = unitData.CoinCost.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            PlayerUnitSpawner.Instance.SetSelectedUnit(unitData.UnitHero);
            gameUnitCardUI.SelectCard(this);
            AudioManager.Instance.PlayClickSound();
        });

        Deselect();
    }

    public void Select()
    {
        UpdateUI(opaque: true, frameColor: activeColor, outlineColor: selectedColor);
    }

    public void Deselect()
    {
        UpdateUI(opaque: true, frameColor: activeColor, outlineColor: activeColor);
    }

    public void Enable()
    {
        button.interactable = true;
        frame.color = activeColor;
    }

    public void Disable()
    {
        button.interactable = false;
        UpdateUI(opaque: false, frameColor: inActiveColor, outlineColor: activeColor);
    }

    private void UpdateUI(bool opaque, Color frameColor, Color outlineColor)
    {
        unitImage.color = opaque ? OpaqueWhite : TransparentWhite;
        frame.color = frameColor;
        outline.color = outlineColor;
    }
}
