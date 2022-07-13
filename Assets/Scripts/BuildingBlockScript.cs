using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingBlockScript : MonoBehaviour
{
    public Rigidbody RigidBody;
    public MeshRenderer BuildingRenderer;
    public XRRayInteractor LeftInteractor;
    public GameObject CreationPrefab;
    public FixedJoint fixedJoint;
    public int BuildingSocketLayer;
    // Start is called before the first frame update
    private bool withinSocket;
    public GameObject parentCreation;
    public CreationScript creationScript;

    private void SelectEnterMethods(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(gameObject)) return;
        if (parentCreation == null) return;
        creationScript.removeBlock(arg0.interactableObject.transform);
        transform.SetParent(null);
        fixedJoint.connectedBody = null;
        AllowSocket(true);
    }

    private void SelectExitMethods(SelectExitEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(gameObject)) return;
        if (withinSocket)
        {
        }
        else
        {
            parentCreation = Instantiate(CreationPrefab, transform.position, transform.rotation);
            creationScript = parentCreation.GetComponent<CreationScript>();
            creationScript.awakeMethods(gameObject);
            transform.SetParent(parentCreation.transform);
            AllowSocket(false);
        }
        
    }

    internal void takenMethods()
    {
        AllowSocket(true);
    }

    internal void AllowSocket(bool allowSocket)
    {
        if (allowSocket)
        {
            GetComponent<XRGrabInteractable>().interactionLayers |= (1 << BuildingSocketLayer);
        }
        else
        {
            GetComponent<XRGrabInteractable>().interactionLayers &= ~(1 << BuildingSocketLayer);
        }
    }

    internal void StartMethods()
    {
        LeftInteractor.selectEntered.AddListener(SelectEnterMethods);
        LeftInteractor.selectExited.AddListener(SelectExitMethods);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<XRSocketInteractor>()) return;
        withinSocket = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<XRSocketInteractor>()) return;
        withinSocket = true;
    }
}
