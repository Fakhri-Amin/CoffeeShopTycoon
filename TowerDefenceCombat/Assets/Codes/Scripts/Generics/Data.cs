using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class Data
{
    [SerializeReference]
    public List<BaseData> Datas = new List<BaseData>();

    public Data(bool setSingleton, params System.Type[] datas)
    {
        if (setSingleton)
        {
            dataController = this;
        }

        foreach (var dataType in datas)
        {
            var dt = (BaseData)Activator.CreateInstance(dataType);
            Datas.Add(dt);
        }
    }

    private static Data dataController;

    public static void Save<T>() where T : BaseData
    {
        dataController.save<T>();
    }

    public static void Load<T>() where T : BaseData
    {
        dataController.load<T>();
    }

    public static T Get<T>() where T : BaseData
    {
        return dataController.get<T>();
    }

    public static void Load()
    {
        dataController.load();
    }

    public static void Save()
    {
        dataController.save();
    }

    public T get<T>() where T : BaseData
    {
        return Datas.Where(d => d is T).First() as T;
    }

    public void save<T>() where T : BaseData
    {
        foreach (var data in Datas)
        {
            if (data is T)
            {
                data.Save();
            }
        }
    }

    public void load<T>() where T : BaseData
    {
        foreach (var data in Datas)
        {
            if (data is T)
            {
                data.Load();
            }
        }
    }

    public void save()
    {
        foreach (var data in Datas)
        {
            data.Save();
        }
    }
    public void load()
    {
        foreach (var data in Datas)
        {
            data.Load();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        Data data = new Data(true, GetDataClass().ToArray());
        data.load();
    }

    public static List<System.Type> GetDataClass()
    {
        System.Type baseType = typeof(BaseData);

        System.Reflection.Assembly[] allAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

        List<System.Type> dataTypes = new List<System.Type>();

        foreach (var assembly in allAssemblies)
        {
            System.Type[] allTypes = assembly.GetTypes()
            .Where(t => t != baseType && baseType.IsAssignableFrom(t) && !t.IsAbstract)
            .ToArray();

            dataTypes.AddRange(allTypes);
        }

        return dataTypes;
    }
}

[System.Serializable]
public class BaseData
{
    public virtual string Name { get; }
    public virtual string Key { get; }

    public virtual void Load()
    {
        if (PlayerPrefs.HasKey(Key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(Key), this);
        }
        else
        {
            OnNoData();
        }
    }

    public virtual void LoadFromJSON(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
    }

    public virtual void Save()
    {
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(this));
    }
    public virtual void OnNoData()
    {
        Save();
    }
}