using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeOnTriggerEnter : Trigger
{
    [SerializeField] private string acceptableTag;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == acceptableTag) Activate();
    }
}
