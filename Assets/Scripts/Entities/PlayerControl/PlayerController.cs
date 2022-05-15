using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camera;
using DefaultNamespace;
using JetBrains.Annotations;
using PlayerControl;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private AnimationController playerAnimator;

    [SerializeField] private Transform camera;

    [SerializeField] private InputActionReference moveAcRef;
    [SerializeField] private InputActionReference sprintAcRef;
    [SerializeField] private InputActionReference attackAcRef;
    [SerializeField] private InputActionReference dodgeAcRef;

    // [SerializeField] private InputActionReference pauseAcRef;

    [Min(0f)] [SerializeField] private float movSpeed = 1f;
    [Min(0f)] [SerializeField] private float sprintSpeed = 2f;
    [Min(0f)] [SerializeField] private float rotSpeed = 10f;

    [Min(1f)] [SerializeField] private float animationSpeedMultiplier = 1.5f;

    public bool isAttacking = false;
    public bool isDodging = false;


    public Vector2 inputAxis;
    private bool isSprinting = false;
    private bool paused = false;

    [SerializeField] public UnityEvent attackEvent;
    [SerializeField] public UnityEvent dodgeEvent;
    [SerializeField] public UnityEvent pauseEvent;
    [SerializeField] public UnityEvent unpauseEvent;

    private InputAction movAc => FetchAction(moveAcRef);
    private InputAction sprintAc => FetchAction(sprintAcRef);

    private InputAction attackAc => FetchAction(attackAcRef);

    // private InputAction pauseAc => FetchAction(pauseAcRef);
    private InputAction dodgeAc => FetchAction(dodgeAcRef);

    private InputAction FetchAction(InputActionReference reference)
    {
        var action = reference.action;
        if (!action.enabled) action.Enable();
        return action;
    }

    private void Awake()
    {
        var compSet = GetComponent<ComponentSet>();
        playerAnimator = compSet.animationController;
        cc = compSet.characterController;
        var moveSet = GetComponent<Moveset>();
        if (moveSet != null) moveSet.attacksDone.AddListener(OnAttackEnd);
        var dodge = GetComponent<Dodge>();
        if (dodge != null) dodge.DodgeDone.AddListener(OnDodgeEnd);

        playerAnimator.SwitchAnimation("runSpeed", movSpeed * animationSpeedMultiplier);
        playerAnimator.SwitchAnimation("sprintSpeed", sprintSpeed * animationSpeedMultiplier);
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

    private void OnEnable()
    {
        // pauseAc.performed += OnPause;
        movAc.performed += OnMove;
        movAc.canceled += OnMoveCanceled;
        sprintAc.performed += OnSprint;
        sprintAc.canceled += OnSprintCanceled;

        attackAc.performed += OnAttack;
        dodgeAc.performed += OnDodge;
    }

    private void OnDisable()
    {
        // pauseAc.performed -= OnPause;
        movAc.performed -= OnMove;
        movAc.canceled -= OnMoveCanceled;
        sprintAc.performed -= OnSprint;
        sprintAc.canceled -= OnSprintCanceled;

        attackAc.performed -= OnAttack;
        dodgeAc.performed -= OnDodge;
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        SprintCancel();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        MoveCancel();
    }

    private void OnSprint(InputAction.CallbackContext ctx)
    {
        Sprint();
    }


    private void Sprint()
    {
        if (playerAnimator.GetBool("isWalking"))
        {
            playerAnimator.SwitchAnimation("isSprinting", true);
            isSprinting = true;
        }
    }

    private void SprintCancel()
    {
        playerAnimator.SwitchAnimation("isSprinting", false);
        isSprinting = false;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
        playerAnimator.SwitchAnimation("isWalking", true);
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        isAttacking = true;
        attackEvent.Invoke();
    }

    private void OnAttackEnd()
    {
        isAttacking = false;
    }


    void OnDodge(InputAction.CallbackContext ctx)
    {
        if (!isAttacking)
        {
            isDodging = true;
            dodgeEvent.Invoke();
        }
    }

    void OnDodgeEnd()
    {
        isDodging = false;
    }

    private void MoveCancel()
    {
        inputAxis = Vector2.zero;
        playerAnimator.SwitchAnimation("isWalking", false);
        playerAnimator.SwitchAnimation("isSprinting", false);
        isSprinting = false;
    }

    private void FixedUpdate()
    {
        if (!isAttacking && !isDodging) UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        var currMovSpeed = isSprinting ? sprintSpeed : movSpeed;
        var positionMovement = inputAxis != Vector2.zero ? cc.transform.forward * currMovSpeed : Vector3.zero;
        positionMovement.y = 0;
        var newPos = cc.center + positionMovement;
        cc.SimpleMove(newPos);
    }

    private void UpdateRotation()
    {
        var angle = -Vector2.SignedAngle(Vector2.up, inputAxis);
        var fixedInput = Quaternion.Euler(0, angle + camera.eulerAngles.y, 0);
        var nextRotation = Quaternion.Lerp(cc.transform.rotation, fixedInput, rotSpeed * Time.deltaTime);
        cc.transform.rotation = nextRotation;
    }
}