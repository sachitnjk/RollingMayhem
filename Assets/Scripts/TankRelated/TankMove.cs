using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TankMove : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction turnAction;

    private Vector2 moveInput;
    private float turnInput;

    private bool isMoving = false;
    private bool isTurning = false;
        
    [Header("Tank References")]
    [SerializeField] private Rigidbody tankRB;
    
    [FormerlySerializedAs("moveSpeed")]
    [Header("Tank Attributes")]
    [SerializeField] private float currentMoveSpeed;
    [Range(1f, 10f)]
    [SerializeField] private float maxSpeed = 10f;
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        // if (moveAction != null)
        // {
        //     MoveTank();
        // }
        if (turnAction != null)
        {
            TurnTurret();
        }
    }

    private void FixedUpdate()
    {
        if (moveAction != null)
        {
            MoveTank();
        }
    }

    private void Initialize()
    {
        playerInput = InputProvider.GetPlayerInput();

        if (playerInput != null)
        {
            moveAction = playerInput.actions["Movement"];
            turnAction = playerInput.actions["TurretRotation"];
        }

        currentMoveSpeed = 0f;
    }
    
    private void MoveTank()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 0f, maxSpeed);

        if (GetMovingStatus())
        {
            HandleAccelerateDecelerateOnMove();
        }
        else if(!moveAction.IsPressed())
        {
            HandleDeccelerate();
        }
        else if (!isMoving && moveAction.IsPressed())
        {
            currentMoveSpeed++;
            isMoving = true;
        }

        if (moveInput != null)
        {
            Vector3 forwardMovement = transform.forward * moveInput.y * currentMoveSpeed * Time.fixedDeltaTime;
            
            tankRB.MovePosition(tankRB.position + forwardMovement);
        }
    }

    private void HandleAccelerateDecelerateOnMove()
    {
        
        if (moveAction.IsPressed())
        {
            if (currentMoveSpeed < maxSpeed)
            {
                currentMoveSpeed++;
            }
        }
        else if(!moveAction.IsPressed())
        {
            if (currentMoveSpeed > 0f)
            {
                currentMoveSpeed--;
            }
        }
    }

    private void HandleDeccelerate()
    {
        if (currentMoveSpeed > 0f)
        {
            currentMoveSpeed--;
        }
    }

    private bool GetMovingStatus()
    {
        if (tankRB.velocity.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        return isMoving;
    }

    private void TurnTurret()
    {
        turnInput = turnAction.ReadValue<float>();

        if (turnInput != 0)
        {
            
        }
    }
}
