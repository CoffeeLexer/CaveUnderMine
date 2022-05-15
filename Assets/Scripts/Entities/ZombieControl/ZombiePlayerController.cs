using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZombiePlayerController : NetworkBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private InputActionReference moveAcRef;
    [SerializeField] private float rotSpeed = 10f;

    [SerializeField] private float moveSpeed = 1f;

    private InputAction moveAc => FetchAction(moveAcRef);

    private InputAction FetchAction(InputActionReference reference)
    {
        var action = reference.action;
        if (!action.enabled) action.Enable();
        return action;
    }

    private AnimationController animationController;
    private CharacterController cc;

    private Vector2 inputAxis;

    private void Awake()
    {
        animationController = GetComponent<AnimationController>();
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (IsHost) return;
        moveAc.performed += OnMove;
        moveAc.canceled += OnMoveCancel;
    }

    private void OnDisable()
    {
        if (IsHost) return;
        moveAc.performed -= OnMove;
        moveAc.canceled -= OnMoveCancel;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
        animationController.SwitchAnimation("isWalking", true);
        SetAnimationParamServerRpc("isWalking", true);
    }

    
    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        inputAxis = Vector2.zero;
        animationController.SwitchAnimation("isWalking", false);
        SetAnimationParamServerRpc("isWalking", false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetAnimationParamServerRpc(string parameter, bool value) =>
        animationController.SwitchAnimation(parameter, value);

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePositionServerRpc(Vector2 inputAxis)
    {
        var positionMovement = inputAxis != Vector2.zero ? transform.forward * moveSpeed : Vector3.zero;
        positionMovement.y = 0;
        var newPos = cc.center + positionMovement;
        cc.SimpleMove(newPos);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateRotationServerRpc(Vector2 inputAxis, Vector3 camera)
    {
        var angle = -Vector2.SignedAngle(Vector2.up, inputAxis);
        var fixedInput = Quaternion.Euler(0, angle + camera.y, 0);
        var nextRotation = Quaternion.Lerp(transform.rotation, fixedInput, rotSpeed * Time.deltaTime);
        cc.transform.rotation = nextRotation;
    }

    private void Update()
    {
        // Debug.Log("Updating");
        UpdatePositionServerRpc(inputAxis);
        UpdateRotationServerRpc(inputAxis, camera.eulerAngles);
    }
}