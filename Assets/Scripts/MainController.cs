using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour 
{
    public static MainController mainController; // static reference to this singleton

    [System.NonSerialized] 
    public string gameType = "cardboard";
    [System.NonSerialized]
    public List<GameObject> allTunnels = new List<GameObject>();
    public List<GameObject> allPlaceables = new List<GameObject>();
    public GameObject images;
    // 1st - model, 2nd - x position, 3rd z position, 4th rotation
    public int[,] map = {{6, -36, -12, 90}, {0, -24, -12, 270}, {1, -24, -6, 180}, {1, -24, 0, 270}, {1, -18, -12, 90}, {1, -18, -6, 0}, {1, -18, 0, 90}, {3, -18, 6, 180}};
    public int[,] placeables = {{5, -42, -12, 90}};

    public GameObject cardBoard;
    public int currentScene = 0; //Gets number in Update()
    private int previousScene = 0;

	// Use this for initialization
	void Start () 
    {

        // Get all tunnels from assets
        foreach (GameObject a in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (a.gameObject.tag == "Tunnel")
            {
                allTunnels.Add(a.gameObject);
            }
            if (a.gameObject.tag == "Placeable")
            {
                allPlaceables.Add(a.gameObject);
            }
        }

	}

    void Awake()
    {
        if(mainController == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            mainController = this;
        }
        else if(mainController != null)
        {
            Destroy(gameObject);
        }

    }
	
	// Update is called once per frame
	void Update () 
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if(previousScene != currentScene)
        {
            previousScene = currentScene;
            loadScene();
        }
	}

    private void loadScene()
    {
        if (currentScene == 1)
        {
            buildMap();
        }
    }

    private void buildMap()
    {
        for(int a = 0; a < map.GetLength(0); a++)
        {
            GameObject currentModel = CameraControl.mainCamera.findById(allTunnels, map[a, 0]);
            GameObject modelClone = Instantiate(currentModel, new Vector3(map[a, 1], 0.0f, map[a, 2]), Quaternion.Euler(0.0f, map[a, 3], 0.0f)) as GameObject;
            modelClone.transform.localScale = new Vector3(1.0003f, 1.0003f, 1.0003f);
        }
        for (int a = 0; a < placeables.GetLength(0); a++)
        {
            GameObject currentModel = CameraControl.mainCamera.findById(allPlaceables, placeables[a, 0]);
            GameObject modelClone = Instantiate(currentModel, new Vector3(placeables[a, 1], 0.0f, placeables[a, 2]), Quaternion.Euler(0.0f, placeables[a, 3], 0.0f)) as GameObject;

            // Adding camera
            if (currentModel.GetComponent<Model>().name == "Player")
            {
                if (gameType == "cardboard")
                {
                    Instantiate(cardBoard, currentModel.transform.position, transform.rotation);
                }
            }
        }


    }


}
