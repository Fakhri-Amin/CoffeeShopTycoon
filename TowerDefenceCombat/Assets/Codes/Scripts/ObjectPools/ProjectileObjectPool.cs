using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileObjectPool : Singleton<ProjectileObjectPool>
{
    [System.Serializable]
    public class ProjectileData
    {
        public ProjectileType Type;
        public Projectile Projectile;
        [HideInInspector] public Transform ParentTransform;
        [HideInInspector] public ObjectPool<Projectile> ObjectPool;
    }

    [SerializeField] private List<ProjectileData> projectileDatas = new List<ProjectileData>();

    private void Start()
    {
        foreach (var item in projectileDatas)
        {
            item.ObjectPool = new ObjectPool<Projectile>(() =>
            {
                if (item.ParentTransform == null)
                {
                    item.ParentTransform = Instantiate(new GameObject(), transform).transform;
                    item.ParentTransform.name = item.Type.ToString();
                }
                return Instantiate(item.Projectile, item.ParentTransform);
            }, obj =>
            {
                obj.gameObject.SetActive(true);
            }, obj =>
            {
                obj.gameObject.SetActive(false);
            }, obj =>
            {
                Destroy(obj.gameObject);
            }, false, 15, 25);
        }
    }

    public Projectile GetPooledObject(ProjectileType type)
    {
        return projectileDatas.Find(i => i.Type == type).ObjectPool.Get();
    }

    public void ReturnToPool(ProjectileType type, Projectile projectile)
    {
        projectileDatas.Find(i => i.Type == type).ObjectPool.Release(projectile);
    }
}
