using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroySpawn : MonoBehaviour
{
    [SerializeField] private GameObject spawnObject;
    private void OnDestroy()
    {
        Instantiate(spawnObject, gameObject.transform.position, Quaternion.identity);
    }
}
