using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
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
    [System.NonSerialized] public string[,] map =  {{"rd64", "-51", "57", "90"}, {"rd192", "-45", "57", "180"}, {"rd192", "-39", "57", "180"}, {"rd192", "-33", "57", "180"}, {"rd192", "-27", "57", "180"}, {"rc192", "-21", "57", "180"}, {"rc128", "-15", "57", "180"}, {"rd64", "15", "57", "90"}, {"rd128", "21", "57", "180"}, {"ra96", "-51", "51", "90"}, {"ra240", "-45", "51", "0"}, {"rd240", "-39", "51", "0"}, {"rd240", "-33", "51", "0"}, {"rd240", "-27", "51", "0"}, {"rc240", "-21", "51", "0"}, {"rc144", "-15", "51", "270"}, {"rd32", "15", "51", "0"}, {"rd20", "21", "51", "270"}, {"ra96", "-51", "45", "90"}, {"ra240", "-45", "45", "0"}, {"rd240", "-39", "45", "0"}, {"rd240", "-33", "45", "0"}, {"rd240", "-27", "45", "0"}, {"rc240", "-21", "45", "0"}, {"rc144", "-15", "45", "270"}, {"td6", "-3", "45", "90"}, {"td14", "3", "45", "180"}, {"td14", "9", "45", "180"}, {"td10", "15", "45", "90"}, {"td9", "21", "45", "270"}, {"ra96", "-51", "39", "90"}, {"ra240", "-45", "39", "0"}, {"rd240", "-39", "39", "0"}, {"rd240", "-33", "39", "0"}, {"rd240", "-27", "39", "0"}, {"rc240", "-21", "39", "0"}, {"rc144", "-15", "39", "270"}, {"td6", "-9", "39", "90"}, {"td11", "-3", "39", "0"}, {"td15", "3", "39", "0"}, {"td13", "9", "39", "270"}, {"rd96", "-51", "33", "90"}, {"rd240", "-45", "33", "0"}, {"rd240", "-39", "33", "0"}, {"rd240", "-33", "33", "0"}, {"rd240", "-27", "33", "0"}, {"rc176", "-21", "33", "270"}, {"rc16", "-15", "33", "270"}, {"td5", "-9", "33", "0"}, {"td7", "3", "33", "90"}, {"td9", "9", "33", "270"}, {"rd32", "-51", "27", "0"}, {"rd48", "-45", "27", "0"}, {"rd48", "-39", "27", "0"}, {"rd52", "-33", "27", "0"}, {"rc48", "-27", "27", "0"}, {"rc80", "-21", "27", "90"}, {"rc192", "-15", "27", "180"}, {"rc131", "-9", "27", "180"}, {"td10", "-3", "27", "90"}, {"td9", "3", "27", "270"}, {"tc5", "-33", "21", "0"}, {"rc64", "-27", "21", "90"}, {"rc224", "-21", "21", "90"}, {"rc240", "-15", "21", "0"}, {"rc144", "-9", "21", "270"}, {"tc6", "-51", "15", "90"}, {"tc12", "-45", "15", "180"}, {"tc6", "-39", "15", "90"}, {"tc9", "-33", "15", "270"}, {"rc96", "-27", "15", "90"}, {"rc176", "-21", "15", "270"}, {"rc112", "-15", "15", "0"}, {"rc144", "-9", "15", "270"}, {"tc5", "-51", "9", "0"}, {"tc3", "-45", "9", "0"}, {"tc9", "-39", "9", "270"}, {"rc96", "-27", "9", "90"}, {"rc208", "-21", "9", "180"}, {"rc224", "-15", "9", "90"}, {"rc144", "-9", "9", "270"}, {"tc5", "-51", "3", "0"}, {"tc6", "-39", "3", "90"}, {"tc10", "-33", "3", "90"}, {"rc104", "-27", "3", "90"}, {"rc240", "-21", "3", "0"}, {"rc176", "-15", "3", "270"}, {"rc16", "-9", "3", "270"}, {"rc65", "-51", "-3", "90"}, {"rc192", "-45", "-3", "180"}, {"rc129", "-39", "-3", "180"}, {"rc32", "-27", "-3", "0"}, {"rc48", "-21", "-3", "0"}, {"rc16", "-15", "-3", "270"}, {"rc96", "-51", "-9", "90"}, {"rc240", "-45", "-9", "0"}, {"rc144", "-39", "-9", "270"}, {"ra64", "-33", "-9", "90"}, {"ra192", "-27", "-9", "180"}, {"ra192", "-21", "-9", "180"}, {"ra128", "-15", "-9", "180"}, {"rc32", "-51", "-15", "0"}, {"rc52", "-45", "-15", "0"}, {"rc16", "-39", "-15", "270"}, {"ra96", "-33", "-15", "90"}, {"ra240", "-27", "-15", "0"}, {"ra240", "-21", "-15", "0"}, {"ra144", "-15", "-15", "270"}, {"ta7", "-45", "-21", "90"}, {"ta10", "-39", "-21", "90"}, {"ra104", "-33", "-21", "90"}, {"ra240", "-27", "-21", "0"}, {"ra240", "-21", "-21", "0"}, {"ra144", "-15", "-21", "270"}, {"ra64", "-51", "-27", "90"}, {"ra193", "-45", "-27", "180"}, {"ra128", "-39", "-27", "180"}, {"ra96", "-33", "-27", "90"}, {"ra240", "-27", "-27", "0"}, {"ra240", "-21", "-27", "0"}, {"ra144", "-15", "-27", "270"}, {"ra32", "-51", "-33", "0"}, {"ra52", "-45", "-33", "0"}, {"ra16", "-39", "-33", "270"}, {"ra32", "-33", "-33", "0"}, {"ra48", "-27", "-33", "0"}, {"ra48", "-21", "-33", "0"}, {"ra16", "-15", "-33", "270"}, {"tb5", "-45", "-39", "0"}, {"tb5", "-45", "-45", "0"}, {"tb1", "-45", "-51", "180"}};
    [System.NonSerialized] public string[,] placeables = {{"p1", "-45", "-47", "0"}, {"p6", "-45", "-33.1", "0"}, {"p2", "-48", "-30", "90"}, {"p2", "-42", "-30", "270"}, {"p6", "-45", "-26.9", "0"}, {"p5", "-45", "-15.094", "0"}, {"p4", "-27", "-21", "24"}, {"p3", "-33.1", "-21", "270"}, {"p3", "-40", "-3", "0"}, {"p3", "-50", "-3", "0"}, {"p8", "-24", "3", "0"}, {"p4", "18", "54", "224"}, {"p9", "-8", "28", "0"}, {"p9", "-8", "34", "0"}, {"p9", "-2", "28", "0"}, {"p9", "3", "40", "0"}, {"p9", "3", "46", "0"}, {"p9", "9", "46", "0"}, {"p9", "9", "40", "0"}, {"p9", "15", "46", "0"}, {"p3", "20", "51", "0"}, {"p5", "-33", "27", "0"}, {"p2", "-48", "36", "0"}, {"p2", "-42", "36", "0"}, {"p2", "-39", "39", "270"}, {"p2", "-48", "54", "180"}, {"p2", "-42", "54", "0"}, {"p2", "-39", "51", "270"}, {"p2", "-42", "39", "270"}, {"p2", "-42", "51", "270"}, {"p2", "-39", "48", "0"}, {"p2", "-39", "42", "0"}, {"p2", "-30", "30", "270"}, {"p2", "-27", "33", "0"}, {"p2", "-24", "36", "270"}, {"p2", "-24", "42", "270"}, {"p2", "-24", "48", "270"}, {"p2", "-27", "51", "180"}, {"p10", "-27", "54", "0"}, {"p10", "-21", "54", "0"}, {"p10", "-21", "48", "0"}, {"p10", "-15", "48", "0"}, {"p10", "-15", "54", "0"}, {"p7", "-33", "45", "294"}};
    [System.NonSerialized] public Color fogColor = Color.red;
    [System.NonSerialized] public float maxDensity = 0.2f;
    private float currentDensity = 0f;
    private bool fogAppear = false;
    private float fogCooldown = 10;
    private float defaultFogCooldown = 10;
    
    
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
                    a.gameObject.GetComponent<Model>().idOnlyInt = castInto.codeToId(a.gameObject.GetComponent<Model>().code);
                }
                else if (a.gameObject.tag == "Room")
                {
                    allRooms.Add(a.gameObject);
                }

            }

            allPlaceables = allPlaceables.OrderBy(b => b.GetComponent<Model>().idOnlyInt).ToList();

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
        if(Input.GetMouseButtonDown(0) && currentScene == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if(Input.GetKey("f"))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        updateFog();
	}

    public void enterFog(FogMode mode, Color color)
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = mode;
        RenderSettings.fogColor = color;
        fogAppear = true;
    }

    public void exitFog()
    {
        fogAppear = false;
    }

    private void updateFog()
    {
        if (fogAppear && currentDensity < maxDensity) currentDensity += 0.002f;
        else if (!fogAppear && currentDensity > 0 && fogCooldown < 0) currentDensity -= 0.002f;

        if(!fogAppear && currentDensity > 0f)
        {
            fogCooldown -= 0.1f;
        }

        if (!fogAppear && currentDensity < 0.01f)
        {
            RenderSettings.fog = false;
            currentDensity = 0f;
            fogCooldown = defaultFogCooldown;
        }

        RenderSettings.fogDensity = currentDensity;
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
