using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InvokeOnTriggerStay : Trigger
{
    [SerializeField] private string acceptableTag;
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == acceptableTag) Activate();
    }
}
