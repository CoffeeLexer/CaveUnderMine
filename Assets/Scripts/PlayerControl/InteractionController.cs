using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference interactActionRef;

    [SerializeField] 
    private InteractableController interactableController;

    public static event Action<DroppedItem> InteractEvent;
    private InputAction interactAction
    {
        get
        {
            var action = interactActionRef.action;
            if(!action.enabled) action.Enable();
            return action;
        }
    }

    private void OnEnable()
    {
        interactAction.performed += OnInteract;
    }
    
    private void OnDisable()
    {
        interactAction.performed -= OnInteract;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        var selected = interactableController.selected;
        if (selected != null)
        {
            InteractEvent?.Invoke(interactableController.GetSelected());
            Destroy(selected.gameObject);
        }
    }
}
