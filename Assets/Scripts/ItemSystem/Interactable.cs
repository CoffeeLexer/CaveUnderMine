using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] public string lookText;
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private bool destroyOnInvoke;
    private Amount amount;
    private bool hasAmount;
    private void Awake()
    {
        hasAmount = TryGetComponent<Amount>(out amount);
    }

    public void Invoke()
    {
        onInteract.Invoke();
        if(destroyOnInvoke) Destroy(gameObject);
    }

    public string GetLookText()
    {
        if (hasAmount)
        {
            return string.Concat(lookText, string.Concat(" (", amount.number, ")"));
        }
        else
        {
            return lookText;
        }
    }
}
