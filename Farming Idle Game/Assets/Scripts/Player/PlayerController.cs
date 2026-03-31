using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Commented out a whole bunch of the old code because it's not working with the new camera and it's too complicated
 for me to debug TBH. */

public class PlayerController : MonoBehaviour
{
    // private PlayerControls controls;
    // private Vector3 movement;
    // [SerializeField] private float _turnSmoothTime = 0.1f;
    // private float _turnSmoothVelocity;
    
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
        
        // controls = new PlayerControls();
        // controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        // controls.Player.Move.canceled += ctx => movement = Vector3.zero;
    }

    private void Update()
    {
        // Old code for movement & rotation.
        
        // Variables needed for movement.
        // float horizontal = Input.GetAxisRaw("Horizontal");
        // float vertical = Input.GetAxisRaw("Vertical");
        // Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //
        // // Smoothly turns the player in the direction they're moving in.
        // if (direction.magnitude >= 0.1f)
        // {
        //     float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //     float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, 
        //                   _turnSmoothTime);
        //     transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //     
        //     Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //     playerController.Move(moveDir.normalized * _moveSpeed * Time.deltaTime);
        // }
        
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (vertical < 0) vertical *= -1;
        if (horizontal < 0) horizontal *= -1;
        float move = vertical + horizontal;
        
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        moveDirection = forward * move;
        moveDirection.Normalize();
        moveDirection *= _moveSpeed;
        
        UpdateTargetDirection();
        
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
        
        if (input != Vector2.zero)
        {
            // moveDirection.y = moveDirectionY;
            playerController.Move(moveDirection * Time.deltaTime);
        }
    }

    private void UpdateTargetDirection()
    {
        var forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        
        var right = Camera.main.transform.TransformDirection(Vector3.right);
        targetDirection = input.x * right + input.y * forward;
    }

    // Activates Input Actions when the object is enabled.
    // private void OnEnable()
    // {
    //     controls.Player.Enable();
    // }
    //
    // // Deactivates Input Actions when the object is disabled.
    // private void OnDisable()
    // {
    //     controls.Player.Disable();
    // }

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