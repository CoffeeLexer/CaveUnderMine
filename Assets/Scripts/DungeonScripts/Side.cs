using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;

namespace DungeonScripts
{
    public class Side
    {
        public int index;
        public float rotation;
        public Vector2 unitPosition;
        public Layout layout;
        public GameObject gameObject;

        public bool isActive()
        {
            if (gameObject == null) return false;
            return gameObject.activeSelf;
        }
        public Side(int Index = -1)
        {
            index = Index;
        }

        public void Init(GameObject objectBase, float unitSize, GameObject parent)
        {
            gameObject = GameObject.Instantiate(
                objectBase,
                new Vector3(
                    -unitPosition.x * unitSize,
                    0,
                    unitPosition.y * unitSize),
                objectBase.transform.rotation);
            gameObject.name = $"Side_{rotation}";
            gameObject.transform.Rotate(0, 0, rotation);
            gameObject.transform.SetParent(parent.transform);
        }
    }
}