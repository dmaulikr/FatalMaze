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
    public Ray ray;
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
    private string[,] currentMap;
    private string[,] currentPlaceables;
    private string mapsPath;
    private GameObject tunnels;
    private GameObject placeables;
    private List<GameObject> mapPlaceables = new List<GameObject>();

    void Start()
    {
        mainCamera = this;
        controller = MainController.mainController;
        tunnels = GameObject.Find("Tunnels");
        placeables = GameObject.Find("Placeables");
        mapsPath = Application.dataPath + "/maps/";
        camera = transform.GetComponent<Camera>();
        defaultSlide = slideSpeed;
        segmentList = GameObject.Find("LineRenderer").GetComponent<LineRendererScript>().segmentList;

        //loadLocaly();
    }


    void Update()
    {

        // each frame we send a RayCast from camera
        ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {

            if(objectHit != hit.transform) // if we hit a new object
            {
                if (objectHit != null && selectedObject != null && (objectHit.tag == "SegmentTunnel" || objectHit.tag == "SegmentRoom")) objectHit.GetComponent<MeshRenderer>().enabled = false;
                objectHit = hit.transform;
                if ((objectHit.tag == "SegmentTunnel" && selectedObject != null && (selectedObject.tag == "Tunnel" || selectedObject.tag == "Eraser") || (objectHit.tag == "SegmentRoom" && selectedObject != null && (selectedObject.tag == "Room" || selectedObject.tag == "Eraser")))) objectHit.GetComponent<MeshRenderer>().enabled = true;
            }

            if (selectedObject != null && selectedObject.tag != "Eraser" && mousePosition[0] <= screenWidth - 205) selectedObject.transform.position = new Vector3((int)Mathf.Floor(hit.point.x), selectedObject.transform.position.y, (int)Mathf.Ceil(hit.point.z)); //updating selected object position

            if (selectedObject == null && (objectHit.tag == "SegmentTunnel" || objectHit.tag == "SegmentRoom")) objectHit.GetComponent<MeshRenderer>().enabled = false;

        }

        if (selectedObject != null) selectedPosition = selectedObject.transform.position; // Setting selected position var

        //UI//
        if (selectedObject != null && selectedObject.tag != "Placeable" )
        {
            if (Input.GetMouseButton(0)) // if it's not placeable
            {
                putObject();
            }
        }
        else if(selectedObject != null && selectedObject.tag == "Placeable")
        {
            if (Input.GetMouseButtonDown(0)) // if it is a placeable, we look for mouse button up
            {
                putObject();
            }
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
        if (Input.GetKeyDown("t") || Input.GetAxis("Mouse ScrollWheel") < 0f && selectedObject != null && selectedObject.tag == "Placeable") //Zoom)
        {
            if(selectedObject.GetComponent<Model>().freeRotation) selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, (int)(selectedObject.transform.localEulerAngles.y + 10f + 10f - selectedObject.transform.localEulerAngles.y%10), 0.0f));
            else selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, (int)(selectedObject.transform.localEulerAngles.y + 90f), 0.0f));
        }
        if (Input.GetKeyDown("r") || Input.GetAxis("Mouse ScrollWheel") > 0f && selectedObject != null && selectedObject.tag == "Placeable") //Zoom)
        {
            if (selectedObject.GetComponent<Model>().freeRotation) selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, (int)(selectedObject.transform.localEulerAngles.y - 10f - selectedObject.transform.localEulerAngles.y % 10), 0.0f));
            else selectedObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, (int)(selectedObject.transform.localEulerAngles.y - 90f), 0.0f));
        }


        // zooming
        if(Input.GetAxis("Mouse ScrollWheel") > 0f && (selectedObject == null || selectedObject.tag != "Placeable")) //Zoom
        {
            if(transform.position.y > 5f) transform.Translate(Vector3.forward * scrollSpeed);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && (selectedObject == null || selectedObject.tag != "Placeable")) //Zoom out
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
            if((selectedObject.tag == "Tunnel" && objectHit.tag == "SegmentTunnel") || (selectedObject.tag == "Room" && objectHit.tag == "SegmentRoom")) // put object
            {
                objectHit.GetComponent<SegmentTunnel>().type = selectedIdShort;
                updateSegments(objectHit.transform);
            }
            if (selectedObject.tag == "Eraser" && (objectHit.tag == "SegmentTunnel" || objectHit.tag == "SegmentRoom")) // delete object
            {
                objectHit.GetComponent<SegmentTunnel>().type = null;
                updateSegments(objectHit.transform);
            }
            else if (selectedObject.tag == "Placeable") // put placeable
            {
                selectedObject.transform.parent = placeables.transform;
                selectedObject.GetComponent<Model>().saveStats();
                mapPlaceables.Add(selectedObject);

                selectedObject = null;
                selectObject(selectedIdFull);
            }
        }

    }

    public void updateSegments(Transform collider)
    {
        Collider[] colliders = Physics.OverlapSphere(collider.position, 1); // Segments colliding with current segment

        bool found = false;
        for(int a = 0; a < colliders.Length; a++)
        {
            if(colliders[a].gameObject.tag == "Segment")
            {
                if (colliders[a].gameObject.GetComponent<Segment>().updateSegment(collider, true)) found = true; // first we check if tile with current code exists, if it does we set found true
            }
        }

        if(found)
        {
            for (int a = 0; a < colliders.Length; a++)
            {
                if (colliders[a].gameObject.tag == "Segment")
                {
                    colliders[a].gameObject.GetComponent<Segment>().updateSegment(collider, false); // this will actually instantiate the correct tile
                }
            }
        }

    }

    public void deleteObject()
    {
        if(selectedObject != null)
        {
            Destroy(selectedObject);
        }
    }

    public void selectObject(string modelId)
    {
        if (selectedObject != null) Destroy(selectedObject.gameObject); // destroy previously selected object

        if (objectHit.tag == "SegmentTunnel" || objectHit.tag == "SegmentRoom") objectHit.GetComponent<MeshRenderer>().enabled = false;

        if (modelId == "eraser")
        {
            GameObject eraser = new GameObject();
            eraser.tag = "Eraser";
            selectedObject = eraser;
            return;
        }

        string tag = "";
        if (findObject(controller.allTunnels, modelId))
        {
            tag = findObject(controller.allTunnels, modelId).gameObject.tag;
        }
        else if(findObject(controller.allPlaceables, modelId))
        {
            tag = findObject(controller.allPlaceables, modelId).gameObject.tag;
        }
        else if (findObject(controller.allRooms, modelId))
        {
            tag = findObject(controller.allRooms, modelId).gameObject.tag;
        }

        if(tag == "Tunnel")
        {
            if (findObject(controller.allTunnels, modelId))
            {
                GameObject tunnel = findObject(controller.allTunnels, modelId);
                GameObject tunnelClone = Instantiate(tunnel, tunnel.transform.position, tunnel.transform.rotation) as GameObject;
                tunnelClone.GetComponentInChildren<MeshRenderer>().enabled = false;
                tunnelClone.GetComponent<Collider>().enabled = false;
                selectedObject = tunnelClone;
                selectedIdShort = selectedObject.GetComponent<Model>().shortCode;
            }
        }
        else if (tag == "Room")
        {
            if (findObject(controller.allRooms, modelId))
            {
                GameObject room = findObject(controller.allRooms, modelId);
                GameObject roomClone = Instantiate(room, room.transform.position, room.transform.rotation) as GameObject;
                roomClone.GetComponentInChildren<MeshRenderer>().enabled = false;
                roomClone.GetComponent<Collider>().enabled = false;
                selectedObject = roomClone;
                selectedIdShort = selectedObject.GetComponent<Model>().shortCode;
            }
        }
        else if(tag == "Placeable")
        {
            if (findObject(controller.allPlaceables, modelId))
            {
                GameObject placeable = findObject(controller.allPlaceables, modelId);
                GameObject placeableClone = Instantiate(placeable, placeable.transform.position, placeable.transform.rotation) as GameObject;
                selectedObject = placeableClone;
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
            if (list[a].GetComponent<SegmentTunnel>() && list[a].GetComponent<SegmentTunnel>().id == id)
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

        string mapString = getArrayString(currentMap); //Converts current map list to string array
        string placeablesString = getArrayString(currentPlaceables); // same
        controller.map = currentMap;
        controller.placeables = currentPlaceables;

        print("tunnels: " + mapString);
        print("placeables: " + placeablesString);

        saveMapLocaly();
        SceneManager.LoadScene(1);
    }

    public void buildMaps()
    {
        int modelCount = 0;
        int placeableCount = 0;
        placeableCount = mapPlaceables.Count;

        for (var a = 0; a < segmentList.Count; a++)
        {
            if (segmentList[a].GetComponent<Segment>().holder != null) modelCount++; // getting model count
        }

        currentMap = new string[modelCount, 4];

        for (int a = 0, mapPos = 0; a < segmentList.Count; a++) // build map array
        {
            if (segmentList[a].gameObject.GetComponent<Segment>().holder != null)
            {
                int[] coords = new int[4];

                for (int b = 0; b < 4; b++)
                {
                    currentMap[mapPos, b] = segmentList[a].gameObject.GetComponent<Segment>().holder.GetComponent<Model>().coords[b];
                }
                mapPos++;
            }
        }

        currentPlaceables = new string[placeableCount, 4];

        for (int a = 0, placePos = 0; a < mapPlaceables.Count; a++) // build placeables array
        {
            int[] coords = new int[4];
            for (int b = 0; b < 4; b++)
            {
                currentPlaceables[placePos, b] = mapPlaceables[a].gameObject.GetComponent<Model>().coords[b];
            }
            placePos++;
        }

    }

    public string getArrayString(string[,] request)
    {
        string mapString = "{";
        for (int a = 0; a < request.GetLength(0); a++)
        {
            int innerLength = request.GetLength(1);
            mapString += "{\"";
            for (int b = 0; b < innerLength; b++)
            {
                mapString += request[a, b];
                if (b != innerLength - 1) mapString += "\", \"";
            }
            mapString += "\"}";
            if (a != request.GetLength(0) - 1) mapString += ", ";
        }
        mapString += "}";
        return mapString;
    }

    public void saveMapLocaly()
    {

        if (!Directory.Exists(mapsPath)) // if maps directory doesn't exist, create a new one
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
        public string[,] mData;
        public string[,] pData;
    }


}
