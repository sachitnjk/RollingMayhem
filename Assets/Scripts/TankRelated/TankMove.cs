using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankMove : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction turnAction;

    private Vector2 moveInput;
    private float turnInput;
    
    [Header("Tank References")]
    [SerializeField] private Rigidbody tankRB;
    
    [Header("Tank Attributes")]
    [Range(1f, 10f)]
    [SerializeField] private float moveSpeed = 5f;
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (moveAction != null)
        {
            MoveTank();
        }
        if (turnAction != null)
        {
            TurnTurret();
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

        if (moveInput != null)
        {
            Vector3 forwardMovement = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
            
            tankRB.MovePosition(tankRB.position + forwardMovement);
        }
    }

    private void TurnTurret()
    {
        turnInput = turnAction.ReadValue<float>();

        if (turnInput != 0)
        {
            
        }
    }
}
