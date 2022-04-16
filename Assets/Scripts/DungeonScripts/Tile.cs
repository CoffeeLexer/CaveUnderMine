using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

namespace DungeonScripts
{
    public class Tile
    {
        public Vector2Int index;
        public Layout layout;
        public GameObject gameObjectGround;
        public GameObject gameObjectCeiling;
        public Tile()
        {
            index = new Vector2Int(-1, -1);
        }
        public Tile(Vector2Int Index)
        {
            index = Index;
        }
        // Direction: Hint Numpad
        public Side GetSide(int direction)
        {
            int i = direction * 5 % 7 % 5;
            //  8 => 0
            //  4 => 1
            //  6 => 2
            //  2 => 3
            return layout.rawSides[(index.x + index.y * layout.width) * 4 + i];
        }

        public Tile GetAdjacent(int direction)
        {
            switch (direction)
            {
                case 8:
                {
                    if (index.y > 0)
                    {
                        return layout.tiles[index.x, index.y - 1];
                    }
                    return null;
                }
                case 4:
                {
                    if (index.x > 0)
                    {
                        return layout.tiles[index.x - 1, index.y];
                    }
                    return null;
                }
                case 6:
                {
                    if (index.x + 1 < layout.width)
                    {
                        return layout.tiles[index.x + 1, index.y];
                    }
                    return null;
                }
                case 2:
                {
                    if (index.y + 1 < layout.height)
                    {
                        return layout.tiles[index.x, index.y + 1];
                    }
                    return null;
                }
                default:
                    return null;
            }
        }
        public void DestroyWall(int direction)
        {
            GetSide(direction).gameObject.SetActive(false);
            GetAdjacent(direction).GetSide(10 - direction).gameObject.SetActive(false);
        }

        public Corner GetCorner(int direction)
        {
            int i = direction * 3 % 5 - 1;
            int h = i / 2;
            i = index.y * (layout.width + 1) + h * (layout.width + 1) + i % 2 + index.x;
            return layout.rawCorners[i];
        }

        public void Init(GameObject objectBase, float unitSize, GameObject parent)
        {
            gameObjectGround = GameObject.Instantiate(
                objectBase,
                new Vector3(
                    -index.x * unitSize - unitSize / 2,
                    0,
                    index.y * unitSize + unitSize / 2),
                objectBase.transform.rotation);
            gameObjectGround.name = $"Tile_{index.x}_{index.y}";
            gameObjectGround.transform.SetParent(parent.transform);
        }
        public void InitCeiling(GameObject objectBase, float unitSize, GameObject parent)
        {
            gameObjectGround = GameObject.Instantiate(
                objectBase,
                new Vector3(
                    -index.x * unitSize - unitSize / 2,
                    5,
                    index.y * unitSize + unitSize / 2),
                objectBase.transform.rotation);
            gameObjectGround.name = $"Tile_{index.x}_{index.y}";
            gameObjectGround.transform.SetParent(parent.transform);
        }
    }
}