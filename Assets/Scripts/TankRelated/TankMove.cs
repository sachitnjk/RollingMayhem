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
    
    //--Serialized for debug
    [Header("Debug Serialized fields")]
    [SerializeField] private float currentMoveSpeed = 0f;
    [SerializeField] private float currentTurnSpeed = 0f;
    //--
    private bool isTurning = false;
    private bool isGrounded;
    private Vector3 previousVelocity;
        
    [Header("Tank References")]
    [SerializeField] private Rigidbody tankRB;
    [SerializeField] private TankBaseSO tankBaseSO;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxDistanceToCheckForGround = 0.2f;
    
    [Header("Tilt Settings")]
    [SerializeField] private float tiltFactor = 50f;
    [SerializeField] private float maxTiltTorque = 150f;
    
    private void Start()
    {
        Initialize();
        previousVelocity = tankRB.velocity;
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
        if (moveAction != null && IsTankGrounded())
        {
            MoveTankPhysics();
        }

        ApplyTilt();
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
        
        ApplyTilt();
    }
    
    private void MoveTankPhysics()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        
        //forward, backwrad movement
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
        
        // //Base - left, right rotation
        // if (moveInput.x != 0)
        // {
        // }
    }
    
    private void TurnTurret()
    {
        turnInput = turnAction.ReadValue<float>();

        if (turnInput != 0)
        {
            
        }
    }

    private void ApplyTilt()
    {
        // Calculating acceleration (change in velocity over time)
        Vector3 acceleration = (tankRB.velocity - previousVelocity) / Time.fixedDeltaTime;

        // Project acceleration onto the tank's local X-axis to detect pitch tilt
        float forwardAcceleration = Vector3.Dot(acceleration, transform.forward);

        // Calculating torque to apply based on forward acceleration
        Vector3 tiltTorque = -transform.right * forwardAcceleration * tiltFactor;

        // Clampign the torque to prevent excessive tilt
        tiltTorque = Vector3.ClampMagnitude(tiltTorque, maxTiltTorque);
        
        tankRB.AddTorque(tiltTorque, ForceMode.Acceleration);

        // Updating previous velocity
        previousVelocity = tankRB.velocity;
    }
    
    private bool IsTankGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, maxDistanceToCheckForGround, groundLayer);
        return isGrounded;
    }

}
