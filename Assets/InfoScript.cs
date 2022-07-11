using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScript : MonoBehaviour
{
    public string Message;
    [SerializeField]
    private Transform CameraTransform;
    public void HoverMethod()
    {
        InfoManager._instance.SetInfoBox(Message, transform, CameraTransform);
    }
    public void HoverExitMethod()
    {
        InfoManager._instance.HideInfoBox();
    }
}
