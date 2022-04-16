using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;
using DungeonScripts;
using Unity.VisualScripting;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject DefaultGround;
    [SerializeField] private GameObject DefaultWall;
    [SerializeField] private GameObject DefaultColumn;
    [SerializeField] private GameObject DefaultCeiling;
    [SerializeField] private GameObject DefaultLamp;
    [SerializeField] private float LampHeight;
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private float TileWidth;
    [SerializeField] private int Seed;
    [SerializeField] public List<Room> rooms;
    [SerializeField] public bool test;
    private Layout layout;
    private void Awake()
    {
        layout = new Layout(Width, Height);
        layout.BuildBlank();
        layout.InitTiles(DefaultGround, TileWidth);
        layout.InitCeilings(DefaultCeiling, TileWidth);
        layout.InitSides(DefaultWall, TileWidth);
        layout.InitCorners(DefaultColumn, TileWidth);
        layout.BuildRooms(rooms, TileWidth);
        layout.GenerateCorridor(new Random(Seed));
        layout.RoomErrorFixEntry();
        layout.ClearWallsFull(rooms);
        layout.ClearColumnParts();
        layout.InitLamps(DefaultLamp, LampHeight);
    }
}
