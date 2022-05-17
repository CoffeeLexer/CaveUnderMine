using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeOnCollisionStay : Trigger
{
    [SerializeField] private string acceptableTag;
    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == acceptableTag) Activate();
    }
}
