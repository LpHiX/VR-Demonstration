using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadFlameTest()
    {
        SceneManager.LoadScene("FlameTestScene");
    }
    public void LoadBuildingScene()
    {
        SceneManager.LoadScene("BuildingScene");
    }
    public void LoadSolarSystemScene()
    {
        SceneManager.LoadScene("SolarSystemScene");
    }
}
