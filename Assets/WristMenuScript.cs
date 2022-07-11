using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WristMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject wristUI;
    private bool wristIsActive = true;
    [SerializeField]
    private InputActionReference MenuInputReference;
    
    void Start()
    {
        ToggleWristUI();
        MenuInputReference.action.started += MenuPressed;
    }
    public void MenuPressed(InputAction.CallbackContext context)
    {
        ToggleWristUI();
    }
    public void ReturnHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
    public void CloseMenu()
    {
        ToggleWristUI();
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
