using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool rawMovementInput;
    private Vector3 _inputVector;
    
    [SerializeField] private bool rawMouseLookInput;
    [SerializeField] private Vector2 inputMouseLookVector;

    [SerializeField] private bool isSprinting = false, jump = false;
    
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        _inputVector.x = rawMovementInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        _inputVector.z = rawMovementInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        inputMouseLookVector.x = rawMouseLookInput ? Input.GetAxisRaw("Mouse X") : Input.GetAxis("Mouse X");
        inputMouseLookVector.y = rawMouseLookInput ? Input.GetAxisRaw("Mouse Y") : Input.GetAxis("Mouse Y");

        isSprinting = Input.GetKey(KeyCode.LeftShift);

        jump = Input.GetKeyDown(KeyCode.Space);
    }

    public Vector3 GetInputVector()
    {
        return _inputVector;
    }
    
    public Vector3 GetInputMouseLookVector()
    {
        return inputMouseLookVector;
    }

    public bool GetSprinting()
    {
        return isSprinting;
    }

    public bool GetJump()
    {
        return jump;
    }
    
    public void SetJump(bool jumpState)
    {
        jump = jumpState;
    }
}
