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
    public string gameType = "standalone";
    [System.NonSerialized]
    public List<GameObject> allTunnels = new List<GameObject>();
    public List<GameObject> allRooms = new List<GameObject>();
    public List<GameObject> allPlaceables = new List<GameObject>();
    public GameObject images;
    // 1st - model, 2nd - x position, 3rd z position, 4th rotation
    public string[,] map = {{"ra64", "-3", "27", "90"}, {"ra192", "3", "27", "180"}, {"ra192", "9", "27", "180"}, {"ra192", "15", "27", "180"}, {"ra128", "21", "27", "180"}, {"ra96", "-3", "21", "90"}, {"ra176", "3", "21", "270"}, {"ra48", "9", "21", "0"}, {"ra48", "15", "21", "0"}, {"ra20", "21", "21", "270"}, {"ra32", "-3", "15", "0"}, {"ra16", "3", "15", "270"}, {"ta4", "9", "15", "0"}, {"ta5", "21", "15", "0"}, {"ta6", "-9", "9", "90"}, {"ta10", "-3", "9", "90"}, {"ta10", "3", "9", "90"}, {"ta15", "9", "9", "0"}, {"ta10", "15", "9", "90"}, {"ta15", "21", "9", "0"}, {"ta8", "27", "9", "90"}, {"ta5", "-9", "3", "0"}, {"ra64", "3", "3", "90"}, {"ra193", "9", "3", "180"}, {"ra128", "15", "3", "180"}, {"ta5", "21", "3", "0"}, {"ta5", "-9", "-3", "0"}, {"ra96", "3", "-3", "90"}, {"ra240", "9", "-3", "0"}, {"ra144", "15", "-3", "270"}, {"ta1", "21", "-3", "180"}, {"ta1", "-9", "-9", "180"}, {"ra32", "3", "-9", "0"}, {"ra48", "9", "-9", "0"}, {"ra16", "15", "-9", "270"}};
    public string[,] placeables = {{"p2", "9", "-3", "144"}, {"p1", "0", "24", "270"}, {"p1", "3", "25", "0"}, {"p0", "-9", "-1", "0"}};

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
            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToInt(placeables[a, 1]), currentModel.transform.position.y, castInto.stringToInt(placeables[a, 2])), Quaternion.Euler(0.0f, castInto.stringToInt(placeables[a, 3]), 0.0f)) as GameObject;
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
