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
    public List<GameObject> allRooms = new List<GameObject>();
    public List<GameObject> allPlaceables = new List<GameObject>();
    public GameObject images;
    // 1st - model, 2nd - x position, 3rd z position, 4th rotation
    public string[,] map =  {{"ta6", "-3", "27", "90"}, {"ta8", "3", "27", "90"}, {"ta6", "9", "27", "90"}, {"ta12", "15", "27", "180"}, {"ta7", "-3", "21", "90"}, {"ta10", "3", "21", "90"}, {"ta11", "9", "21", "0"}, {"ta9", "15", "21", "270"}, {"ra64", "21", "21", "90"}, {"ra128", "27", "21", "180"}, {"ta7", "-3", "15", "90"}, {"ta8", "3", "15", "90"}, {"ra64", "9", "15", "90"}, {"ra192", "15", "15", "180"}, {"ra224", "21", "15", "90"}, {"ra144", "27", "15", "270"}, {"ta7", "-3", "9", "90"}, {"ta8", "3", "9", "90"}, {"ra36", "9", "9", "0"}, {"ra48", "15", "9", "0"}, {"ra48", "21", "9", "0"}, {"ra16", "27", "9", "270"}, {"ta6", "-15", "3", "90"}, {"ta10", "-9", "3", "90"}, {"ta15", "-3", "3", "0"}, {"ta10", "3", "3", "90"}, {"ta9", "9", "3", "270"}, {"ta2", "-21", "-3", "270"}, {"ta9", "-15", "-3", "270"}, {"ta5", "-3", "-3", "0"}, {"ra64", "3", "-3", "90"}, {"ra128", "9", "-3", "180"}, {"ta6", "-9", "-9", "90"}, {"ta11", "-3", "-9", "0"}, {"ra104", "3", "-9", "90"}, {"ra144", "9", "-9", "270"}, {"ta5", "-9", "-15", "0"}, {"ra32", "3", "-15", "0"}, {"ra16", "9", "-15", "270"}, {"ta5", "-9", "-21", "0"}, {"ta1", "-9", "-27", "180"}};
    public string[,] placeables = {{"p0", "-9", "-21", "0"}};

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
                else if (a.gameObject.tag == "Placeable")
                {
                    allPlaceables.Add(a.gameObject);
                }
                else if (a.gameObject.tag == "Room")
                {
                    allRooms.Add(a.gameObject);
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
            GameObject currentModel = new GameObject();
                
            if(findObject.FindObjectByCode(allTunnels, map[a, 0]) != null) currentModel = findObject.FindObjectByCode(allTunnels, map[a, 0]);
            else if(findObject.FindObjectByCode(allRooms, map[a, 0]) != null) currentModel = findObject.FindObjectByCode(allRooms, map[a, 0]);

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
