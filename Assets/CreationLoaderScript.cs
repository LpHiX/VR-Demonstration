using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationLoaderScript : MonoBehaviour
{
    [SerializeField]
    private string creationJson;
    [SerializeField]
    private CreationDataManager CreationDataManager;
    // Start is called before the first frame update
    void Start()
    {
        CreationDataManager.loadToScene(creationJson, transform);
    }
}
