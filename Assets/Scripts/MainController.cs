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
    public string[,] map =  {{"ta6", "9", "15", "90"}, {"ta12", "15", "15", "180"}, {"ta2", "-9", "9", "270"}, {"ta12", "-3", "9", "180"}, {"ta5", "9", "9", "0"}, {"ta5", "15", "9", "0"}, {"ta6", "-15", "3", "90"}, {"ta10", "-9", "3", "90"}, {"ta15", "-3", "3", "0"}, {"ta10", "3", "3", "90"}, {"ta13", "9", "3", "270"}, {"ta5", "15", "3", "0"}, {"ta1", "-15", "-3", "180"}, {"ta5", "-3", "-3", "0"}, {"ta3", "9", "-3", "0"}, {"ta9", "15", "-3", "270"}, {"ta6", "-9", "-9", "90"}, {"ta15", "-3", "-9", "0"}, {"ta10", "3", "-9", "90"}, {"ta12", "9", "-9", "180"}, {"ta1", "-9", "-15", "180"}, {"ta3", "-3", "-15", "0"}, {"ta10", "3", "-15", "90"}, {"ta9", "9", "-15", "270"}};
    public string[,] placeables = {{"p0", "-15", "-1", "210"}};

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
        FindObject findObject = new FindObject();


        for(int a = 0; a < map.GetLength(0); a++)
        {
            GameObject currentModel = findObject.FindObjectByCode(allTunnels, map[a, 0]);
            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToInt(map[a, 1]), 0.0f, castInto.stringToInt(map[a, 2])), Quaternion.Euler(0.0f, castInto.stringToInt(map[a, 3]), 0.0f)) as GameObject;
            modelClone.transform.localScale = new Vector3(1.0003f, 1.0003f, 1.0003f);
        }
        for (int a = 0; a < placeables.GetLength(0); a++)
        {
            GameObject currentModel = findObject.FindObjectByCode(allPlaceables, placeables[a, 0]);
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
