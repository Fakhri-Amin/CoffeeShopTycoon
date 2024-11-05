using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollectedUI : MonoBehaviour
{
    [SerializeField] private Image coinImage;
    [SerializeField] private TMP_Text coinCollectedText;

    public void InitializeCoinImage(Sprite sprite)
    {
        coinImage.sprite = sprite;
    }

    public void UpdateCoinCollectedUI(float coinCollected)
    {
        coinCollectedText.text = coinCollected.ToString();
    }
}
