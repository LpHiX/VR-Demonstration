using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public static InfoManager _instance;
    public TextMeshProUGUI TextComponent;
    public TextMeshProUGUI NameComponent;
    public GameObject TextPanel;
    public GameObject SelectCanvas;
    public GameObject NameCanvas;
    public Transform cameraTransform;

    public Transform planetTransform;
    private Transform selectTransform;
    private RectTransform infoTransform;
    private RectTransform nameTransform;
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
        infoTransform = TextPanel.GetComponent<RectTransform>();
        nameTransform = NameCanvas.GetComponent<RectTransform>();

        TextPanel.SetActive(false);
        SelectCanvas.SetActive(false);
        NameCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TextPanel.activeSelf)
        {
            TextPanel.transform.position = planetTransform.position;
            TextPanel.transform.rotation = Quaternion.LookRotation(planetTransform.position - cameraTransform.position, Vector3.up);
            TextPanel.transform.localScale = 0.003f * Vector3.one * Vector3.Distance(planetTransform.position, cameraTransform.position) * Mathf.Tan(Mathf.Deg2Rad * 60f * 0.5f);
            infoTransform.pivot = new Vector2(-10 * Mathf.Sqrt(planetTransform.localScale.x) / Vector3.Distance(planetTransform.position, cameraTransform.position), 0.5f);

            NameCanvas.transform.position = planetTransform.position; 
            NameCanvas.transform.rotation = Quaternion.LookRotation(planetTransform.position - cameraTransform.position, Vector3.up);
            NameCanvas.transform.localScale = 0.003f * Vector3.one * Vector3.Distance(planetTransform.position, cameraTransform.position) * Mathf.Tan(Mathf.Deg2Rad * 60f * 0.5f);
            nameTransform.pivot = new Vector2(-10 * Mathf.Sqrt(planetTransform.localScale.x) / Vector3.Distance(planetTransform.position, cameraTransform.position), -0.2f);
        }
        if (SelectCanvas.activeSelf)
        {
            SelectCanvas.transform.position = selectTransform.position;
            SelectCanvas.transform.rotation = Quaternion.LookRotation(selectTransform.position - cameraTransform.position, Vector3.up);
            SelectCanvas.transform.localScale = 0.001f * Mathf.Sqrt(selectTransform.localScale.x)  * Vector3.one * Vector3.Distance(selectTransform.position, cameraTransform.position) * Mathf.Tan(Mathf.Deg2Rad * 60f * 0.5f);
        }
    }
    public void SetInfoBox(string Name, string Message, Transform planetTransform)
    {
        this.planetTransform = planetTransform;

        TextPanel.SetActive(true);
        TextComponent.text = Message;

        NameCanvas.SetActive(true);
        NameComponent.text = Name;
    }
    public void HideInfoBox()
    {
        this.planetTransform = null;

        TextPanel.SetActive(false);
        TextComponent.text = string.Empty;

        NameCanvas.SetActive(false);
        NameComponent.text = string.Empty;
    }
    public void ShowSelector(Transform selectTransform)
    {
        this.selectTransform = selectTransform;
        SelectCanvas.SetActive(true);
    }
    public void HideSelector()
    {
        SelectCanvas.SetActive(false);
    }
}
