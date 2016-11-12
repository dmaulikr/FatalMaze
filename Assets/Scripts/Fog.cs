using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour 
{
    public Color fogColor = Color.red;
    public float maxDensity = 0.2f;
    private float currentDensity = 0f;
    private bool fogAppear = false;
    private MainController mainController = MainController.mainController;

    void Awake()
    {
        gameObject.transform.FindChild("Fog").GetComponent<Renderer>().material.color = fogColor;

        if (MainController.mainController.currentScene == 1)
        {
            gameObject.transform.FindChild("Fog").GetComponent<MeshRenderer>().enabled = false;
            if (gameObject.GetComponent<CycleTexture>()) Destroy(gameObject.GetComponent<CycleTexture>());
        }
    }

    void OnTriggerEnter(Collider other)
    {

        mainController.fogColor = fogColor;
        mainController.maxDensity = maxDensity;

        if(other.tag == "PlayerBody")
        {
            mainController.enterFog(FogMode.ExponentialSquared, fogColor);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerBody")
        {
            mainController.exitFog();
        }
    }
}
