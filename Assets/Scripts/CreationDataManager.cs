using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CreationDataManager : MonoBehaviour
{
    private class CreationData
    {
        public List<Vector3Int> positions = new List<Vector3Int>();
        public List<Color> colors = new List<Color>();
        internal CreationData(Dictionary<Vector3Int, BlockObject> dictionary)
        {
            foreach (KeyValuePair<Vector3Int, BlockObject> entry in dictionary)
            {
                positions.Add(entry.Key);
                colors.Add(entry.Value.color);
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
        string json = JsonUtility.ToJson(creationData);
        File.WriteAllText(Application.dataPath + "/debugFile.json", json);
        Debug.Log("Saved creation with "+ creationScript.containedBlocks.Count + "blocks");
    }

    private void loadMethod(InputAction.CallbackContext obj)
    {
        string json = File.ReadAllText(Application.dataPath + "/debugFile.json");
        CreationData loadedCreationData = JsonUtility.FromJson<CreationData>(json);
        Debug.Log("Loading creation with " + loadedCreationData.positions.Count + " blocks");

        Dictionary<Vector3Int, Color> startingBlocks = new Dictionary<Vector3Int, Color>();
        for (int i = 0; i < loadedCreationData.positions.Count; i++)
        {
            startingBlocks.Add(loadedCreationData.positions[i], loadedCreationData.colors[i]);
        }

        GameObject creationObject = Instantiate(CreationPrefab, RightInteractor.transform.position + RightInteractor.transform.forward * 5, RightInteractor.transform.rotation);
        creationObject.GetComponent<CreationScript>().LoadCustomCreationMethods(startingBlocks, LeftInteractor);
    }
}
