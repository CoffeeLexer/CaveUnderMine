using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionCamera : MonoBehaviour
{
    private UnityEngine.Camera rayCamera;
    public UnityEngine.Camera UICamera;
    public RectTransform crosshair;
    public Text text;

    private Interactable interaction;
    private RaycastHit hit;
    public Ray ray;

    [SerializeField] private Vector3 rayPosition;

    private void OnEnable()
    {
        rayCamera = GetComponent<UnityEngine.Camera>();
        var v = UICamera.WorldToScreenPoint(crosshair.position);
        rayPosition = new Vector3(v.x, v.y, 0.0f);
    }

    private void Update()
    {
        ray = rayCamera.ScreenPointToRay(rayPosition);
        if (Physics.Raycast(ray, out hit))
        {
            Transform obj = hit.transform;
            if (obj.TryGetComponent<Interactable>(out interaction))
            {
                text.text = interaction.GetLookText();
            }
            else
            {
                text.text = "";
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (interaction) interaction.Invoke();
    }
}