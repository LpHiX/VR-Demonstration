using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InfoScript : MonoBehaviour
{
    [SerializeField]
    private string Message;
    [SerializeField]
    private string Name;
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
        }
        else
        {
            InfoManager._instance.SetInfoBox(Name, Message, transform);
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