using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour 
{
    public Color fogColor = Color.red;
    public float maxDensity = 0.2f;
    private float currentDensity = 0f;
    private bool fogAppear = false;

    void Awake()
    {
        if (MainController.mainController.currentScene == 1)
        {
            gameObject.transform.FindChild("Fog").GetComponent<MeshRenderer>().enabled = false;
            if (gameObject.GetComponent<CycleTexture>()) Destroy(gameObject.GetComponent<CycleTexture>());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBody")
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = fogColor;
            fogAppear = true;
        }
    }

    void Update()
    {
        if (fogAppear && currentDensity < maxDensity) currentDensity += 0.002f;
        RenderSettings.fogDensity = currentDensity;
    }
}
