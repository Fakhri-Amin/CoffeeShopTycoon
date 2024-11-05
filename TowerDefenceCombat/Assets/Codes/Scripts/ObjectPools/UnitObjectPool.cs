using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class UnitObjectPool : Singleton<UnitObjectPool>
{
    [System.Serializable]
    public class UnitHeroReference
    {
        public UnitHero Type;
        public Unit Unit;
        [HideInInspector] public Transform ParentTransform;
        [HideInInspector] public ObjectPool<Unit> ObjectPool;
    }

    [TableList(ShowIndexLabels = true)] public List<UnitHeroReference> UnitHeroReferences = new List<UnitHeroReference>();

    private void Start()
    {
        foreach (var item in UnitHeroReferences)
        {
            item.ObjectPool = new ObjectPool<Unit>(() =>
            {
                if (item.ParentTransform == null)
                {
                    item.ParentTransform = Instantiate(new GameObject(), transform).transform;
                    item.ParentTransform.name = item.Type.ToString();
                }
                return Instantiate(item.Unit, item.ParentTransform);
            }, obj =>
            {
                obj.gameObject.SetActive(true);
                obj.ResetState();
            }, obj =>
            {
                obj.gameObject.SetActive(false);
            }, obj =>
            {
                Destroy(obj.gameObject);
            }, false, 10, 20);
        }
    }

    public Unit GetPooledObject(UnitHero type)
    {
        return UnitHeroReferences.Find(i => i.Type == type).ObjectPool.Get();
    }

    public void ReturnToPool(UnitHero type, Unit effect)
    {
        UnitHeroReferences.Find(i => i.Type == type).ObjectPool.Release(effect);
    }
}


