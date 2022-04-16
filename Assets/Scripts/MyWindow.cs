using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DungeonScripts;
using TMPro;
using Random = System.Random;

public class MyWindow : EditorWindow
{
    private GameObject DefaultGround;
    private GameObject DefaultWall;
    private GameObject DefaultColumn;
    private GameObject DefaultCeiling;
    private GameObject DefaultLamp;
    private float LampHeight;
    private int Width;
    private int Height;
    private float TileWidth;
    private int Seed;
    
    [MenuItem("Supreme/Dungeon Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MyWindow));
    }
    
    void OnGUI()
    {
        GUILayout.Label("Prefabs", EditorStyles.largeLabel);
        DefaultGround = EditorGUILayout.ObjectField("Ground", DefaultGround, typeof(GameObject)) as GameObject;
        DefaultWall = EditorGUILayout.ObjectField("Wall", DefaultWall, typeof(GameObject)) as GameObject;
        DefaultColumn = EditorGUILayout.ObjectField("Column", DefaultColumn, typeof(GameObject)) as GameObject;
        DefaultCeiling = EditorGUILayout.ObjectField("Ceiling", DefaultCeiling, typeof(GameObject)) as GameObject;
        DefaultLamp = EditorGUILayout.ObjectField("Lamp", DefaultLamp, typeof(GameObject)) as GameObject;
        GUILayout.Label("Generator Settings", EditorStyles.largeLabel);
        LampHeight = EditorGUILayout.FloatField("Lamp Height", LampHeight);
        Width = EditorGUILayout.IntField("Dungeon Width", Width);
        Height = EditorGUILayout.IntField("Dungeon Height", Height);
        TileWidth = EditorGUILayout.FloatField("Tile Width", TileWidth);
        Seed = EditorGUILayout.IntField("Seed", Seed);
        if (GUILayout.Button("Generate"))
        {
            Debug.Log("Generating");
            Layout layout = new Layout(Width, Height);
            layout.BuildBlank();
            layout.SetRoomLayoutEmpty();
            layout.InitTiles(DefaultGround, TileWidth);
            layout.InitCeilings(DefaultCeiling, TileWidth);
            layout.InitSides(DefaultWall, TileWidth);
            layout.InitCorners(DefaultColumn, TileWidth);
            layout.GenerateCorridor(new Random(Seed));
            layout.ClearColumnParts();
            layout.InitLamps(DefaultLamp, LampHeight);
            layout.SetRootParent();
        }
    }
}
