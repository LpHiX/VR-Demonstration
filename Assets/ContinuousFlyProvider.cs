using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class ContinuousFlyProvider : MonoBehaviour
{
    public InputActionReference LJoystickReference;
    public InputActionReference RJoystickReference;
    public Transform XRForward;
    public Transform XROrigin;
    public float MoveSpeed = 5;
    public float TurnSpeed = 60;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 moveVector = XRForward.transform.TransformVector(LJoystickReference.action.ReadValue<Vector2>().x, 0, LJoystickReference.action.ReadValue<Vector2>().y);
        float moveSpeed = Mathf.Exp(RJoystickReference.action.ReadValue<Vector2>().y * 5);
        XROrigin.GetComponent<CharacterController>().Move(moveVector * moveSpeed * Time.deltaTime);

        //XROrigin.Rotate(new Vector3(0,RJoystickReference.action.ReadValue<Vector2>().x * 60 * Time.deltaTime,0));
        //Debug.Log(LJoystickReference.action.ReadValue<Vector2>() + " " + RJoystickReference.action.ReadValue<Vector2>());
    }
}
