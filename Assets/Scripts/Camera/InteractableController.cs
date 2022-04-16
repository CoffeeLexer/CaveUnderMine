using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [SerializeField]
    private float maxInterractionDistance = 5f;

    [SerializeField] 
    private Material highlightMaterial;

    [SerializeField]
    private CinemachineFreeLook cinemachineFreeLook;

    private Transform target;
    public Transform selected { get; private set; }
    private Camera cam;
    private Vector3 center;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        center = new Vector3((float)cam.pixelWidth / 2, (float)cam.pixelHeight / 2, 0);
        target = cinemachineFreeLook.m_LookAt;
    }

    private void Update()
    {

        var ray = cam.ScreenPointToRay(center);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.cyan);
        if (Physics.Raycast(ray, out hit, maxInterractionDistance + Vector3.Distance(transform.position, target.position)))
        {
            var something = hit.transform.GetComponent<DroppedItem>();
            if (something != null)
            {
                selected = hit.transform;
                hit.transform.GetComponent<DroppedItem>().Mark();
            }
            else
            {
                ResetMaterial();
            }
        }
        else
        {
            ResetMaterial();
        }
    }

    private void ResetMaterial()
    {
        if (selected == null) return;
        selected.transform.GetComponent<DroppedItem>().UnMark();
        selected = null;
    }

    public DroppedItem GetSelected()
    {
        return selected?.transform.GetComponent<DroppedItem>();
    }
}
