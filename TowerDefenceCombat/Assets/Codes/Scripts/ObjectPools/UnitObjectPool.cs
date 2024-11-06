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

    [TableList(ShowIndexLabels = true)] public List<UnitHeroReference> PlayerUnitHeroReferences = new List<UnitHeroReference>();
    [TableList(ShowIndexLabels = true)] public List<UnitHeroReference> EnemyUnitHeroReferences = new List<UnitHeroReference>();

    private void Start()
    {
        foreach (var item in PlayerUnitHeroReferences)
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

        foreach (var item in EnemyUnitHeroReferences)
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

    public Unit GetPooledObject(UnitType unitType, UnitHero unitHero)
    {
        if (unitType == UnitType.Player)
        {
            return PlayerUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Get();
        }
        else
        {
            return EnemyUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Get();
        }
    }

    public void ReturnToPool(UnitType unitType, UnitHero unitHero, Unit effect)
    {
        if (unitType == UnitType.Player)
        {
            PlayerUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Release(effect);
        }
        else
        {
            EnemyUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Release(effect);
        }
    }
}


