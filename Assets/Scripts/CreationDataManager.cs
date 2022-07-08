using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CreationDataManager : MonoBehaviour
{
    [System.Serializable]
    private class BlockData
    {
        public Vector3Int position;
        public Color color;

        public BlockData(Vector3Int position, Color color)
        {
            this.position = position;
            this.color = color;
        }
    }
    [System.Serializable]
    private class CreationData
    {
        public List<BlockData> blocks = new List<BlockData>();
        internal CreationData(Dictionary<Vector3Int, BlockObject> dictionary)
        {
            foreach (KeyValuePair<Vector3Int, BlockObject> entry in dictionary)
            {
                blocks.Add(new BlockData(entry.Key, entry.Value.color));
            }
        }
    }
    public GameObject CreationPrefab;
    public InputActionReference LoadReference;
    public InputActionReference SaveReference;
    public XRRayInteractor LeftInteractor;
    public XRRayInteractor RightInteractor;
    void Start()
    {
        SaveReference.action.started += saveMethod;
        LoadReference.action.started += loadMethod;
    }

    private void saveMethod(InputAction.CallbackContext obj)
    {
        if (RightInteractor.interactablesSelected.Count == 0)
        {
            Debug.Log("nothing selected");
            return;
        }
        CreationScript creationScript = RightInteractor.interactablesSelected[0].transform.gameObject.GetComponent<CreationScript>();
        CreationData creationData = new CreationData(creationScript.containedBlocks);
        string json = JsonUtility.ToJson(creationData, true);
        File.WriteAllText(Application.dataPath + "/debugFile.json", json);
        //Debug.Log("Saved creation with "+ creationScript.containedBlocks.Count + "blocks");
    }

    private void loadMethod(InputAction.CallbackContext obj)
    {
        string json = File.ReadAllText(Application.dataPath + "/debugFile.json");
        CreationData loadedCreationData = JsonUtility.FromJson<CreationData>(json);
        //Debug.Log("Loading creation with " + loadedCreationData.blocks.Count + " blocks");

        Dictionary<Vector3Int, Color> startingBlocks = new Dictionary<Vector3Int, Color>();
        for (int i = 0; i < loadedCreationData.blocks.Count; i++)
        {
            startingBlocks.Add(loadedCreationData.blocks[i].position, loadedCreationData.blocks[i].color);
        }

        GameObject creationObject = Instantiate(CreationPrefab, RightInteractor.transform.position + RightInteractor.transform.forward * 2, RightInteractor.transform.rotation);
        creationObject.GetComponent<CreationScript>().LoadCustomCreationMethods(startingBlocks, LeftInteractor);
        
    }
}
