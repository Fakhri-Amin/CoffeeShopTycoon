using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingTextUI : MonoBehaviour
{
    public void DisplayInfoText(string text)
    {
        TMP_Text infoText = FloatingTextObjectPool.Instance.GetPooledObject();

        infoText.text = text;

        infoText.transform.DOLocalMoveY(300, 3f).SetEase(Ease.Linear);
        infoText.GetComponent<CanvasGroup>().DOFade(0, 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            FloatingTextObjectPool.Instance.ReturnToPool(infoText);
        });
    }
}
