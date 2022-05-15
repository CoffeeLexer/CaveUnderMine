using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeOnCollisionExit : Trigger
{
    [SerializeField] private string acceptableTag;
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == acceptableTag) Activate();
    }
}
