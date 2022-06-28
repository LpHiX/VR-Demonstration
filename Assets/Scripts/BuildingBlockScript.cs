using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingBlockScript : MonoBehaviour
{
    public Rigidbody BuildingBody;
    public MeshRenderer BuildingRenderer;
    public XRRayInteractor LeftInteractor;
    public GameObject CreationPrefab;
    // Start is called before the first frame update
    private bool withinSocket;
    private GameObject parentCreation;
    private CreationScript creationScript;
    void Start()
    {
        LeftInteractor.selectEntered.AddListener(SelectEnterMethods);
        LeftInteractor.selectExited.AddListener(SelectExitMethods);
    }

    private void SelectEnterMethods(SelectEnterEventArgs arg0)
    {
        if (parentCreation == null) return;
        creationScript.checkEmpty();
        transform.SetParent(null);
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
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<GridScript>()) return;
        withinSocket = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<GridScript>()) return;
        withinSocket = true;
    }
}
