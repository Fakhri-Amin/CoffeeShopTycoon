using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using Farou.Utility;

public class UIEffectObjectPool : Singleton<UIEffectObjectPool>
{
    [System.Serializable]
    public class UIEffectReference
    {
        public CurrencyType Type;
        public Image Image;
        public Transform ParentTransform;
        [HideInInspector] public ObjectPool<Image> ObjectPool;
    }

    [SerializeField] private List<UIEffectReference> effectObjectPool = new List<UIEffectReference>();
    private void Start()
    {
        foreach (var item in effectObjectPool)
        {
            item.ObjectPool = new ObjectPool<Image>(() =>
            {
                return Instantiate(item.Image, item.ParentTransform);
            }, obj =>
            {
                obj.gameObject.SetActive(true);
            }, obj =>
            {
                obj.gameObject.SetActive(false);
            }, obj =>
            {
                Destroy(obj.gameObject);
            }, false, 20, 30);
        }
    }

    public Image GetPooledObject(CurrencyType type)
    {
        return effectObjectPool.Find(i => i.Type == type).ObjectPool.Get();
    }

    public void ReturnToPool(CurrencyType type, Image image)
    {
        effectObjectPool.Find(i => i.Type == type).ObjectPool.Release(image);
    }
}
