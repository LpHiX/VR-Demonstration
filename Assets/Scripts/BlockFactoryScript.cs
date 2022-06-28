using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockFactoryScript : MonoBehaviour
{
    public GameObject BuildingBlockPrefab = null;
    public GameObject RedSlider = null;
    public GameObject GreenSlider = null;
    public GameObject BlueSlider = null;
    public XRRayInteractor LeftInteractor = null;
    // Start is called before the first frame update
    private GameObject currentBlock = null;
    private MeshRenderer currentRenderer = null;
    private GameObject heldBlock = null;
    void Start()
    {
        replaceBlock();
        LeftInteractor.selectEntered.AddListener(SelectEnterMethods);
        LeftInteractor.selectExited.AddListener(SelectExitMethods);
    }

    private void SelectEnterMethods(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(currentBlock)) return;
        heldBlock = currentBlock;
        replaceBlock();
    }

    private void SelectExitMethods(SelectExitEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(heldBlock)) return;
        heldBlock.GetComponent<Rigidbody>().isKinematic = false;
        heldBlock = null;
    }

    private void replaceBlock()
    {
        currentBlock = Instantiate(BuildingBlockPrefab, transform);
        currentRenderer = currentBlock.GetComponent<MeshRenderer>();
        currentBlock.GetComponent<Rigidbody>().isKinematic = true;
        currentBlock.GetComponent<BuildingBlockScript>().LeftInteractor = LeftInteractor;
    }
    void Update()
    {
        currentRenderer.material.color = new Color(
            RedSlider.transform.localPosition.z * 4 + 0.5f,
            GreenSlider.transform.localPosition.z * 4 + 0.5f,
            BlueSlider.transform.localPosition.z * 4 + 0.5f
            );
    }
}
