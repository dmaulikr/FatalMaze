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
    // 1st - model, 2nd - x position, 3rd y position, 4th z rotation, 5th rotation
    [System.NonSerialized] public string[,] map = {{"re64", "-15", "0", "45", "90"}, {"re192", "-9", "0", "45", "180"}, {"re192", "-3", "0", "45", "180"}, {"re192", "3", "0", "45", "180"}, {"re192", "9", "0", "45", "180"}, {"re192", "15", "0", "45", "180"}, {"re128", "21", "0", "45", "180"}, {"re32", "-15", "0", "39", "0"}, {"re52", "-9", "0", "39", "0"}, {"re48", "-3", "0", "39", "0"}, {"re48", "3", "0", "39", "0"}, {"re48", "9", "0", "39", "0"}, {"re112", "15", "0", "39", "0"}, {"re144", "21", "0", "39", "270"}, {"rc64", "-21", "0", "33", "90"}, {"rc128", "-15", "0", "33", "180"}, {"ta5", "-9", "0", "33", "0"}, {"rc64", "-3", "0", "33", "90"}, {"rc128", "3", "0", "33", "180"}, {"re96", "15", "0", "33", "90"}, {"re144", "21", "0", "33", "270"}, {"rc96", "-21", "0", "27", "90"}, {"rc144", "-15", "0", "27", "270"}, {"ta5", "-9", "0", "27", "0"}, {"rc96", "-3", "0", "27", "90"}, {"rc144", "3", "0", "27", "270"}, {"ta6", "9", "0", "27", "90"}, {"re104", "15", "0", "27", "90"}, {"re144", "21", "0", "27", "270"}, {"rc32", "-21", "0", "21", "0"}, {"rc20", "-15", "0", "21", "270"}, {"ta5", "-9", "0", "21", "0"}, {"rc36", "-3", "0", "21", "0"}, {"rc20", "3", "0", "21", "270"}, {"ta5", "9", "0", "21", "0"}, {"re96", "15", "0", "21", "90"}, {"re144", "21", "0", "21", "270"}, {"ra65", "-15", "0", "15", "90"}, {"ra193", "-9", "0", "15", "180"}, {"ra193", "-3", "0", "15", "180"}, {"ra193", "3", "0", "15", "180"}, {"ra129", "9", "0", "15", "180"}, {"re32", "15", "0", "15", "0"}, {"re16", "21", "0", "15", "270"}, {"ra96", "-15", "0", "9", "90"}, {"ra240", "-9", "0", "9", "0"}, {"ra240", "-3", "0", "9", "0"}, {"ra240", "3", "0", "9", "0"}, {"ra144", "9", "0", "9", "270"}, {"ra32", "-15", "0", "3", "0"}, {"ra48", "-9", "0", "3", "0"}, {"ra52", "-3", "0", "3", "0"}, {"ra48", "3", "0", "3", "0"}, {"ra16", "9", "0", "3", "270"}, {"tb5", "-3", "0", "-3", "0"}, {"tb5", "-3", "0", "-9", "0"}, {"tb5", "-3", "0", "-15", "0"}, {"tb1", "-3", "0", "-21", "180"}};
    [System.NonSerialized] public string[,] placeables = {{"p6", "-3", "0", "2.9", "0"}, {"p6", "-9", "0", "15.1", "0"}, {"p6", "-14.2", "0", "15.1", "0"}, {"p6", "-3", "0", "15.1", "0"}, {"p6", "3", "0", "15.1", "0"}, {"p6", "8.19", "0", "15.1", "0"}, {"p6", "14.91", "0", "27", "90"}, {"p6", "-9", "0", "38.91", "0"}, {"p1", "-3", "0", "-21", "0"}};
    [System.NonSerialized] public Color fogColor = Color.red;
    [System.NonSerialized] public float maxDensity = 0.2f;
    private float currentDensity = 0f;
    private bool fogAppear = false;
    private float fogCooldown = 10;
    private float defaultFogCooldown = 10;
    
    
    public GameObject cardBoard;
    public GameObject pcCamera;
    public GameObject cardboardReadyScreen;
    public GameObject pcReadyScreen;
    public int currentScene = 0; //Gets number in Update()
    private int previousScene = 0;
    private CastInto castInto = new CastInto();
    private GameObject tunnelsContainer;
    private GameObject placeablesContainer;
    private GameObject gameCamera;
    [System.NonSerialized]
    public bool isPlaying = false;
    [System.NonSerialized]
    public bool isReady = false;

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
            resetDefaults();
            Cursor.lockState = CursorLockMode.Locked;
            if (tunnelsContainer == null) tunnelsContainer = GameObject.Find("Tunnels");
            if (placeablesContainer == null) placeablesContainer = GameObject.Find("Placeables");
            buildMap();
        }
    }

    private void resetDefaults()
    {
        isPlaying = false;
        isReady = false;
    }

    public void addGameCamera()
    {
        Instantiate(gameCamera, gameCamera.transform.position, gameCamera.transform.rotation);
    }

    private void buildMap()
    {
        FindObject findObject = new FindObject();


        for(int a = 0; a < map.GetLength(0); a++) //Build tunnels & rooms
        {
            GameObject currentModel = transform.gameObject; // this is needed to prevent unity from creating an empty gameobject which would be visible in hierarchy
                
            if(findObject.FindObjectByCode(allTunnels, map[a, 0]) != null) currentModel = findObject.FindObjectByCode(allTunnels, map[a, 0]);
            else if(findObject.FindObjectByCode(allRooms, map[a, 0]) != null) currentModel = findObject.FindObjectByCode(allRooms, map[a, 0]);

            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToInt(map[a, 1]), 0.0f, castInto.stringToInt(map[a, 3])), Quaternion.Euler(0.0f, castInto.stringToInt(map[a, 4]), 0.0f)) as GameObject;
            modelClone.transform.parent = tunnelsContainer.transform;
        }
        for (int a = 0; a < placeables.GetLength(0); a++) //Build placeables
        {
            GameObject currentModel = findObject.FindObjectByCode(allPlaceables, placeables[a, 0]);
            GameObject modelClone = Instantiate(currentModel, new Vector3(castInto.stringToFloat(placeables[a, 1]), castInto.stringToFloat(placeables[a, 2]), castInto.stringToFloat(placeables[a, 3])), Quaternion.Euler(0.0f, castInto.stringToInt(placeables[a, 4]), 0.0f)) as GameObject;
            modelClone.transform.parent = placeablesContainer.transform;

            // Adding camera
            if (currentModel.GetComponent<Model>().name == "Player")
            {
                if (gameType == "cardboard")
                {
                    gameCamera = cardBoard;
                    gameCamera.transform.position = currentModel.transform.position;
                    gameCamera.transform.rotation = transform.rotation;
                    //GameObject carboardClone = Instantiate(cardBoard, currentModel.transform.position, transform.rotation) as GameObject;
                }
                else if (gameType == "standalone")
                {
                    gameCamera = pcCamera;
                    gameCamera.transform.position = currentModel.transform.position;
                    gameCamera.transform.rotation = transform.rotation;
                    //GameObject pcCameraClone = Instantiate(pcCamera, currentModel.transform.position, transform.rotation) as GameObject;
                }
            }
        }

        isReady = true;
    }


}
