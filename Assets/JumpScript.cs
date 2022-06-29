using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpScript : MonoBehaviour
{
    public InputActionReference JumpReference;
    private CharacterController characterController;

    private float yVelocity = 0;
    void Start()
    {
        JumpReference.action.started += jumpMethod;
        characterController = GetComponent<CharacterController>();
    }

    private void jumpMethod(InputAction.CallbackContext obj)
    {
        if (characterController.isGrounded)
        {
            yVelocity = 0.05f;
        }
    }

    void OnDestroy()
    {
        JumpReference.action.started -= jumpMethod;

    }
    private void Update()
    {
        characterController.Move(new Vector3(0, yVelocity, 0));
        if(!characterController.isGrounded)
        {
            yVelocity -= 0.2f * Time.deltaTime;
        }
    }
}
