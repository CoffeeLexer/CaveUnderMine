using System;
using System.Collections;
using System.Collections.Generic;
using Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;
[Serializable]
public class AwakeGenStruct
{
    
    [SerializeField] public GameObject item;
    [SerializeField] public int amount;
    [SerializeField] public bool isItem = true;
}

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private GoldTracker goldTracker;
    [SerializeField] private GameObject tileLayout;
    [SerializeField] private List<AwakeGenStruct> startGenList;
    private static Random rng = new Random();

    public void Generate()
    {
        foreach (var item in startGenList)
        {
            for(int i = 0; i < item.amount; i++) Generate(item);
        }
    }
    private void Generate(AwakeGenStruct obj)
    {
        Offset offset;
        Vector3 vec3offset;
        if (obj.item.TryGetComponent<Offset>(out offset)) vec3offset = offset.number;
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
                    GameObject go = obj.isItem ? Instantiate(obj.item, Vector3.up, Quaternion.identity, i) : Instantiate(obj.item, i.position + Vector3.up, Quaternion.identity, i);
                    if (go.TryGetComponent<NudgeTracker>(out var coin))
                    {
                        coin.tracker = goldTracker;
                    }
                    if(go.TryGetComponent<NetworkObjectSpawner>(out var spawner)) spawner.Spawn();
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
