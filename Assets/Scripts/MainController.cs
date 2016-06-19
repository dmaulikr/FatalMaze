using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
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
    public string[,] map =  {{"ta4", "-3", "15", "0"}, {"ta5", "-3", "9", "180"}, {"ta5", "-3", "3", "180"}, {"ta5", "-3", "-3", "180"}, {"ta5", "-3", "-9", "180"}, {"ta1", "-3", "-15", "180"}};
    public string[,] placeables = {{"p0", "-2", "-12", "0"}};

    public GameObject cardBoard;
    public GameObject pcCamera;
    public int currentScene = 0; //Gets number in Update()
    private int previousScene = 0;
    private CastInto castInto = new CastInto();

	// Use this for initialization
	void Start () 
    {
        try
        {
            // Get all tunnels from assets
            GameObject[] allObjects = Resources.LoadAll<GameObject>("Prefabs");

            foreach (GameObject a in allObjects)
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
        catch(Exception err)
        {
            print("failed with " + err);
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
            print(map.GetLength(0));
            GameObject currentModel = CameraControl.mainCamera.findObject(allTunnels, map[a, 0]);
            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToInt(map[a, 1]), 0.0f, castInto.stringToInt(map[a, 2])), Quaternion.Euler(0.0f, castInto.stringToInt(map[a, 3]), 0.0f)) as GameObject;
            modelClone.transform.localScale = new Vector3(1.0003f, 1.0003f, 1.0003f);
        }
        for (int a = 0; a < placeables.GetLength(0); a++)
        {
            GameObject currentModel = CameraControl.mainCamera.findObject(allPlaceables, placeables[a, 0]);
            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToInt(placeables[a, 1]), 0.0f, castInto.stringToInt(placeables[a, 2])), Quaternion.Euler(0.0f, castInto.stringToInt(placeables[a, 3]), 0.0f)) as GameObject;

            // Adding camera
            if (currentModel.GetComponent<Model>().name == "Player")
            {
                if (gameType == "cardboard")
                {
                    GameObject carboardClone = Instantiate(cardBoard, currentModel.transform.position, transform.rotation) as GameObject;
                }
            }
        }


    }


}
