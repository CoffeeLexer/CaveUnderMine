using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace DungeonScripts
{
    public class Layout
    {
        public int width;
        public int height;
        public Tile[,] tiles;
        public Side[] rawSides;
        public Corner[] rawCorners;
        public Random rng;
        public GameObject tileParent;
        public GameObject ceilingParent;
        public GameObject sideParent;
        public GameObject cornerParent;
        public GameObject lampParent;
        public bool[,] roomLayout;
        
        public Layout(int Width, int Height)
        {
            width = Width;
            height = Height;
            tiles = new Tile[width, height];
            rawSides = new Side[width * height * 4];
            rawCorners = new Corner[(width + 1) * (height + 1)];
        }

        public void RoomErrorFixEntry()
        {
            bool active = true;
            while (active)
            {
                bool[,] flags = new bool[width, height];
                FlowMatrix(flags);
                bool needFix = HasFalse(flags);
                active = needFix ? true : false;
                RoomFix(flags);
            }
        }

        public void RoomFix(bool[,] flags)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (flags[x, y] != flags[x + 1, y] &&
                        !roomLayout[x, y] &&
                        !roomLayout[x + 1, y])
                    {
                        tiles[x, y].DestroyWall(6);
                        return;
                    }
                }
            }
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (flags[x, y] != flags[x, y + 1] &&
                        !roomLayout[x, y] &&
                        !roomLayout[x, y + 1])
                    {
                        tiles[x, y].DestroyWall(2);
                        return;
                    }
                }
            }
        }
        public bool[,] FlowMatrix(bool[,] matrix)
        {
            Queue<Vector2Int> positions = new Queue<Vector2Int>();
            positions.Enqueue(new Vector2Int(0, 0));
            while (positions.Count > 0)
            {
                var p = positions.Dequeue();
                matrix[p.x, p.y] = true;

                if(p.x > 0)
                    if(!tiles[p.x, p.y].GetSide(4).gameObject.activeSelf)
                        if(!matrix[p.x - 1, p.y])
                            positions.Enqueue(new Vector2Int(p.x - 1, p.y));
                if(p.x + 1 < width)
                    if(!tiles[p.x, p.y].GetSide(6).gameObject.activeSelf)
                        if(!matrix[p.x + 1, p.y])
                            positions.Enqueue(new Vector2Int(p.x + 1, p.y));
                if(p.y > 0)
                    if(!tiles[p.x, p.y].GetSide(8).gameObject.activeSelf)
                        if(!matrix[p.x, p.y - 1])
                            positions.Enqueue(new Vector2Int(p.x, p.y - 1));
                if(p.y + 1 < height)
                    if(!tiles[p.x, p.y].GetSide(2).gameObject.activeSelf)
                        if(!matrix[p.x, p.y + 1])
                            positions.Enqueue(new Vector2Int(p.x, p.y + 1));
            }

            return matrix;
        }

        public void SetRoomLayoutEmpty()
        {
            roomLayout = new bool[width, height];
        }

        public bool HasFalse(bool[,] matrix)
        {
            foreach (var one in matrix)
            {
                if (!one) return true;
            }
            return false;
        }
        public void GenerateCorridor(Random rng)
        {
            int[,] direction = new int[width, height];
            bool[,] flags = roomLayout.Clone() as bool[,];
            flags[0, 0] = true;
            int RandomDirection(Random r) => r.Next(1, 5) * 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (flags[x, y] == true) continue;
                    Vector2Int start = new Vector2Int(x, y);
                    Vector2Int current = new Vector2Int(x, y);
                    while (!flags[current.x, current.y])
                    {
                        switch (RandomDirection(rng))
                        {
                            case 8:
                            {
                                // The most top tile
                                if (current.y == 0) continue;
                                // Tile is part of room
                                if (roomLayout[current.x, current.y - 1])
                                    // Side CAN NOT be destroyed (it is part of room)
                                    if (tiles[current.x, current.y].GetSide(8).gameObject.activeSelf)
                                        continue;
                                direction[current.x, current.y] = 8;
                                current.y--;
                                continue;
                            }
                            case 4:
                            {
                                // The most left tile
                                if (current.x == 0) continue;
                                // Tile is part of room
                                if(roomLayout[current.x - 1, current.y])
                                    // Side CAN NOT be destroyed (it is part of room)
                                    if(tiles[current.x, current.y].GetSide(4).gameObject.activeSelf)
                                        continue;
                                direction[current.x, current.y] = 4;
                                current.x--;
                                continue;
                            }
                            case 6:
                            {
                                // The most right tile
                                if (current.x + 1 == width) continue;
                                // Tile is part of room
                                if(roomLayout[current.x + 1, current.y])
                                    // Side CAN NOT be destroyed (it is part of room)
                                    if (tiles[current.x, current.y].GetSide(6).gameObject.activeSelf)
                                        continue;
                                direction[current.x, current.y] = 6;
                                current.x++;
                                continue;
                            }
                            case 2:
                            {
                                // The most bottom tile
                                if (current.y + 1 == height) continue;
                                // Tile is part of room
                                if(roomLayout[current.x, current.y + 1])
                                    // Side CAN NOT be destroyed (it is part of room)
                                    if(tiles[current.x, current.y].GetSide(2).gameObject.activeSelf)
                                        continue;
                                direction[current.x, current.y] = 2;
                                current.y++;
                                continue;
                            }
                        }
                    }
                    current.x = start.x;
                    current.y = start.y;
                    while (!flags[current.x, current.y])
                    {
                        int d = direction[current.x, current.y];
                        tiles[current.x, current.y].DestroyWall(d);
                        flags[current.x, current.y] = true;
                        switch (d)
                        {
                            case 8:
                            {
                                current.y--;
                                continue;
                            }
                            case 4:
                            {
                                current.x--;
                                continue;
                            }
                            case 6:
                            {
                                current.x++;
                                continue;
                            }
                            case 2:
                            {
                                current.y++;
                                continue;
                            }
                        }
                    }
                }
            }
        }
        public void BuildRooms(List<Room> rooms, float tileSize)
        {
            ValidateRooms(rooms);
            ClearGround(rooms);
            ClearWalls(rooms);
            ClearColumns(rooms);
            ClearEntrances(rooms);
            InitPrefabs(rooms, tileSize);
        }

        public void InitPrefabs(List<Room> rooms, float tileSize)
        {
            foreach (var room in rooms)
            {
                GameObject.Instantiate(
                    room.prefab,
                    new Vector3(-room.x * tileSize, 0, room.y * tileSize),
                    room.prefab.transform.rotation);
            }
        }
        public void ClearEntrances(List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                foreach (int entrance in room.entrances)
                {
                    int x = room.x;
                    int y = room.y;
                    int direction = 8;
                    int index = entrance;
                    while (index > 0)
                    {
                        index--;
                        switch (direction)
                        {
                            case 8:
                            {
                                if (x + 1 < room.x + room.width)
                                {
                                    x++;
                                }
                                else
                                {
                                    direction = 6;
                                }
                                continue;
                            }
                            case 6:
                            {
                                if (y + 1 < room.y + room.height)
                                {
                                    y++;
                                }
                                else
                                {
                                    direction = 2;
                                }
                                continue;
                            }
                            case 2:
                            {
                                if (x - 1 >= room.x)
                                {
                                    x--;
                                }
                                else
                                {
                                    direction = 4;
                                }
                                continue;
                            }
                            case 4:
                            {
                                if (y - 1 >= room.y)
                                {
                                    y--;
                                }
                                else
                                {
                                    direction = 8;
                                }
                                continue;
                            }
                        }
                    }
                    Tile t = tiles[x, y].GetAdjacent(direction);
                    if (t == null)
                        throw new Exception(
                            $"Room_{room.x}_{room.y}: Entrance ({entrance}) is dungeons edge (Destroying wall will result access to out of bounds)");
                    t.DestroyWall(10 - direction);
                }
            }
        }
        public void ClearGround(List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                for (int y = room.y; y < room.y + room.height; y++)
                {
                    for (int x = room.x; x < room.x + room.width; x++)
                    {
                        tiles[x, y].gameObjectGround.SetActive(false);
                    }
                }
            }
        }
        public void ClearColumns(List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                for (int y = room.y; y < room.y + room.height - 1; y++)
                {
                    for (int x = room.x; x < room.x + room.width - 1; x++)
                    {
                        tiles[x, y].GetCorner(3).gameObject[0].SetActive(false);
                        tiles[x, y].GetCorner(3).gameObject[1].SetActive(false);
                        tiles[x, y].GetCorner(3).gameObject[2].SetActive(false);
                        tiles[x, y].GetCorner(3).gameObject[3].SetActive(false);
                    }
                }
            }
        }
        public void ClearWallsFull(List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                for (int y = room.y; y < room.y + room.height; y++)
                {
                    for (int x = room.x; x < room.x + room.width; x++)
                    {
                        tiles[x, y].GetSide(8).gameObject.SetActive(false);
                        tiles[x, y].GetSide(4).gameObject.SetActive(false);
                        tiles[x, y].GetSide(6).gameObject.SetActive(false);
                        tiles[x, y].GetSide(2).gameObject.SetActive(false);
                    }
                }
            }
        }
        public void ClearWalls(List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                for (int y = room.y; y < room.y + room.height; y++)
                {
                    for (int x = room.x; x < room.x + room.width; x++)
                    {
                        if(y > room.y)
                        tiles[x, y].GetSide(8).gameObject.SetActive(false);
                        if(x > room.x)
                        tiles[x, y].GetSide(4).gameObject.SetActive(false);
                        if(x < room.x + room.width - 1)
                        tiles[x, y].GetSide(6).gameObject.SetActive(false);
                        if(y < room.y + room.height - 1)
                        tiles[x, y].GetSide(2).gameObject.SetActive(false);
                    }
                }
            }
        }
        public void ValidateRooms(List<Room> rooms)
        {
            roomLayout = new bool[width, height];
            foreach (Room room in rooms)
            {
                for (int y = room.y; y < room.y + room.height; y++)
                {
                    for (int x = room.x; x < room.x + room.width; x++)
                    {
                        if (x < 0 || y < 0 || x >= width || y >= height)
                            throw new Exception($"Room {room.x} {room.y} goes out of bounds");
                        if (roomLayout[x, y]) 
                            throw new Exception($"Room {room.x} {room.y} overlaps with other room");
                        roomLayout[x, y] = true;
                    }
                }
            }
        }
        public void BuildBlank()
        {
            BuildTiles();
            BuildSides();
            BuildCorners();
        }

        public void InitTiles(GameObject tileObject, float unitSize)
        {
            tileParent = new GameObject("Tiles");
            foreach (var i in tiles)
            {
                i.Init(tileObject, unitSize, tileParent);
            }
        }
        public void InitCeilings(GameObject ceilingObject, float unitSize)
        {
            ceilingParent = new GameObject("Ceilings");
            foreach (var i in tiles)
            {
                i.InitCeiling(ceilingObject, unitSize, ceilingParent);
            }
        }

        public void InitSides(GameObject sideObject, float unitSize)
        {
            sideParent = new GameObject("Walls");
            foreach (var i in rawSides)
            {
                i.Init(sideObject, unitSize, sideParent);
            }
        }

        public void ClearColumnParts()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (!tiles[x, y].GetSide(6).gameObject.activeSelf)
                    {
                        // Horizontal Up
                        if (tiles[x, y].GetSide(8).gameObject.activeSelf &&
                            tiles[x + 1, y].GetSide(8).gameObject.activeSelf)
                        {
                            tiles[x, y].GetCorner(9).gameObject[0].SetActive(false);
                            tiles[x, y].GetCorner(9).gameObject[1].SetActive(false);
                        }

                        // Horizontal Down
                        if (tiles[x, y].GetSide(2).gameObject.activeSelf &&
                            tiles[x + 1, y].GetSide(2).gameObject.activeSelf)
                        {
                            {
                                tiles[x, y].GetCorner(3).gameObject[2].SetActive(false);
                                tiles[x, y].GetCorner(3).gameObject[3].SetActive(false);
                            }
                        }
                    }
                }
            }
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!tiles[x, y].GetSide(2).gameObject.activeSelf)
                    {
                        // Vertical Left
                        if (tiles[x, y].GetSide(4).gameObject.activeSelf &&
                            tiles[x, y + 1].GetSide(4).gameObject.activeSelf)
                        {
                            tiles[x, y].GetCorner(1).gameObject[0].SetActive(false);
                            tiles[x, y].GetCorner(1).gameObject[3].SetActive(false);
                        }

                        // Vertical Right
                        if (tiles[x, y].GetSide(6).gameObject.activeSelf &&
                            tiles[x, y + 1].GetSide(6).gameObject.activeSelf)
                        {
                            {
                                tiles[x, y].GetCorner(3).gameObject[1].SetActive(false);
                                tiles[x, y].GetCorner(3).gameObject[2].SetActive(false);
                            }
                        }
                    }
                }
            }
        }

        public void SetRootParent()
        {
            GameObject go = new GameObject("Layout");
            ceilingParent.transform.SetParent(go.transform);
            cornerParent.transform.SetParent(go.transform);
            lampParent.transform.SetParent(go.transform);
            sideParent.transform.SetParent(go.transform);
            tileParent.transform.SetParent(go.transform);
        }
        public void InitLamps(GameObject lampObject, float lampHeight)
        {
            lampParent = new GameObject("Lamps");
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if (!tiles[x, y].GetSide(6).gameObject.activeSelf)
                    {
                        // Horizontal Up
                        if (tiles[x, y].GetSide(8).gameObject.activeSelf &&
                            tiles[x + 1, y].GetSide(8).gameObject.activeSelf)
                        {
                            GameObject obj = GameObject.Instantiate(
                                lampObject,
                                tiles[x, y].GetCorner(9).gameObject[0].transform.position,
                                Quaternion.Euler(0, 0, 0));
                                obj.transform.SetParent(lampParent.transform);
                        }

                        // Horizontal Down
                        if (tiles[x, y].GetSide(2).gameObject.activeSelf &&
                            tiles[x + 1, y].GetSide(2).gameObject.activeSelf)
                        {
                            {
                                GameObject obj = GameObject.Instantiate(
                                    lampObject,
                                    tiles[x, y].GetCorner(3).gameObject[3].transform.position,
                                    Quaternion.Euler(0, 180, 0));
                                obj.transform.SetParent(lampParent.transform);
                            }
                        }
                    }
                }
            }
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!tiles[x, y].GetSide(2).gameObject.activeSelf)
                    {
                        // Vertical Left
                        if (tiles[x, y].GetSide(4).gameObject.activeSelf &&
                            tiles[x, y + 1].GetSide(4).gameObject.activeSelf)
                        {
                            GameObject obj = GameObject.Instantiate(
                                lampObject,
                                tiles[x, y].GetCorner(1).gameObject[0].transform.position,
                                Quaternion.Euler(0, 270, 0));
                                obj.transform.SetParent(lampParent.transform);
                        }

                        // Vertical Right
                        if (tiles[x, y].GetSide(6).gameObject.activeSelf &&
                            tiles[x, y + 1].GetSide(6).gameObject.activeSelf)
                        {
                            {
                                GameObject obj = GameObject.Instantiate(
                                    lampObject,
                                    tiles[x, y].GetCorner(3).gameObject[0].transform.position,
                                    Quaternion.Euler(0, 90, 0));
                                obj.transform.SetParent(lampParent.transform);
                            }
                        }
                    }
                }
            }
            lampParent.transform.position = new Vector3(0, lampHeight, 0);
        }

        public void InitCorners(GameObject cornerObject, float unitSize)
        {
            cornerParent = new GameObject("Corners");
            foreach (var i in rawCorners)
            {
                i.Init(cornerObject, unitSize, cornerParent);
            }
        }

        private void BuildTiles()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, y] = new Tile(new Vector2Int(x, y));
                    tiles[x, y].layout = this;
                }
            }
        }

        private void BuildSides()
        {
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rawSides[index] = new Side(index);
                    rawSides[index].unitPosition = new Vector2(x + 0.5f, y);
                    rawSides[index].rotation = 0.0f;
                    index++;
                    rawSides[index] = new Side(index);
                    rawSides[index].unitPosition = new Vector2(x, y + 0.5f);
                    rawSides[index].rotation = 270.0f;
                    index++;
                    rawSides[index] = new Side(index);
                    rawSides[index].unitPosition = new Vector2(x + 1f, y + 0.5f);
                    rawSides[index].rotation = 90.0f;
                    index++;
                    rawSides[index] = new Side(index);
                    rawSides[index].unitPosition = new Vector2(x + 0.5f, y + 1f);
                    rawSides[index].rotation = 180.0f;
                    index++;
                }
            }
        }
        private void BuildCorners()
        {
            int index = 0;
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    rawCorners[index] = new Corner(index);
                    rawCorners[index].unitPosition = new Vector2(x, y);
                    index++;
                }
            }
        }
    }
}