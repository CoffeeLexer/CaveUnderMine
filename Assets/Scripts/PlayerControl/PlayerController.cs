using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using PlayerControl;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private Transform camera;
    
    [SerializeField] 
    private InputActionReference moveAcRef;
    [SerializeField] 
    private InputActionReference sprintAcRef;
    [SerializeField]
    private InputActionReference attackAcRef;

    [SerializeField] private InputActionReference pauseAcRef;

    [Min(0f)]
    [SerializeField] 
    private float movSpeed = 1f;
    [Min(0f)] [SerializeField] 
    private float sprintSpeed = 2f;
    [Min(0f)]
    [SerializeField]
    private float rotSpeed = 10f;

    [Min(1f)] 
    [SerializeField] 
    private float animationSpeedMultiplier = 1.5f;

    private bool movementBlock = false;
    
    private CharacterController cc;
    private Animator playerAnimator;
    public Vector2 inputAxis;
    private bool isSprinting = false;
    private bool paused = false;
    
    [SerializeField]
    public UnityEvent attackEvent;
    [SerializeField]
    public UnityEvent pauseEvent;
    [SerializeField]
    public UnityEvent unpauseEvent;

    private InputAction movAc => FetchAction(moveAcRef);
    private InputAction sprintAc => FetchAction(sprintAcRef);
    private InputAction attackAc => FetchAction(attackAcRef);
    private InputAction pauseAc => FetchAction(pauseAcRef);

    private InputAction FetchAction(InputActionReference reference)
    {
        var action = reference.action;
        if(!action.enabled) action.Enable();
        return action;
    }

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        var moveSet = GetComponent<Moveset>();
        if(moveSet != null) moveSet.attacksDone.AddListener(OnAttackEnd);
        playerAnimator.SetFloat("runSpeed", movSpeed * animationSpeedMultiplier);
        playerAnimator.SetFloat("sprintSpeed", sprintSpeed * animationSpeedMultiplier);
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        paused = !paused;
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            pauseEvent.Invoke();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            unpauseEvent.Invoke();
        }
    }

    private void OnAttackEnd()
    {
        movementBlock = false;
    }

    private void OnEnable()
    {
        pauseAc.performed += OnPause;
        movAc.performed += OnMove;
        movAc.canceled += OnMoveCanceled;
        sprintAc.performed += OnSprint;
        sprintAc.canceled += OnSprintCanceled;
        
        attackAc.performed += OnAttack;
    }
    
    private void OnDisable()
    {
        pauseAc.performed -= OnPause;
        movAc.performed -= OnMove;
        movAc.canceled -= OnMoveCanceled;
        sprintAc.performed -= OnSprint;
        sprintAc.canceled -= OnSprintCanceled;
        
        attackAc.performed -= OnAttack;
        
        playerAnimator.fireEvents = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        if (!paused) SprintCancel();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        if (!paused) MoveCancel();
    }

    private void OnSprint(InputAction.CallbackContext ctx)
    {
        if (!paused) Sprint();
    }


    private void Sprint()
    {
        if (playerAnimator.GetBool("isWalking"))
        {
            playerAnimator.SetBool("isSprinting", true);
            isSprinting = true;
        }
    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        if (!paused)
        {
            inputAxis = ctx.ReadValue<Vector2>();
            playerAnimator.SetBool("isWalking", true);
        }
    }
    
    void OnAttack(InputAction.CallbackContext ctx)
    {
        movementBlock = true;
        attackEvent.Invoke();
    }
    
    private void MoveCancel()
    {
        inputAxis = Vector2.zero;
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isSprinting", false);
        isSprinting = false;
    }
    
    private void SprintCancel()
    {
        playerAnimator.SetBool("isSprinting", false);
        isSprinting = false;
    }

    private void FixedUpdate()
    {
        if(!movementBlock) UpdatePosition();
        if(inputAxis != Vector2.zero) UpdateRotation();
    }

    private void UpdatePosition()
    {
        var currMovSpeed = isSprinting ? sprintSpeed : movSpeed;
        var positionMovement = inputAxis != Vector2.zero ? transform.forward * currMovSpeed : Vector3.zero;
        positionMovement.y = 0;
        var newPos = cc.center + positionMovement;
        cc.SimpleMove(newPos);
    }

    private void UpdateRotation()
    {
        var angle = -Vector2.SignedAngle(Vector2.up, inputAxis);
        var fixedInput = Quaternion.Euler(0, angle + camera.eulerAngles.y, 0);
        var nextRot = Quaternion.Lerp(transform.rotation, fixedInput, rotSpeed * Time.deltaTime);
        cc.transform.rotation = nextRot;
    }
}
