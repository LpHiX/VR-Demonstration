using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WirstMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject wristUI;
    private bool wristIsActive = true;
    
    void Start()
    {
        ToggleWristUI();
    }
    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleWristUI();
        }
    }
    public void ReturnHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
    public void DebugMessage()
    {
        Debug.Log("test");
    }

    private void ToggleWristUI()
    {
        if (wristIsActive)
        {
            wristUI.SetActive(false);
            wristIsActive = false;
        }
        else
        {
            wristUI.SetActive(true);
            wristIsActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
