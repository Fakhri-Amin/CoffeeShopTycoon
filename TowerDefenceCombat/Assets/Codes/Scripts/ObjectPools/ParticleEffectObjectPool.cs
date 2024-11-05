using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleEffectObjectPool : Singleton<ParticleEffectObjectPool>
{
    [System.Serializable]
    public class VisualEffectReference
    {
        public ParticleEffectType Type;
        public ParticleSystem Effect;
        [HideInInspector] public Transform ParentTransform;
        [HideInInspector] public ObjectPool<ParticleSystem> ObjectPool;
    }

    [SerializeField] private List<VisualEffectReference> visualEffectDatas = new List<VisualEffectReference>();
    private void Start()
    {
        foreach (var item in visualEffectDatas)
        {
            item.ObjectPool = new ObjectPool<ParticleSystem>(() =>
            {
                if (item.ParentTransform == null)
                {
                    item.ParentTransform = Instantiate(new GameObject(), transform).transform;
                    item.ParentTransform.name = item.Type.ToString();
                }
                return Instantiate(item.Effect, item.ParentTransform);
            }, obj =>
            {
                obj.gameObject.SetActive(true);
            }, obj =>
            {
                obj.gameObject.SetActive(false);
            }, obj =>
            {
                Destroy(obj.gameObject);
            }, false, 10, 20);
        }
    }

    public ParticleSystem GetPooledObject(ParticleEffectType type)
    {
        return visualEffectDatas.Find(i => i.Type == type).ObjectPool.Get();
    }

    public void ReturnToPool(ParticleEffectType type, ParticleSystem effect)
    {
        visualEffectDatas.Find(i => i.Type == type).ObjectPool.Release(effect);
    }
}

public enum ParticleEffectType
{
    Hit,
    Dead
}
