using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour 
{
    public static MainController mainController; // static reference to this singleton

    public string[] gameTypes = { "standalone", "cardboard" };
    private string gameType = "standalone";
    [System.NonSerialized] public int currentGameType;
    [System.NonSerialized] public List<GameObject> allTunnels = new List<GameObject>();
    [System.NonSerialized] public List<GameObject> allRooms = new List<GameObject>();
    [System.NonSerialized] public List<GameObject> allPlaceables = new List<GameObject>();
    // 1st - model, 2nd - x position, 3rd z position, 4th rotation
    [System.NonSerialized] public string[,] map =  {{"ra64", "-21", "27", "90"}, {"ra192", "-15", "27", "180"}, {"ra128", "-9", "27", "180"}, {"ra64", "-3", "27", "90"}, {"ra192", "3", "27", "180"}, {"ra192", "9", "27", "180"}, {"ra192", "15", "27", "180"}, {"ra192", "21", "27", "180"}, {"ra128", "27", "27", "180"}, {"ra32", "-21", "21", "0"}, {"ra52", "-15", "21", "0"}, {"ra16", "-9", "21", "270"}, {"ra32", "-3", "21", "0"}, {"ra52", "3", "21", "0"}, {"ra112", "9", "21", "0"}, {"ra240", "15", "21", "0"}, {"ra240", "21", "21", "0"}, {"ra144", "27", "21", "270"}, {"ta6", "-21", "15", "90"}, {"ta11", "-15", "15", "0"}, {"ta14", "-9", "15", "180"}, {"ta10", "-3", "15", "90"}, {"ta9", "3", "15", "270"}, {"ra96", "9", "15", "90"}, {"ra240", "15", "15", "0"}, {"ra240", "21", "15", "0"}, {"ra144", "27", "15", "270"}, {"ta7", "-21", "9", "90"}, {"ta10", "-15", "9", "90"}, {"ta9", "-9", "9", "270"}, {"ra64", "-3", "9", "90"}, {"ra192", "3", "9", "180"}, {"ra160", "9", "9", "0"}, {"ra48", "15", "9", "0"}, {"ra48", "21", "9", "0"}, {"ra16", "27", "9", "270"}, {"ta5", "-21", "3", "0"}, {"ra64", "-9", "3", "90"}, {"ra224", "-3", "3", "90"}, {"ra240", "3", "3", "0"}, {"ra208", "9", "3", "180"}, {"ra128", "15", "3", "180"}, {"ta2", "-27", "-3", "270"}, {"ta15", "-21", "-3", "0"}, {"ta10", "-15", "-3", "90"}, {"ra104", "-9", "-3", "90"}, {"ra240", "-3", "-3", "0"}, {"ra240", "3", "-3", "0"}, {"ra240", "9", "-3", "0"}, {"ra144", "15", "-3", "270"}, {"ta7", "-21", "-9", "90"}, {"ta12", "-15", "-9", "180"}, {"ra32", "-9", "-9", "0"}, {"ra112", "-3", "-9", "0"}, {"ra240", "3", "-9", "0"}, {"ra176", "9", "-9", "270"}, {"ra16", "15", "-9", "270"}, {"ra64", "-27", "-15", "90"}, {"ra193", "-21", "-15", "180"}, {"ra193", "-15", "-15", "180"}, {"ra128", "-9", "-15", "180"}, {"ra32", "-3", "-15", "0"}, {"ra52", "3", "-15", "0"}, {"ra16", "9", "-15", "270"}, {"ra96", "-27", "-21", "90"}, {"ra240", "-21", "-21", "0"}, {"ra240", "-15", "-21", "0"}, {"ra144", "-9", "-21", "270"}, {"ra65", "3", "-21", "90"}, {"ra128", "9", "-21", "180"}, {"ra32", "-27", "-27", "0"}, {"ra48", "-21", "-27", "0"}, {"ra48", "-15", "-27", "0"}, {"ra16", "-9", "-27", "270"}, {"ra32", "3", "-27", "0"}, {"ra16", "9", "-27", "270"}};
    [System.NonSerialized] public string[,] placeables = {{"p0", "-12", "-24", "0"}, {"p1", "-12", "-22", "0"}, {"p1", "-25", "-18", "270"}, {"p1", "-25", "-24", "270"}, {"p1", "-16", "-22", "180"}, {"p1", "-19", "-19", "270"}, {"p1", "-19", "-14", "270"}, {"p2", "-15", "-14.9", "0"}, {"p3", "-11", "-17", "124"}, {"p5", "-21", "-14.9", "0"}, {"p6", "3", "-3", "234"}, {"p4", "-9.1", "-3", "90"}, {"p3", "6", "-23", "200"}, {"p1", "12", "21", "0"}, {"p1", "24", "21", "0"}, {"p4", "3", "20.9", "0"}};

    public GameObject cardBoard;
    public GameObject pcCamera;
    public int currentScene = 0; //Gets number in Update()
    private int previousScene = 0;
    private CastInto castInto = new CastInto();
    private GameObject tunnelsContainer;
    private GameObject placeablesContainer;

	// Use this for initialization
	void Start () 
    {

        try
        {
            // Load all tunnels, rooms and placeables from assets
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
        if(mainController == null) // main controller has to behave like singleton
        {
            DontDestroyOnLoad(transform.gameObject);
            mainController = this;
        }
        else if(mainController != null)
        {
            Destroy(gameObject);
        }

    }
	

	void Update () 
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if(previousScene != currentScene)
        {
            previousScene = currentScene;
            loadScene();
        }
	}

    public void updateGameMode()
    {
        gameType = gameTypes[currentGameType];
    }

    private void loadScene()
    {
        if (currentScene == 1) // if it's a main scene, build map
        {
            if (tunnelsContainer == null) tunnelsContainer = GameObject.Find("Tunnels");
            if (placeablesContainer == null) placeablesContainer = GameObject.Find("Placeables");
            buildMap();
        }
    }

    private void buildMap()
    {
        FindObject findObject = new FindObject();


        for(int a = 0; a < map.GetLength(0); a++) //Build tunnels & rooms
        {
            GameObject currentModel = transform.gameObject; // this is needed to prevent unity from creating an empty gameobject which would be visible in hierarchy
                
            if(findObject.FindObjectByCode(allTunnels, map[a, 0]) != null) currentModel = findObject.FindObjectByCode(allTunnels, map[a, 0]);
            else if(findObject.FindObjectByCode(allRooms, map[a, 0]) != null) currentModel = findObject.FindObjectByCode(allRooms, map[a, 0]);

            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToInt(map[a, 1]), 0.0f, castInto.stringToInt(map[a, 2])), Quaternion.Euler(0.0f, castInto.stringToInt(map[a, 3]), 0.0f)) as GameObject;
            modelClone.transform.parent = tunnelsContainer.transform;
        }
        for (int a = 0; a < placeables.GetLength(0); a++) //Build placeables
        {
            GameObject currentModel = findObject.FindObjectByCode(allPlaceables, placeables[a, 0]);
            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToFloat(placeables[a, 1]), currentModel.transform.position.y, castInto.stringToFloat(placeables[a, 2])), Quaternion.Euler(0.0f, castInto.stringToInt(placeables[a, 3]), 0.0f)) as GameObject;
            modelClone.transform.parent = placeablesContainer.transform;

            // Adding camera
            if (currentModel.GetComponent<Model>().name == "Player")
            {
                if (gameType == "cardboard")
                {
                    GameObject carboardClone = Instantiate(cardBoard, currentModel.transform.position, transform.rotation) as GameObject;
                }
                else if (gameType == "standalone")
                {
                    GameObject pcCameraClone = Instantiate(pcCamera, currentModel.transform.position, transform.rotation) as GameObject;
                }
            }
        }


    }


}
