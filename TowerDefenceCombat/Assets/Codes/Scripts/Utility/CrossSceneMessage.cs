using System;
using System.Collections;
using System.Collections.Generic;
using Farou.Utility;
using UnityEngine;

public class CrossSceneMessage : Singleton<CrossSceneMessage>
{
    private Dictionary<string, object> message = new Dictionary<string, object>();

    public new void Awake()
    {
        base.Awake();
    }

    private void send(string key, object value)
    {
        message[key] = value;
    }

    private bool has(params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!message.ContainsKey(key)) return false;
        }

        return true;
    }

    private void remove(params string[] keys)
    {
        foreach (var key in keys)
        {
            message.Remove(key);
        }
    }

    private bool getBoolean(string key)
    {
        return (bool)message[key];
    }

    private string getString(string key)
    {
        return (string)message[key];
    }

    private float getFloat(string key)
    {
        return (float)message[key];
    }

    private int getInt(string key)
    {
        return (int)message[key];
    }

    public static void Send(string key, object value) => Instance.send(key, value);

    public static bool Has(params string[] keys) => Instance.has(keys);

    public static void Remove(params string[] keys) => Instance.remove(keys);

    public static bool GetBoolean(string key) => Instance.getBoolean(key);

    public static string GetString(string key) => Instance.getString(key);

    public static float GetFloat(string key) => Instance.getFloat(key);

    public static int GetInt(string key) => Instance.getInt(key);
}