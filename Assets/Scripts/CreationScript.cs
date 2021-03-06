using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockObject
{
    internal GameObject gameObject;
    internal FixedJoint fixedJoint;
    internal BuildingBlockScript blockScript;
    internal Color color;
    internal BlockObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.fixedJoint = gameObject.GetComponent<FixedJoint>();
        this.blockScript = gameObject.GetComponent<BuildingBlockScript>();
        this.color = gameObject.GetComponent<MeshRenderer>().material.color;
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
    public GameObject BuildingBlockPrefab;
    public GameObject GridPrefab;
    public BoxCollider BoxCollider;
    public GameObject GrabAnchor;

    public Dictionary<Vector3Int, BlockObject> containedBlocks = new Dictionary<Vector3Int, BlockObject>();
    private Dictionary<Vector3Int, GridObject> containedGrids = new Dictionary<Vector3Int, GridObject>();
    private Rigidbody creationRigidbody;
    private Dictionary<Vector3Int, BlockObject> initialBlocks = new Dictionary<Vector3Int, BlockObject>();

    public void checkEmpty()
    {
        if(containedBlocks.Count == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            updateCollider();
        }
    }
    public void LoadCustomCreationMethods(Dictionary<Vector3Int, Color> startingBlocks, XRRayInteractor leftInteractor)
    {
        creationRigidbody = GetComponent<Rigidbody>();
        foreach(KeyValuePair<Vector3Int, Color> entry in startingBlocks)
        {
            GameObject newBlock = Instantiate(BuildingBlockPrefab, transform);
            BuildingBlockScript buildingBlockScript = newBlock.GetComponent<BuildingBlockScript>();
            buildingBlockScript.LeftInteractor = leftInteractor;
            buildingBlockScript.StartMethods();
            buildingBlockScript.parentCreation = gameObject;
            buildingBlockScript.creationScript = this;
            newBlock.GetComponent<MeshRenderer>().material.color = entry.Value;

            newBlock.transform.position = transform.TransformPoint(entry.Key);
            newBlock.transform.localScale = Vector3.one;
            BlockObject blockObject = new BlockObject(newBlock);
            containedBlocks.Add(entry.Key, blockObject);
            initialBlocks.Add(entry.Key, blockObject);
            blockObject.fixedJoint.connectedBody = creationRigidbody;

        }
        foreach (KeyValuePair<Vector3Int, BlockObject> entry in initialBlocks)
        {
            runAdjacentBlocks(createGrids, entry.Value.gameObject.transform.position, false);
        }
        updateCollider();
    }
    public void awakeMethods(GameObject firstBlockObject)
    {
        creationRigidbody = GetComponent<Rigidbody>();
        runAdjacentBlocks(createGrids, firstBlockObject.transform.position, false);
        BlockObject blockObject = new BlockObject(firstBlockObject);
        containedBlocks.Add(new Vector3Int(0, 0, 0), blockObject);
        initialBlocks.Add(new Vector3Int(0, 0, 0), blockObject);
        blockObject.fixedJoint.connectedBody = creationRigidbody;
        updateCollider();
    }

    private bool createGrids(Vector3 position)
    {
        // Returns true if could be created
        if (containedBlocks.ContainsKey(relativeIntPosition(position))) return false;
        if (containedGrids.ContainsKey(relativeIntPosition(position))) return false;

        GridObject gridObject = new GridObject(Instantiate(GridPrefab, position, transform.rotation, transform));
        containedGrids.Add(relativeIntPosition(position), gridObject);
        gridObject.socket.selectEntered.AddListener(SocketEnterMethods);
        gridObject.socket.selectExited.AddListener(SocketExitMethods);
        return true;
    }

    internal void removeBlock(Transform removedTransform)
    {
        containedBlocks.Remove(relativeIntPosition(removedTransform.position));
        runAdjacentBlocks(removeGrid, removedTransform.position, false);
        if(initialBlocks.ContainsKey(relativeIntPosition(removedTransform.position))){
            createGrids(removedTransform.position);
            initialBlocks.Remove(relativeIntPosition(removedTransform.position));
        }
        checkEmpty();
    }

    private bool removeGrid(Vector3 position)
    {
        if (containedBlocks.ContainsKey(relativeIntPosition(position))) return false;
        if (runAdjacentBlocks(gridUseful, position, true))
        {
            //Debug.Log(position + " Found to be useful");
            return false;
        }
        containedGrids[relativeIntPosition(position)].socket.selectExited.RemoveAllListeners();
        Destroy(containedGrids[relativeIntPosition(position)].gameObject);
        containedGrids.Remove(relativeIntPosition(position));
        return true;
    }

    private bool gridUseful(Vector3 position)
    {
        // Grid is not useless if it is connected to a block
        //Debug.Log("Testing + " + position);
        return containedBlocks.ContainsKey(relativeIntPosition(position));
    }
    private Vector3Int relativeIntPosition(Vector3 position) {return Vector3Int.RoundToInt(transform.InverseTransformPoint(position)); }

    private bool runAdjacentBlocks(Func<Vector3, bool> method, Vector3 centerPosition, bool runOnSelf)
    {
        bool outBool = false;
        if (method.Invoke(centerPosition + transform.TransformVector(new Vector3Int(1, 0, 0)))) outBool = true;
        if (method.Invoke(centerPosition + transform.TransformVector(new Vector3Int(-1, 0, 0)))) outBool = true;
        if (method.Invoke(centerPosition + transform.TransformVector(new Vector3Int(0, 1, 0)))) outBool = true;
        if (method.Invoke(centerPosition + transform.TransformVector(new Vector3Int(0, -1, 0)))) outBool = true;
        if (method.Invoke(centerPosition + transform.TransformVector(new Vector3Int(0, 0, 1)))) outBool = true;
        if (method.Invoke(centerPosition + transform.TransformVector(new Vector3Int(0, 0, -1)))) outBool = true;
        if (runOnSelf)
        {
            if (method.Invoke(centerPosition)) outBool = true;
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

    private void SocketExitMethods(SelectExitEventArgs arg0)
    {

    }
}
