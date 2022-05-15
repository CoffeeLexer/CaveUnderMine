using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeOnCollisionEnter : Trigger
{
    [SerializeField] private string acceptableTag;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == acceptableTag) Activate();
    }
}
