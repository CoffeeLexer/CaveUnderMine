using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField]
    private Material toggleMaterial;
    private List<Renderer> renderers;
    private List<Material> materials;
    private bool isMarked;
    public int count = 1;
    private void Awake()
    {
        renderers = new List<Renderer>();
        ExtractRecursion(this.gameObject);
        materials = new List<Material>();
        foreach (var one in renderers)
        {
            materials.Add(one.material);
        }
        isMarked = false;
    }

    public void Mark()
    {
        foreach (var one in renderers)
        {
            one.material = toggleMaterial;
        }
        Debug.Log("Mark");
    }

    public void UnMark()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = materials[i];
        }
        Debug.Log("UnMark");
    }
    private void ExtractRecursion(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        if(renderer) renderers.Add(renderer);
        foreach (Transform child in obj.transform)
        {
            ExtractRecursion(child.gameObject);
        }
    }
}
