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
    [System.NonSerialized] public string[,] map =  {{"tb4", "-9", "0", "9", "0"}, {"tb5", "-9", "0", "3", "0"}, {"rd64", "-15", "0", "-3", "90"}, {"rd193", "-9", "0", "-3", "180"}, {"rd128", "-3", "0", "-3", "180"}, {"rd32", "-15", "0", "-9", "0"}, {"rd52", "-9", "0", "-9", "0"}, {"rd16", "-3", "0", "-9", "270"}, {"rc64", "-27", "0", "-15", "90"}, {"rc192", "-21", "0", "-15", "180"}, {"rc128", "-15", "0", "-15", "180"}, {"tc5", "-9", "0", "-15", "0"}, {"rc96", "-27", "0", "-21", "90"}, {"rc240", "-21", "0", "-21", "0"}, {"rc146", "-15", "0", "-21", "270"}, {"tc9", "-9", "0", "-21", "270"}, {"rc32", "-27", "0", "-27", "0"}, {"rc52", "-21", "0", "-27", "0"}, {"rc16", "-15", "0", "-27", "270"}, {"tc5", "-21", "0", "-33", "0"}, {"tc5", "-21", "0", "-39", "0"}, {"tc1", "-21", "0", "-45", "180"}};
    [System.NonSerialized] public string[,] placeables = {{"p6", "-21", "-0.0001094341", "-27.094", "0"}, {"p13", "-9", "0", "-12", "0"}, {"p6", "-9", "-0.0001296997", "-12.006", "0"}, {"p6", "-9", "-9.524822E-05", "-9.094", "0"}, {"p12", "-9", "0", "11", "0"}, {"p1", "-21", "0", "-37", "0"}};
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
