using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    void OnEnable()
    {
        InteractionController.InteractEvent += OnItemGet;
    }

    private void OnDisable()
    {
        InteractionController.InteractEvent -= OnItemGet;
    }

    private void OnItemGet(DroppedItem item)
    {
        Debug.Log("I've got an item");
    }
}
