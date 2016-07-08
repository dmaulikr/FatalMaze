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
    public string[,] map = {{"ra64", "15", "21", "90"}, {"ra192", "21", "21", "180"}, {"ra128", "27", "21", "180"}, {"ra64", "3", "15", "90"}, {"ra128", "9", "15", "180"}, {"ra96", "15", "15", "90"}, {"ra240", "21", "15", "0"}, {"ra144", "27", "15", "270"}, {"ra32", "3", "9", "0"}, {"ra80", "9", "9", "90"}, {"ra224", "15", "9", "90"}, {"ra176", "21", "9", "270"}, {"ra16", "27", "9", "270"}, {"ta4", "-15", "3", "0"}, {"ra96", "9", "3", "90"}, {"ra240", "15", "3", "0"}, {"ra208", "21", "3", "180"}, {"ra128", "27", "3", "180"}, {"ta5", "-15", "-3", "0"}, {"ra64", "-3", "-3", "90"}, {"ra128", "3", "-3", "180"}, {"ra32", "9", "-3", "0"}, {"ra112", "15", "-3", "0"}, {"ra176", "21", "-3", "270"}, {"ra16", "27", "-3", "270"}, {"ta5", "-15", "-9", "0"}, {"ra96", "-3", "-9", "90"}, {"ra208", "3", "-9", "180"}, {"ra192", "9", "-9", "180"}, {"ra224", "15", "-9", "90"}, {"ra144", "21", "-9", "270"}, {"ta2", "-21", "-15", "270"}, {"ta15", "-15", "-15", "0"}, {"ta10", "-9", "-15", "90"}, {"ra104", "-3", "-15", "90"}, {"ra240", "3", "-15", "0"}, {"ra240", "9", "-15", "0"}, {"ra240", "15", "-15", "0"}, {"ra144", "21", "-15", "270"}, {"ta2", "-21", "-21", "270"}, {"ta11", "-15", "-21", "0"}, {"ta8", "-9", "-21", "90"}, {"ra32", "-3", "-21", "0"}, {"ra48", "3", "-21", "0"}, {"ra48", "9", "-21", "0"}, {"ra48", "15", "-21", "0"}, {"ra16", "21", "-21", "270"}};
    public string[,] placeables = {{"p0", "-15", "-3", "0"}};

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
