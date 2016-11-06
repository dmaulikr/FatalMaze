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
    [System.NonSerialized] public string[,] map =  {{"rc64", "15", "27", "90"}, {"rc192", "21", "27", "180"}, {"rc128", "27", "27", "180"}, {"rc64", "-15", "21", "90"}, {"rc192", "-9", "21", "180"}, {"rc128", "-3", "21", "180"}, {"rc96", "15", "21", "90"}, {"rc240", "21", "21", "0"}, {"rc144", "27", "21", "270"}, {"rc96", "-15", "15", "90"}, {"rc240", "-9", "15", "0"}, {"rc146", "-3", "15", "270"}, {"tc14", "3", "15", "180"}, {"tc12", "9", "15", "180"}, {"rc96", "15", "15", "90"}, {"rc240", "21", "15", "0"}, {"rc144", "27", "15", "270"}, {"rc32", "-15", "9", "0"}, {"rc52", "-9", "9", "0"}, {"rc16", "-3", "9", "270"}, {"tc3", "3", "9", "0"}, {"tc11", "9", "9", "0"}, {"rc104", "15", "9", "90"}, {"rc240", "21", "9", "0"}, {"rc144", "27", "9", "270"}, {"tc7", "-9", "3", "90"}, {"ta10", "-3", "3", "90"}, {"ta12", "3", "3", "180"}, {"rc96", "15", "3", "90"}, {"rc240", "21", "3", "0"}, {"rc144", "27", "3", "270"}, {"tc5", "-9", "-3", "0"}, {"ra64", "-3", "-3", "90"}, {"ra193", "3", "-3", "180"}, {"ra128", "9", "-3", "180"}, {"rc96", "15", "-3", "90"}, {"rc240", "21", "-3", "0"}, {"rc144", "27", "-3", "270"}, {"ta6", "-15", "-9", "90"}, {"ta15", "-9", "-9", "0"}, {"ra104", "-3", "-9", "90"}, {"ra240", "3", "-9", "0"}, {"ra144", "9", "-9", "270"}, {"rc32", "15", "-9", "0"}, {"rc48", "21", "-9", "0"}, {"rc16", "27", "-9", "270"}, {"ta7", "-15", "-15", "90"}, {"ta9", "-9", "-15", "270"}, {"ra32", "-3", "-15", "0"}, {"ra48", "3", "-15", "0"}, {"ra16", "9", "-15", "270"}, {"ra64", "-21", "-21", "90"}, {"ra193", "-15", "-21", "180"}, {"ra128", "-9", "-21", "180"}, {"ra32", "-21", "-27", "0"}, {"ra48", "-15", "-27", "0"}, {"ra16", "-9", "-27", "270"}};
    [System.NonSerialized] public string[,] placeables = {{"p5", "-9", "9", "0"}, {"p5", "-3.1", "-9", "90"}, {"p4", "3", "-2.9", "0"}, {"p3", "3", "-9", "290"}, {"p4", "15", "9", "90"}, {"p3", "-10", "16", "96"}, {"p6", "21", "19", "232"}, {"p1", "-18", "-24", "270"}, {"p1", "-12", "-24", "270"}, {"p0", "-15", "-25", "0"}};

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
