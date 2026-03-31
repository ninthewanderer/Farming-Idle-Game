using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Removed the old code because it's not working with the new camera and it's too complicated for me to debug TBH.

public class PlayerController : MonoBehaviour
{
    private ResourceManager resourceManager;
    private CharacterController playerController;
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float turnSpeed;
    private Vector2 input;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetDirection;
    private Quaternion freeRotation;

    // Gets the necessary components and uses the PlayerControls class to handle player movement.
    private void Awake()
    {
        playerController = GetComponent<CharacterController>();

        // Locks cursor (can be changed later)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Used to determine if there is input and for target direction calculation.
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        
        // Actual values used for movement. 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (vertical < 0) vertical *= -1;
        if (horizontal < 0) horizontal *= -1;
        float move = vertical + horizontal;
        
        // Determines the forward direction of the player and uses it to determine movement direction.
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        moveDirection = forward * move;
        moveDirection.Normalize();
        moveDirection *= _moveSpeed;
        
        // Calculates the direction the player will be moving in based on the camera's position.
        UpdateTargetDirection();
        
        // Rotates the player in the direction the camera is facing.
        if (input != Vector2.zero && targetDirection.magnitude >= 0.1f)
        {
            Vector3 lookDirection = targetDirection.normalized;
            freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
            var differenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;
        
            if (differenceRotation < 0 || differenceRotation > 0)
            {
                eulerY = freeRotation.eulerAngles.y;
            }
            var euler = new Vector3(0, eulerY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * Time.deltaTime);
        }
        
        // Moves the player as long as there is input.
        if (input != Vector2.zero)
        {
            playerController.Move(moveDirection * Time.deltaTime);
        }
    }

    // Updates the direction of the player relative to the camera. 
    private void UpdateTargetDirection()
    {
        var forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        
        var right = Camera.main.transform.TransformDirection(Vector3.right);
        targetDirection = input.x * right + input.y * forward;
    }

    // Get and set so shop can upgrade player speed
    public void SetMoveSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }
}