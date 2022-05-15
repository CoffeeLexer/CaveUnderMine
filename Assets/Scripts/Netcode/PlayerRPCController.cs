using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerRPCController : NetworkBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    
    public void SwitchAnimation(string parameter, object val)
    {
        switch (val)
        {
            case null:
                SetTriggerClientRpc(parameter);
                break;
            case float f:
                SetFloatClientRpc(parameter, f);
                break;
            case bool b:
                SetBoolClientRpc(parameter, b);
                break;
            case int i:
                SetIntegerClientRpc(parameter, i);
                break;
        }
    }

    [ClientRpc]
    private void SetTriggerClientRpc(String parameter) => animator.SetTrigger(parameter);
    [ClientRpc]
    private void SetBoolClientRpc(String parameter, bool val) => animator.SetBool(parameter, val);
    [ClientRpc]
    private void SetFloatClientRpc(String parameter, float val) => animator.SetFloat(parameter, val);
    [ClientRpc]
    private void SetIntegerClientRpc(String parameter, int val) => animator.SetInteger(parameter, val);
    
    
}
