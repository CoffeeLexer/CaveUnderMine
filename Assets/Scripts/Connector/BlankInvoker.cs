using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;

public class BlankInvoker : MonoBehaviour
{
    [SerializeField] private bool repetitive;
    [SerializeField] private UnityEvent m_event;
    private void Awake()
    {
        m_event.AddListener(Stuff);
        m_event.SetPersistentListenerState(2, UnityEventCallState.RuntimeOnly);
        Debug.Log($"Count: {m_event.GetPersistentEventCount()}");
        for (int i = 0; i < m_event.GetPersistentEventCount(); i++)
        {
            
        }
    }

    private void Stuff()
    {
        Debug.Log("Stuff");
    }
}
