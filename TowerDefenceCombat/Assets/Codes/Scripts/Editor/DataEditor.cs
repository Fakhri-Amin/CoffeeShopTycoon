using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class DataEditor : EditorWindow
{
    public Data data;
    private GUIStyle toggleStyle;
    private List<bool> dataFoldouts = new List<bool>();
    private SerializedObject serializedObject;
    private Vector2 scrollPosition;

    [MenuItem("Farou/Data Editor")]
    public static void ShowWindow()
    {
        DataEditor window = GetWindow<DataEditor>("Data Editor");
        window.minSize = new Vector2(300, 400);

    }

    private void OnEnable()
    {
        data = new Data(false, Data.GetDataClass().ToArray());
        data.load();

        toggleStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
        {
            fontSize = 12
        };
        toggleStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
        toggleStyle.hover.textColor = toggleStyle.normal.textColor;
        toggleStyle.active.textColor = toggleStyle.normal.textColor;

        // Set the x to false if you want to the foldout to be closed on open
        dataFoldouts = data.Datas.Select(x => true).ToList();

        if (serializedObject != null)
        {
            serializedObject.Dispose();
        }

        serializedObject = new SerializedObject(this);
    }

    public void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
        {
            fontSize = 15
        };
        GUILayout.Label("Data Editor", titleStyle);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < data.Datas.Count; i++)
        {
            GUILayout.BeginVertical("helpbox");
            if (GUILayout.Button(data.Datas[i].Name, toggleStyle, GUILayout.Height(25)))
            {
                dataFoldouts[i] = !dataFoldouts[i];
            }

            if (dataFoldouts[i])
            {
                SerializedProperty sp = serializedObject.FindProperty("data").FindPropertyRelative("Datas");
                SerializedProperty prop = sp.GetArrayElementAtIndex(i);

                foreach (var child in GetChildren(prop))
                {
                    EditorGUILayout.PropertyField(child, true);
                }
            }

            GUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndScrollView();

        GUILayout.BeginVertical();

        if (GUILayout.Button("Save Data", GUILayout.Height(25)))
        {
            data.save();
        }

        if (GUILayout.Button("Reload Data", GUILayout.Height(25)))
        {
            data.load();
        }

        if (GUILayout.Button("Clear Data", GUILayout.Height(25)))
        {
            PlayerPrefs.DeleteAll();
        }

        GUILayout.EndVertical();
    }

    public static IEnumerable<SerializedProperty> GetChildren(SerializedProperty serializedProperty)
    {
        SerializedProperty currentProperty = serializedProperty.Copy();
        SerializedProperty nextSiblingProperty = serializedProperty.Copy();
        {
            nextSiblingProperty.Next(false);
        }

        if (currentProperty.Next(true))
        {
            do
            {
                if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                    break;

                yield return currentProperty;
            }
            while (currentProperty.Next(false));
        }
    }
}
