using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public static InfoManager _instance;
    public TextMeshProUGUI TextComponent;
    public GameObject TextPanel;

    private Transform planetTransform;
    private Transform cameraTransform;
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TextPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TextPanel.activeSelf)
        {
            TextPanel.transform.position = planetTransform.position;
            TextPanel.transform.rotation = Quaternion.LookRotation(planetTransform.position - cameraTransform.position, Vector3.up);

        }
    }
    public void SetInfoBox(string Message, Transform planetTransform, Transform cameraTransform)
    {
        this.planetTransform = planetTransform;
        this.cameraTransform = cameraTransform;

        TextPanel.SetActive(true);
        TextComponent.text = Message;
    }
    public void HideInfoBox()
    {
        TextPanel.SetActive(false);
        TextComponent.text = string.Empty;
    }
}
