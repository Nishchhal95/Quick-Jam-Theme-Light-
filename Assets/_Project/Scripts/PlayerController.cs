using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Player Input")] [SerializeField]
    private PlayerInput playerInput;
    
    [Header("Ground Check")]
    [SerializeField] private float groundedOffset = .14f;
    [SerializeField] private float groundedRadius = .28f;
    [SerializeField] private LayerMask groundLayer;
    private bool _grounded;

    [Header("Gravity")]
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float jumpHeight = 5f;
    private float _verticalVelocity;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private float _playerSpeed = 10f;
    private Vector3 _movement;
    
    [Header("Camera Motion")]
    [SerializeField] private float mouseXSensitivity = 5f;
    [SerializeField] private float mouseYSensitivity = 5f;
    [SerializeField] private bool showCursor;
    [SerializeField] private CursorLockMode currentCursorLockMode;
    [SerializeField] private Vector2 mouseLookVector;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;

    public AudioSource as_, jump_as;
    public AudioClip[] footsteps;
    public AudioClip jump;
    public AudioClip jumpLand;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GroundCheck();
        Jump();
        ApplyGravity();
        Move();
        CameraMove();
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, 
            transform.position.y - groundedOffset, transform.position.z);
        _grounded = Physics.CheckSphere(spherePosition, groundedRadius, 
            groundLayer, QueryTriggerInteraction.Ignore);
    }
    
    private void Jump()
    {
        if (playerInput.GetJump() && _grounded)
        {
            playerInput.SetJump(false);
            jump_as.PlayOneShot(jump);
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        _verticalVelocity += gravity * Time.deltaTime;
        if (_grounded && _verticalVelocity < 0.0f)
        {
            _verticalVelocity = -2f;
        }
    }

    private void Move()
    {
        _playerSpeed = playerInput.GetSprinting() ? sprintSpeed : walkSpeed;
        
        _movement = (cameraTransform.right * playerInput.GetInputVector().x * _playerSpeed) +
                    (cameraTransform.forward * playerInput.GetInputVector().z * _playerSpeed);
        _movement.y = _verticalVelocity;
       
        characterController.Move(_movement * Time.deltaTime);
        if (_movement.x != 0 || _movement.z != 0)
        {
            if (!as_.isPlaying && _grounded)
            {
                as_.clip = footsteps[UnityEngine.Random.Range(0, footsteps.Length)];
                as_.pitch = playerInput.GetSprinting() ? 1.5f : 1f;
                as_.Play();
            }
        }

        else
        {
            if (as_.isPlaying)
            {
                as_.Stop();
            }
        }
        
        //RotatePlayer();
    }
    
    // private void RotatePlayer()
    // {
    //     Vector3 normalizedInputDirection = new Vector3(_inputVector.x, 0.0f, _inputVector.z).normalized;
    //     // if there is a move input rotate player when the player is moving
    //     if (_inputVector != Vector3.zero)
    //     {
    //         float _targetRotation = Mathf.Atan2(normalizedInputDirection.x, normalizedInputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
    //         float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
    //
    //         // rotate to face input direction relative to camera position
    //         transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    //     }
    // }

    private void CameraMove()
    {
        Cursor.visible = showCursor;
        Cursor.lockState = currentCursorLockMode;

        mouseLookVector = new Vector2(playerInput.GetInputMouseLookVector().x * mouseXSensitivity, 
            playerInput.GetInputMouseLookVector().y * mouseYSensitivity);

        
        Vector3 newEulerAngles = cameraTransform.localRotation.eulerAngles + 
                                 new Vector3(-mouseLookVector.y, mouseLookVector.x, 0);
        if (newEulerAngles.x > 180)
        {
            newEulerAngles.x = newEulerAngles.x - 360;
        }
        newEulerAngles.x = Mathf.Clamp(newEulerAngles.x, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(newEulerAngles);


    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;
			
        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, 
            transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }
}
