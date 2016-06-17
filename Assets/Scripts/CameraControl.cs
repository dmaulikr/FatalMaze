using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour 
{
    public static CameraControl mainCamera;
    private MainController controller;
    private Camera camera;
    private Ray ray;
    private RaycastHit hit;
    private Transform objectHit;
    private List<GameObject> segmentList;
    private Vector2 mousePosition;
    private float screenWidth;
    private float screenHeight;
    private int defaultSlide;
    private int currentModel = 0;
    public int[,] map;
    
    public int slideSpeed = 10;
    public int scrollSpeed = 2;
    public GameObject selectedObject;
    public string selectedIdShort;
    public string selectedIdFull;
    private Vector3 selectedPosition = new Vector3(0f, 0f, 0f);
    public GameObject models;
    private int[,] currentMap;
    private int[,] currentPlaceables;
    private string mapsPath;
    private GameObject tunnels;
    private GameObject placeables;

    void Start()
    {
        controller = MainController.mainController;
        tunnels = GameObject.Find("Tunnels");
        placeables = GameObject.Find("Placeables");
        mapsPath = Application.dataPath + "/maps/";
        mainCamera = this;
        camera = transform.GetComponent<Camera>();
        defaultSlide = slideSpeed;
        segmentList = GameObject.Find("LineRenderer").GetComponent<LineRendererScript>().segmentList;

        loadLocaly();
    }

    void Update()
    {

        ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            objectHit = hit.transform;
            if (selectedObject != null)
            {
                selectedObject.transform.position = new Vector3(hit.point.x, 0f, hit.point.z);
            }
            if (objectHit.gameObject.tag == "SegmentTunnel") objectHit.gameObject.GetComponent<SegmentTunnel>().outline();
        }

        if (selectedObject != null) selectedPosition = selectedObject.transform.position;
        //if (selectedObject.tag == "Placeable") selectedObject.transform.position = objectHit.transform.position;


        //UI//
        if(Input.GetMouseButtonDown(0))
        {
            if(selectedObject != null) putObject();
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("delete"))
        {
                deleteObject();
        }

        if(Input.GetKeyDown("u"))
        {
            tryLevel();
        }

        //rotating
        if (Input.GetKeyDown("t") || Input.GetAxis("Mouse ScrollWheel") < 0f && selectedObject != null) //Zoom)
        {
            selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, selectedObject.transform.localEulerAngles.y + 90f, 0.0f));
        }
        if (Input.GetKeyDown("r") || Input.GetAxis("Mouse ScrollWheel") > 0f && selectedObject != null) //Zoom)
        {
            selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, selectedObject.transform.localEulerAngles.y - 90f, 0.0f));
        }


        // zooming
        if(Input.GetAxis("Mouse ScrollWheel") > 0f && selectedObject == null) //Zoom
        {
            if(transform.position.y > 5f) transform.Translate(Vector3.forward * scrollSpeed);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && selectedObject == null) //Zoom out
        {
            if (transform.position.y < 85f) transform.Translate(-Vector3.forward * scrollSpeed);
        }

        //camera slide
        if (mousePosition[0] <= 5 || Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * slideSpeed);
        }
        if (mousePosition[0] >= screenWidth - 5 || Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * slideSpeed);
        }
        if (mousePosition[1] <= 5 || Input.GetKey("s"))
        {
            transform.Translate(Vector3.down * Time.deltaTime * slideSpeed);
        }
        if (mousePosition[1] >= screenHeight - 5 || Input.GetKey("w"))
        {
            transform.Translate(Vector3.up * Time.deltaTime * slideSpeed);
        }
        if(transform.position.y > 30)
        {
            slideSpeed = 25;
        }
        else
        {
            slideSpeed = defaultSlide;
        }

        // Others
        mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        screenWidth = Screen.width;
        screenHeight = Screen.height;

    }

    private bool isSegment()
    {
        if (objectHit.gameObject.tag == "Segment") return true;
        else return false;
    }

    public void putObject()
    {

        if (selectedObject != null && !EditorUI.editorUI.panelVisible) //This will entangle segment and current tunnel
        {
            if(selectedObject.tag == "Tunnel" && objectHit.tag == "SegmentTunnel")
            {
                objectHit.GetComponent<SegmentTunnel>().type = selectedIdShort;
                updateSegments(objectHit.transform);
            }
            else if (selectedObject.tag == "Placeable")
            {
                selectedObject.transform.parent = placeables.transform;
                selectedObject.GetComponent<Model>().saveStats();
                selectedObject = null;
                selectObject(selectedIdFull);
            }
        }

    }

    public void updateSegments(Transform collider)
    {
        Collider[] colliders = Physics.OverlapSphere(collider.position, 1);
        for(int a = 0; a < colliders.Length; a++)
        {
            if(colliders[a].gameObject.tag == "Segment")
            {
                colliders[a].gameObject.GetComponent<Segment>().updateSegment();
            }
        }
    }

    public void deleteObject()
    {
        if(selectedObject != null)
        {
            Destroy(selectedObject);
        }
        if (objectHit.gameObject.GetComponent<Segment>().holder != null && selectedObject == null)
        {
            Destroy(objectHit.gameObject.GetComponent<Segment>().holder);
        }
    }

    public void selectObject(string modelId)
    {
        string tag = "";
        if (findObject(controller.allTunnels, modelId))
        {
            tag = findObject(controller.allTunnels, modelId).gameObject.tag;
        }
        else if(findObject(controller.allPlaceables, modelId))
        {
            tag = findObject(controller.allPlaceables, modelId).gameObject.tag;
        }

        if(tag == "Tunnel")
        {
            if (findObject(controller.allTunnels, modelId))
            {
                selectedObject = findObject(controller.allTunnels, modelId);
                selectedIdShort = selectedObject.GetComponent<Model>().shortCode;
            }
        }
        else if(tag == "Placeable")
        {
            if (findObject(controller.allPlaceables, modelId))
            {
                GameObject placeable = findObject(controller.allPlaceables, modelId);
                selectedObject = Instantiate(placeable, placeable.transform.position, placeable.transform.rotation) as GameObject;
                selectedIdFull = selectedObject.GetComponent<Model>().code;
            }
        }

    }

    public GameObject findObject(List<GameObject> list, string id) // gets model from list by string id e.g t1
    {
        GameObject returnObject = list[0];
        bool found = false;

        for(int a = 0; a < list.Count; a++)
        {
            if(list[a].GetComponent<Model>().code == id)
            {
                returnObject = list[a];
                found = true;
                break;
            }
        }
        if (found) return returnObject;
        else return null;
    }

    public GameObject findById(List<GameObject> list, int id) // gets model from given list int id
    {
        GameObject returnObject = list[0];
        bool found = false;

        for (int a = 0; a < list.Count; a++)
        {
            if (list[a].GetComponent<SegmentTunnel>() && list[a].GetComponent<SegmentTunnel>().id == id || list[a].GetComponent<Model>() && list[a].GetComponent<Model>().id == id)
            {
                returnObject = list[a];
                found = true;
                break;
            }
        }
        if (found) return returnObject;
        else return null;
    }

    public void tryLevel()
    {
        buildMaps();
        string mapString = getArrayString(currentMap);
        string placeablesString = getArrayString(currentPlaceables);
        controller.map = currentMap;
        controller.placeables = currentPlaceables;

        print("tunnels: " + mapString);
        print("placeables: " + placeablesString);

        saveMapLocaly();
        SceneManager.LoadScene(1);
    }

    public void buildMaps()
    {
        GameObject[] segments = GameObject.FindGameObjectsWithTag("Segment");
        GameObject[] placeables = GameObject.FindGameObjectsWithTag("Placeable");
        int modelCount = 0;
        int placeableCount = 0;
        placeableCount = placeables.Length;

        for (var a = 0; a < segments.Length; a++)
        {
            if (segments[a].GetComponent<Segment>().holder != null) modelCount++;
        }

        currentMap = new int[modelCount, 4];

        for (int a = 0, mapPos = 0; a < segments.Length; a++)
        {
            if (segments[a].gameObject.GetComponent<Segment>().holder != null)
            {
                int[] coords = new int[4];

                for (int b = 0; b < 4; b++)
                {
                    currentMap[mapPos, b] = segments[a].gameObject.GetComponent<Segment>().holder.GetComponent<Model>().coords[b];
                }
                mapPos++;
            }
        }

        currentPlaceables = new int[placeableCount, 4];

        for (int a = 0, placePos = 0; a < placeables.Length; a++)
        {
            int[] coords = new int[4];
            for (int b = 0; b < 4; b++)
            {
                currentPlaceables[placePos, b] = placeables[a].gameObject.GetComponent<Model>().coords[b];
            }
            placePos++;
        }

    }

    public string getArrayString(int[,] request)
    {
        string mapString = "{";
        for (int a = 0; a < request.GetLength(0); a++)
        {
            int innerLength = request.GetLength(1);
            mapString += "{";
            for (int b = 0; b < innerLength; b++)
            {
                mapString += request[a, b];
                if (b != innerLength - 1) mapString += ", ";
            }
            mapString += "}";
            if (a != request.GetLength(0) - 1) mapString += ", ";
        }
        mapString += "}";
        return mapString;
    }

    public void saveMapLocaly()
    {

        if (!Directory.Exists(mapsPath))
        {
            Directory.CreateDirectory(mapsPath);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(mapsPath + "map1.dat");

        MapData data = new MapData();
        data.mData = currentMap;
        data.pData = currentPlaceables;

        bf.Serialize(file, data);
        file.Close();
    }


    public void loadLocaly()
    {
        if(File.Exists(mapsPath + "map1.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mapsPath + "map1.dat", FileMode.Open);
            MapData data = bf.Deserialize(file) as MapData;
            file.Close();

            currentMap = data.mData;
            currentPlaceables = data.pData;
        }
        else
        {
            print("file not found");
        }
    }



    [Serializable]
    class MapData
    {
        public int[,] mData;
        public int[,] pData;
    }


}
