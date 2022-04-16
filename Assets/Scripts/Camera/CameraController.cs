using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference zoomInputRef;

    [SerializeField]
    private bool noMaxDistance = false;

    [SerializeField] 
    private float maxRadius = 20f;
    
    [SerializeField]
    private float minRadius = 2f;

    [SerializeField] 
    private float centerOffset = -2f;

    [SerializeField] 
    private float relativeBottomRadius = 0.7f; 

    [SerializeField] 
    private float zoomStep = 1.25f;

    private float exponent = 1f;
    private CinemachineFreeLook cinemachineFreeLook;

    private bool paused;

    private InputAction zoomInput
    {
        get
        {
            if(!zoomInputRef.action.enabled) zoomInputRef.action.Enable();
            return zoomInputRef.action;
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        cinemachineFreeLook.enabled = !cinemachineFreeLook.enabled;
    }

    private void OnEnable()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        zoomInput.performed += OnScroll;
        cinemachineFreeLook.m_Orbits[0].m_Radius = 0;
        cinemachineFreeLook.m_Orbits[2].m_Height = centerOffset;
        UpdateZoom(minRadius);
    }
    
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        zoomInput.performed -= OnScroll;
    }

    private void OnScroll(InputAction.CallbackContext ctx)
    {
        if (!paused)
        {
            var input = ctx.ReadValue<Vector2>().y;
            if (input == 0) return;
            var currRadius = cinemachineFreeLook.m_Orbits[1].m_Radius;
            if (input < 0)
            {
                if (noMaxDistance || currRadius < maxRadius)
                {
                    var farther = Mathf.Pow(zoomStep, ++exponent);
                    var nextRadius = noMaxDistance || farther <= maxRadius ? farther : maxRadius;
                    UpdateZoom(nextRadius);
                }
            }
            else
            {
                if (currRadius > minRadius)
                {
                    var closer = Mathf.Pow(zoomStep, --exponent);
                    var nextRadius = closer >= minRadius ? closer : minRadius;
                    UpdateZoom(nextRadius);
                }
            }
        }
    }

    private void UpdateZoom(float radius)
    {
        cinemachineFreeLook.m_Orbits[0].m_Height = radius * 2 + centerOffset;
        cinemachineFreeLook.m_Orbits[1].m_Radius = radius;
        cinemachineFreeLook.m_Orbits[1].m_Height = radius + centerOffset;
        cinemachineFreeLook.m_Orbits[2].m_Radius = radius * relativeBottomRadius;
    }
}
