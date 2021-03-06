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
        currentBlock.GetComponent<BuildingBlockScript>().takenMethods();
        heldBlock = currentBlock;
        replaceBlock();
    }

    private void SelectExitMethods(SelectExitEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.gameObject.Equals(heldBlock)) return;
        heldBlock = null;
    }

    private void replaceBlock()
    {
        currentBlock = Instantiate(BuildingBlockPrefab, transform.position + transform.up * 0.2f, transform.rotation,transform);
        currentRenderer = currentBlock.GetComponent<MeshRenderer>();
        BuildingBlockScript currentBlockScript = currentBlock.GetComponent<BuildingBlockScript>();
        currentBlockScript.LeftInteractor = LeftInteractor;
        currentBlockScript.StartMethods();
    }
    void Update()
    {
        currentRenderer.material.color = new Color(
            transform.InverseTransformPoint(RedSlider.transform.position).x * 4 + 0.5f,
            transform.InverseTransformPoint(GreenSlider.transform.position).x * 4 + 0.5f,
            transform.InverseTransformPoint(BlueSlider.transform.position).x * 4 + 0.5f
            );
    }
}
