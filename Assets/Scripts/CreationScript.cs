using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CreationScript : MonoBehaviour
{
    public GameObject GridPrefab;

    private Dictionary<Vector3Int, GameObject> containedBlocks = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, GameObject> containedGrids = new Dictionary<Vector3Int, GameObject>();

    public void checkEmpty()
    {
        if(containedGrids.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    public void awakeMethods(GameObject firstBlockObject)
    {
        containedBlocks.Add(new Vector3Int(0, 0, 0), firstBlockObject);
        firstBlockObject.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
        runAdjacentBlocks(createGrids, firstBlockObject.transform.position, false);
    }

    private bool createGrids(Vector3Int position)
    {
        // Returns true if could be created
        if (containedBlocks.ContainsKey(position)) return false;
        if (containedGrids.ContainsKey(position)) return false;

        GameObject gridObject = Instantiate(GridPrefab, transform.TransformPoint(position), transform.rotation, transform);
        containedGrids.Add(position, gridObject);
        XRSocketInteractor gridSocket = gridObject.GetComponent<XRSocketInteractor>();
        gridSocket.selectEntered.AddListener(SelectEnterMethods);
        gridSocket.selectExited.AddListener(SelectExitMethods);
        return true;
    }

    private void SelectEnterMethods(SelectEnterEventArgs arg0)
    {
        throw new NotImplementedException();
    }

    private void SelectExitMethods(SelectExitEventArgs arg0)
    {
        throw new NotImplementedException();
    }

    private bool runAdjacentBlocks(Func<Vector3Int, bool> method, Vector3 centerPosition, bool runOnSelf)
    {
        bool outBool = false;
        if (method.Invoke(Vector3Int.RoundToInt(transform.InverseTransformPoint(centerPosition)) + new Vector3Int(1, 0, 0))) outBool = true;
        if (method.Invoke(Vector3Int.RoundToInt(transform.InverseTransformPoint(centerPosition)) + new Vector3Int(-1, 0, 0))) outBool = true;
        if (method.Invoke(Vector3Int.RoundToInt(transform.InverseTransformPoint(centerPosition)) + new Vector3Int(0, 1, 0))) outBool = true;
        if (method.Invoke(Vector3Int.RoundToInt(transform.InverseTransformPoint(centerPosition)) + new Vector3Int(0, -1, 0))) outBool = true;
        if (method.Invoke(Vector3Int.RoundToInt(transform.InverseTransformPoint(centerPosition)) + new Vector3Int(0, 0, 1))) outBool = true;
        if (method.Invoke(Vector3Int.RoundToInt(transform.InverseTransformPoint(centerPosition)) + new Vector3Int(0, 0, -1))) outBool = true;
        if (runOnSelf)
        {
            if (method.Invoke(Vector3Int.RoundToInt(transform.InverseTransformPoint(centerPosition)))) outBool = true;
        }
        return outBool;
    }
}
