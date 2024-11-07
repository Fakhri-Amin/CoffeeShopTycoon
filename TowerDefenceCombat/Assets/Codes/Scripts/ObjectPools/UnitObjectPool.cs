using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class UnitObjectPool : Singleton<UnitObjectPool>
{
    [System.Serializable]
    public class PlayerUnitHeroReference
    {
        public PlayerUnitHero Type;
        public PlayerUnit Unit;
        [HideInInspector] public Transform ParentTransform;
        [HideInInspector] public ObjectPool<PlayerUnit> ObjectPool;
    }

    [System.Serializable]
    public class EnemyUnitHeroReference
    {
        public EnemyUnitHero Type;
        public EnemyUnit Unit;
        [HideInInspector] public Transform ParentTransform;
        [HideInInspector] public ObjectPool<EnemyUnit> ObjectPool;
    }

    [TableList(ShowIndexLabels = true)] public List<PlayerUnitHeroReference> PlayerUnitHeroReferences = new List<PlayerUnitHeroReference>();
    [TableList(ShowIndexLabels = true)] public List<EnemyUnitHeroReference> EnemyUnitHeroReferences = new List<EnemyUnitHeroReference>();

    private void Start()
    {
        foreach (var item in PlayerUnitHeroReferences)
        {
            item.ObjectPool = new ObjectPool<PlayerUnit>(() =>
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
            item.ObjectPool = new ObjectPool<EnemyUnit>(() =>
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

    public PlayerUnit GetPooledObject(PlayerUnitHero unitHero)
    {
        return PlayerUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Get();
    }

    public EnemyUnit GetPooledObject(EnemyUnitHero unitHero)
    {
        return EnemyUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Get();
    }

    public void ReturnToPool(PlayerUnitHero unitHero, PlayerUnit effect)
    {
        PlayerUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Release(effect);
    }

    public void ReturnToPool(EnemyUnitHero unitHero, EnemyUnit effect)
    {
        EnemyUnitHeroReferences.Find(i => i.Type == unitHero).ObjectPool.Release(effect);
    }
}


