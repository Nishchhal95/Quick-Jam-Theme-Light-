using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool rawMovementInput;
    private Vector3 _inputVector;
    
    [SerializeField] private bool rawMouseLookInput;
    [SerializeField] private Vector2 inputMouseLookVector;

    [SerializeField] private bool isSprinting = false, jump = false, didAttack = false;

    private void Start()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Additive);
    }

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

        didAttack = Input.GetMouseButtonDown(0) || Input.GetMouseButton(0);
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

    public bool GetDidAttack()
    {
        return didAttack;
    }
}
