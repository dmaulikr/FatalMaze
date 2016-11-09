using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CreateCopy : MonoBehaviour {
    public string roomName = "Roomc";
    public string roomShortCode = "rd";
    private string[,] roomCodes = { { "16", "32", "64", "128" }, { "48", "96", "144", "192" }, { "240", "0", "0", "0" }, { "112", "176", "208", "224" }, { "80", "160", "0", "0" }, { "52", "104", "146", "193" }, { "18", "36", "72", "129" }, { "22", "44", "73", "131" }, { "20", "40", "65", "130" } };
    public List<string> roomNames;
    private int[] roomTimesCount = { 4, 4, 1, 4, 2, 4, 4, 4, 4 };
    public List<GameObject> roomObjects;
    public string roomChildName = "Tunnel1";
    [System.NonSerialized]
    private int[,] roomAngles = { { 270, 0, 90, 180 }, { 0, 90, 270, 180 }, { 0, 0, 0, 0 }, { 0, 270, 180, 90 }, { 90, 0, 0, 0 }, { 0, 90, 270, 180 }, { 270, 0, 90, 180 }, { 270, 0, 90, 180 }, { 270, 0, 90, 180 }};

    public string tunnelName = "Tunnelc";
    public string tunnelShortCode = "tc";
    private string[,] tunnelCodes = { { "1", "2", "4", "8" }, { "5", "10", "0", "0" }, { "7", "11", "13", "14" }, { "3", "6", "9", "12" }, { "15", "0", "0", "0" } };
    public List<string> tunnelNames;
    private int[] tunnelTimesCount = { 4, 2, 4, 4, 1};
    public List<GameObject> tunnelObjects;
    public string tunnelChildName = "Tunnel1";
    private int[,] tunnelAngles = { { 180, 270, 0, 90 }, { 0, 90, 0, 0 }, { 90, 0, 270, 180 }, { 0, 90, 270, 180 }, { 0, 0, 0, 0 } };

	void Start ()
    {
        GameObject cloneObject = transform.gameObject;

        for (var a = 1; a < 10; a++ )
        {
            roomNames.Add(roomName + (a));
        }

        for (var a = 1; a < 6; a++)
        {
            tunnelNames.Add(tunnelName + (a));
        }

        for (var b = 0; b < roomObjects.Count; b++)
        {
            for (var a = 0; a < roomTimesCount[b]; a++)
            {
                GameObject a2 = Instantiate(roomObjects[b], transform.position, Quaternion.Euler(0f, roomAngles[b, a], 0f)) as GameObject;
                a2.GetComponent<Model>().code = roomShortCode + roomCodes[b, a];
                a2.GetComponent<Model>().name = roomName;
                a2.tag = "Room";
                a2.isStatic = true;
                a2.transform.FindChild(roomChildName).GetComponent<MeshRenderer>().receiveShadows = false;
                a2.transform.FindChild(roomChildName).GetComponent<Renderer>().shadowCastingMode = 0;
                a2.transform.FindChild(roomChildName).gameObject.isStatic = false;
                a2.transform.position = new Vector3(0f, 0f, 0f);
                if (a2.GetComponent<CreateCopy>()) Destroy(a2.GetComponent<CreateCopy>());
                a2.name = roomNames[b] + " (" + roomShortCode + roomCodes[b, a] + ")";
                cloneObject = a2;
            }
        }

        for (var b = 0; b < tunnelObjects.Count; b++)
        {
            for (var a = 0; a < tunnelTimesCount[b]; a++)
            {
                GameObject a2 = Instantiate(tunnelObjects[b], transform.position, Quaternion.Euler(0f, tunnelAngles[b, a], 0f)) as GameObject;
                a2.GetComponent<Model>().code = tunnelShortCode + tunnelCodes[b, a];
                a2.GetComponent<Model>().name = tunnelName;
                a2.tag = "Tunnel";
                a2.transform.position = new Vector3(0f, 0f, 0f);
                a2.isStatic = true;
                a2.transform.FindChild(tunnelChildName).GetComponent<MeshRenderer>().receiveShadows = false;
                a2.transform.FindChild(tunnelChildName).GetComponent<Renderer>().shadowCastingMode = 0;
                a2.transform.FindChild(tunnelChildName).gameObject.isStatic = true;
                if (a2.GetComponent<CreateCopy>()) Destroy(a2.GetComponent<CreateCopy>());
                a2.name = tunnelNames[b] + " (" + tunnelShortCode + tunnelCodes[b, a] + ")";
                cloneObject = a2;
            }
        }

        Destroy(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
