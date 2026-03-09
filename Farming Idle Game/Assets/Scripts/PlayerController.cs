using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController playerController;
    private PlayerControls controls;
    private Vector3 movement;
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    

    // Gets the necessary components and uses the PlayerControls class to handle player movement.
    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = Vector3.zero;
    }

    private void Update()
    {
        // Variables needed for movement.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Smoothly turns the player in the direction they're moving in.
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, 
                          _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * _moveSpeed * Time.deltaTime);
        }
    }

    // Activates Input Actions when the object is enabled.
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    
    // Deactivates Input Actions when the object is disabled.
    private void OnDisable()
    {
        controls.Player.Disable();
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
