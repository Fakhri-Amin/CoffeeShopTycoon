using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;
using Farou.Utility;
using DG.Tweening;

public class FloatingTextObjectPool : Singleton<FloatingTextObjectPool>
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Transform parent;

    private ObjectPool<TMP_Text> objectPool;

    private void Start()
    {
        objectPool = new ObjectPool<TMP_Text>(() =>
        {
            return Instantiate(infoText, parent);
        }, obj =>
        {
            obj.gameObject.SetActive(true);
            obj.transform.localPosition = Vector2.zero;
            obj.GetComponent<CanvasGroup>().alpha = 1;
        }, obj =>
        {
            obj.gameObject.SetActive(false);
        }, obj =>
        {
            Destroy(obj.gameObject);
        }, false, 10, 20);
    }

    public TMP_Text GetPooledObject()
    {
        return objectPool.Get();
    }

    public void ReturnToPool(TMP_Text infoText)
    {
        objectPool.Release(infoText);
    }

    public void DisplayInsufficientGoldCoin()
    {
        string infoText = "You don't have enough gold coins!";
        DisplayFloatingText(infoText);
    }

    public void DisplayInsufficientAzureCoin()
    {
        string infoText = "You don't have enough azure coins!";
        DisplayFloatingText(infoText);
    }

    public void DisplayFloatingText(string text)
    {
        TMP_Text infoText = GetPooledObject();

        infoText.text = text;

        infoText.transform.DOLocalMoveY(300, 3f).SetEase(Ease.Linear);
        infoText.GetComponent<CanvasGroup>().DOFade(0, 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ReturnToPool(infoText);
        });
    }
}
