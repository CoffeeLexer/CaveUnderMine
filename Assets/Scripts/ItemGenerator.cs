using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;
[Serializable]
public class AwakeGenStruct
{
    [SerializeField] public GameObject item;
    [SerializeField] public int amount;
}

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tileLayout;
    [SerializeField] private List<AwakeGenStruct> startGenList;
    private static Random rng = new Random();

    private void Start()
    {
        foreach (var item in startGenList)
        {
            for(int i = 0; i < item.amount; i++) Generate(item.item);
        }
    }
    public void Generate(GameObject obj)
    {
        Offset offset;
        Vector3 vec3offset;
        if (obj.TryGetComponent<Offset>(out offset)) vec3offset = offset.number;
        else vec3offset = Vector3.zero;
        int available = AvailableCount();
        if (available == 0) return;
        int insertIndex = rng.Next(0, available);
        int count = 0;
        foreach (Transform i in tileLayout.transform)
        {
            if (i.childCount == 0)
            {
                if (count == insertIndex)
                {
                    GameObject go = Instantiate(obj, Vector3.up, Quaternion.identity, i);
                    go.transform.localPosition = vec3offset;
                    return;
                }
                count++;
            }
        }
    }

    private int AvailableCount()
    {
        int count = 0;
        foreach (Transform i in tileLayout.transform)
        {
            if (i.childCount == 0) count++;
        }
        return count;
    }
}
