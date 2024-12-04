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
    
    private bool isTurning = false;
    private float currentMoveSpeed = 0f;
        
    [Header("Tank References")]
    [SerializeField] private Rigidbody tankRB;
    [SerializeField] private TankBaseSO tankBaseSO;
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
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
    }
    
    private void MoveTank()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.y != 0)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, moveInput.y * tankBaseSO.maxSpeed, tankBaseSO.acceleration * Time.deltaTime);
        }
        else
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, 0f, tankBaseSO.decceleration * Time.deltaTime);
        }

        Vector3 forwardMovement = transform.forward * currentMoveSpeed * Time.deltaTime;
        tankRB.MovePosition(transform.position + forwardMovement);
    }
    
    private void TurnTurret()
    {
        turnInput = turnAction.ReadValue<float>();

        if (turnInput != 0)
        {
            
        }
    }
}
