using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockDupeScript : MonoBehaviour
{
    public XRBaseInteractable DeleteButton;
    public XRRayInteractor LeftInteractor;
    private GameObject clonedObject;
    private XRSocketInteractor socketInteractor;
    private MeshRenderer deleteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        deleteRenderer = DeleteButton.gameObject.GetComponent<MeshRenderer>();

        socketInteractor.selectEntered.AddListener(BeginCloning);
        socketInteractor.selectExited.AddListener(CloneMethod);
        DeleteButton.selectEntered.AddListener(DeleteMethod);
    }

    private void CloneMethod(SelectExitEventArgs arg0)
    {
        if (!gameObject.scene.isLoaded) return;
        clonedObject = Instantiate(clonedObject, transform.position, transform.rotation);
        clonedObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        BuildingBlockScript blockScript = clonedObject.GetComponent<BuildingBlockScript>();
        blockScript.LeftInteractor = LeftInteractor;
        blockScript.StartMethods();
    }

    private void BeginCloning(SelectEnterEventArgs arg0)
    {
        clonedObject = arg0.interactableObject.transform.gameObject;
        deleteRenderer.material.color = new Color(1, 0, 0);
    }

    private void DeleteMethod(SelectEnterEventArgs arg0)
    {
        deleteRenderer.material.color = new Color(0, 1, 0);
        socketInteractor.interactionManager.CancelInteractorSelection(socketInteractor);
        Destroy(clonedObject);
        clonedObject = null;
    }
}
