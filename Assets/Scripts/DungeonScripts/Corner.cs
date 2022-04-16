using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;

namespace DungeonScripts
{
    public class Corner
    {
        public Vector2 unitPosition;
        public int index;
        public Layout layout;
        public GameObject[] gameObject;
        public Corner(int Index = -1)
        {
            index = Index;
        }
        // Direction: Hint numpad
        public Tile GetTile(int direction)
        {
            switch (direction)
            {
                case 7:
                {
                    // Most Left or Top side (cant get tile)
                    if (index < layout.width + 1 ||
                        index % (layout.width + 1) == 0) return null;
                    int x = index % (layout.width + 1);
                    int y = index / (layout.width + 1);
                    return layout.tiles[x - 1, y - 1];
                }
                case 9:
                {
                    // Most Right or Top side (cant get tile)
                    if (index < layout.width + 1 ||
                        index % (layout.width + 1) == layout.width) return null;
                    int x = index % (layout.width + 1);
                    int y = index / (layout.width + 1);
                    return layout.tiles[x, y - 1];
                }
                case 1:
                {
                    // Most Left or Bottom side (cant get tile)
                    if (index % (layout.width + 1) == 0 ||
                        index < (layout.width + 1) * layout.height) return null;
                    int x = index % (layout.width + 1);
                    int y = index / (layout.width + 1);
                    return layout.tiles[x - 1, y];
                }
                case 3:
                {
                    // Most Right or Bottom side (cant get tile)
                    if (index % (layout.width + 1) == layout.width ||
                        index > (layout.width + 1) * layout.height) return null;
                    int x = index % (layout.width + 1);
                    int y = index / (layout.width + 1);
                    return layout.tiles[x, y];
                }
                default:
                    return null;
            }
        }

        public void Init(GameObject objectBase, float unitSize, GameObject parent)
        {
            gameObject = new GameObject[4];
            for (int i = 0; i < 4; i++)
            {
                gameObject[i] = GameObject.Instantiate(
                    objectBase,
                    new Vector3(
                        -unitPosition.x * unitSize,
                        0,
                        unitPosition.y * unitSize),
                    objectBase.transform.rotation);
                gameObject[i].transform.SetParent(parent.transform);
                gameObject[i].transform.Rotate(0, 0, 90.0f * i);
                if (i % 2 == 0)
                {
                    gameObject[i].transform.localScale.Set(-1, 1, -1);
                }
            }
        }
    }
}