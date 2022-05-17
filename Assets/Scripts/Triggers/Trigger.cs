using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool invokeOnce = false;
    [SerializeField] private bool destroyOnInvoke = false;
    [SerializeField] private UnityEvent methods = new UnityEvent();

    public void Activate()
    {
        methods.Invoke();
        if(destroyOnInvoke) Destroy(gameObject);
        else if(invokeOnce) Destroy(this);
    }
}
