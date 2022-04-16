using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace DungeonScripts
{
    [Serializable]
    public class Room
    {
        [SerializeField] public int x;
        [SerializeField] public int y;
        [SerializeField] public int width;
        [SerializeField] public int height;
        [SerializeField] public GameObject prefab;
        
        [SerializeField] public List<int> entrances;
        // Entrance index hint:
        //       0 1 2
        //      +-+-+-+
        //     9|     |3
        //      +     +
        //     8|     |4
        //      +-+-+-+
        //       7 6 5
    }
}