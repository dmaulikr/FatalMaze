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
    [System.NonSerialized] public string[,] map =  {{"ra64", "-9", "27", "90"}, {"ra128", "-3", "27", "180"}, {"ra64", "3", "27", "90"}, {"ra128", "9", "27", "180"}, {"tc6", "-15", "21", "90"}, {"ra104", "-9", "21", "90"}, {"ra144", "-3", "21", "270"}, {"ra36", "3", "21", "0"}, {"ra16", "9", "21", "270"}, {"tc5", "-15", "15", "0"}, {"ra32", "-9", "15", "0"}, {"ra18", "-3", "15", "270"}, {"ta9", "3", "15", "270"}, {"tc2", "-21", "9", "270"}, {"tc15", "-15", "9", "0"}, {"rc72", "-9", "9", "90"}, {"rc192", "-3", "9", "180"}, {"rc192", "3", "9", "180"}, {"rc192", "9", "9", "180"}, {"rc192", "15", "9", "180"}, {"rc128", "21", "9", "180"}, {"tc5", "-15", "3", "0"}, {"rc96", "-9", "3", "90"}, {"rc240", "-3", "3", "0"}, {"rc240", "3", "3", "0"}, {"rc240", "9", "3", "0"}, {"rc240", "15", "3", "0"}, {"rc144", "21", "3", "270"}, {"rc64", "-21", "-3", "90"}, {"rc193", "-15", "-3", "180"}, {"rc160", "-9", "-3", "0"}, {"rc48", "-3", "-3", "0"}, {"rc48", "3", "-3", "0"}, {"rc48", "9", "-3", "0"}, {"rc52", "15", "-3", "0"}, {"rc16", "21", "-3", "270"}, {"rc96", "-21", "-9", "90"}, {"rc240", "-15", "-9", "0"}, {"rc144", "-9", "-9", "270"}, {"ta5", "15", "-9", "0"}, {"ra64", "21", "-9", "90"}, {"ra128", "27", "-9", "180"}, {"rc32", "-21", "-15", "0"}, {"rc48", "-15", "-15", "0"}, {"rc22", "-9", "-15", "270"}, {"tc12", "-3", "-15", "180"}, {"ra64", "3", "-15", "90"}, {"ra128", "9", "-15", "180"}, {"ta5", "15", "-15", "0"}, {"ra96", "21", "-15", "90"}, {"ra144", "27", "-15", "270"}, {"tc3", "-9", "-21", "0"}, {"tc9", "-3", "-21", "270"}, {"ra96", "3", "-21", "90"}, {"ra146", "9", "-21", "270"}, {"ta15", "15", "-21", "0"}, {"ra40", "21", "-21", "0"}, {"ra20", "27", "-21", "270"}, {"ra32", "3", "-27", "0"}, {"ra18", "9", "-27", "270"}, {"ta11", "15", "-27", "0"}, {"ta10", "21", "-27", "90"}, {"ta9", "27", "-27", "270"}};
    [System.NonSerialized] public string[,] placeables = {{"p0", "-15", "-12", "0"}, {"p3", "6", "24", "150"}, {"p4", "-9.1", "8.2", "90"}, {"p5", "15", "-3.094", "180"}, {"p7", "15", "-7", "0"}, {"p5", "20.9", "-20.203", "90"}, {"p5", "9.1", "-21", "270"}};

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
            Cursor.lockState = CursorLockMode.Locked;
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
