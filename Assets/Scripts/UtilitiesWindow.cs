using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DungeonScripts;
using TMPro;
using Random = System.Random;

public class UtilitiesWindow : EditorWindow
{
    private GameObject _parent;
    
    [MenuItem("Supreme/Dungeon Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MyWindow));
    }
    
    void OnGUI()
    {
        GUILayout.Label("Object", EditorStyles.largeLabel);
        _parent = EditorGUILayout.ObjectField("Parent", _parent, typeof(GameObject)) as GameObject;

        if (GUILayout.Button("Delete Inactive Children"))
        {

        }
    }
}
