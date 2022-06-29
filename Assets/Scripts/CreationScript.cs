using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

internal class BlockObject
{
    internal GameObject gameObject;
    internal FixedJoint fixedJoint;
    internal BuildingBlockScript blockScript;
    internal BlockObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.fixedJoint = gameObject.GetComponent<FixedJoint>();
        this.blockScript = gameObject.GetComponent<BuildingBlockScript>();
    }
}

internal class GridObject
{
    internal GameObject gameObject;
    internal XRSocketInteractor socket;
    internal GridObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.socket = gameObject.GetComponent<XRSocketInteractor>();
    }
}

public class CreationScript : MonoBehaviour
{
    public GameObject GridPrefab;
    public BoxCollider BoxCollider;
    public GameObject GrabAnchor;

    private Dictionary<Vector3Int, BlockObject> containedBlocks = new Dictionary<Vector3Int, BlockObject>();
    private Dictionary<Vector3Int, GridObject> containedGrids = new Dictionary<Vector3Int, GridObject>();
    private Rigidbody creationRigidbody;
    private bool firstBlockReplaced = false;

    public void checkEmpty()
    {
        Debug.Log(containedBlocks.Count);
        if(containedBlocks.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    public void awakeMethods(GameObject firstBlockObject)
    {
        creationRigidbody = GetComponent<Rigidbody>();
        runAdjacentBlocks(createGrids, firstBlockObject.transform.position, false);
        BlockObject blockObject = new BlockObject(firstBlockObject);
        containedBlocks.Add(new Vector3Int(0, 0, 0), blockObject);
        blockObject.fixedJoint.connectedBody = creationRigidbody;
        updateCollider();
    }

    private bool createGrids(Vector3Int position)
    {
        // Returns true if could be created
        if (containedBlocks.ContainsKey(position)) return false;
        if (containedGrids.ContainsKey(position)) return false;

        GridObject gridObject = new GridObject(Instantiate(GridPrefab, transform.TransformPoint(position), transform.rotation, transform));
        containedGrids.Add(position, gridObject);
        gridObject.socket.selectEntered.AddListener(SocketEnterMethods);
        gridObject.socket.selectExited.AddListener(SocketExitMethods);
        return true;
    }

    private void SocketExitMethods(SelectExitEventArgs arg0)
    {
        
    }

    internal void removeBlock(Transform removedTransform)
    {
        Vector3Int position = Vector3Int.RoundToInt(transform.InverseTransformPoint(removedTransform.position));
        containedBlocks.Remove(position);
        checkEmpty();
        runAdjacentBlocks(removeGrid, removedTransform.position, false);
        if(!firstBlockReplaced && position == Vector3Int.zero){
            createGrids(position);
            firstBlockReplaced = true;
        }
        updateCollider();
    }

    private bool removeGrid(Vector3Int position)
    {
        if (containedBlocks.ContainsKey(position)) return false;
        if (runAdjacentBlocks(gridUseful, position, true)) return false;
        containedGrids[position].socket.selectExited.RemoveAllListeners();
        Destroy(containedGrids[position].gameObject);
        containedGrids.Remove(position);
        return true;
    }

    private bool gridUseful(Vector3Int position)
    {
        // Grid is not useless if it is connected to a block
        return containedBlocks.ContainsKey(position);
    }

    private void SocketEnterMethods(SelectEnterEventArgs arg0)
    {
        arg0.interactableObject.transform.SetParent(transform);
        BlockObject blockObject = new BlockObject(arg0.interactableObject.transform.gameObject);
        containedBlocks.Add(Vector3Int.RoundToInt(transform.InverseTransformPoint(arg0.interactorObject.transform.position)), blockObject);

        blockObject.blockScript.parentCreation = gameObject;
        blockObject.blockScript.creationScript = this;

        blockObject.gameObject.transform.position = arg0.interactorObject.transform.position;
        blockObject.gameObject.transform.rotation = arg0.interactorObject.transform.rotation;
        blockObject.fixedJoint.connectedBody = creationRigidbody;

        runAdjacentBlocks(createGrids, arg0.interactorObject.transform.position, false);
        updateCollider();
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
    private void updateCollider()
    {
        Vector3 minVector = Vector3.positiveInfinity;
        Vector3 maxVector = Vector3.negativeInfinity;
        foreach (Vector3Int position in containedBlocks.Keys)
        {
            minVector.x = position.x < minVector.x ? position.x : minVector.x;
            minVector.y = position.y < minVector.y ? position.y : minVector.y;
            minVector.z = position.z < minVector.z ? position.z : minVector.z;

            maxVector.x = position.x > maxVector.x ? position.x : maxVector.x;
            maxVector.y = position.y > maxVector.y ? position.y : maxVector.y;
            maxVector.z = position.z > maxVector.z ? position.z : maxVector.z;
        }
        BoxCollider.size = maxVector - minVector + new Vector3(1, 1, 1);
        BoxCollider.center = (maxVector + minVector)/2;
        GrabAnchor.transform.position = transform.TransformPoint((maxVector + minVector) / 2);
    }
}
