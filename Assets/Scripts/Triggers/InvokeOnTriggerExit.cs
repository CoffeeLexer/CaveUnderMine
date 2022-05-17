using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeOnTriggerExit : Trigger
{
    [SerializeField] private string acceptableTag;
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == acceptableTag) Activate();
    }
}
