using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InfoScript : MonoBehaviour
{
    public string Message;
    [SerializeField]
    public void Awake()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(HoverMethod);
        interactable.hoverExited.AddListener(HoverExitMethod);
        interactable.selectEntered.AddListener(SelectMethod);
    }

    private void SelectMethod(SelectEnterEventArgs arg0)
    {
        if (InfoManager._instance.planetTransform == transform)
        {
            InfoManager._instance.HideInfoBox();
            Debug.Log("Hiding");
        }
        else
        {
            InfoManager._instance.SetInfoBox(Message, transform);
            Debug.Log("Showing");
        }
    }

    private void HoverMethod(HoverEnterEventArgs arg0)
    {
        InfoManager._instance.ShowSelector(transform);
    }

    private void HoverExitMethod(HoverExitEventArgs arg0)
    {
        InfoManager._instance.HideSelector();
    }
}