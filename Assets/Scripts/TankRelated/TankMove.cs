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
    [SerializeField] private float currentMoveSpeed = 0f;
        
    [Header("Tank References")]
    [SerializeField] private Rigidbody tankRB;
    [SerializeField] private TankBaseSO tankBaseSO;
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (moveInput.y == 0 && tankRB.velocity.magnitude < 0.1f)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, 0f, tankBaseSO.decceleration * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (moveAction != null)
        {
            MoveTankPhysics();
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

    private void MoveTankNonPhysics()
    {
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }
    }
    
    private void MoveTankPhysics()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        
        if (moveInput.y != 0)
        {
            
            if (Mathf.Sign(moveInput.y) != Mathf.Sign(currentMoveSpeed) && Mathf.Abs(currentMoveSpeed) > 0.1f)
            {
                // Instantly change the direction and apply initial speed
                currentMoveSpeed = moveInput.y * tankBaseSO.breakthroughSpeed; // Optional breakthrough speed for instant direction change
            }
            else
            {
                // Smooth acceleration within the same direction
                currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, moveInput.y * tankBaseSO.maxSpeed, tankBaseSO.acceleration * Time.deltaTime);
            }
            
            Vector3 forwardMovement = transform.forward * currentMoveSpeed * Time.deltaTime;
            tankRB.velocity = forwardMovement;
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
