using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CreateCopy : MonoBehaviour {
    public string name = "Roomc";
    public string shortCode = "rd";
    private string[,] codes = { { "16", "32", "64", "128" }, { "48", "96", "144", "192" }, { "240", "0", "0", "0" }, { "112", "176", "208", "224" }, { "80", "160", "0", "0" }, { "52", "104", "146", "193" }, { "18", "36", "72", "129" }, { "22", "44", "73", "131" }, { "20", "40", "65", "130" } };
    public List<string> names;
    private int[] timesCount = { 4, 4, 1, 4, 2, 4, 4, 4, 4 };
    public List<GameObject> objects;
    [System.NonSerialized]
    private int[,] roomAngles = { { 270, 0, 90, 180 }, { 0, 90, 270, 180 }, { 0, 0, 0, 0 }, { 0, 270, 180, 90 }, { 90, 0, 0, 0 }, { 0, 90, 270, 180 }, { 270, 0, 90, 180 }, { 270, 0, 90, 180 }, { 270, 0, 90, 180 }};
	// Use this for initialization
	void Start ()
    {
        GameObject cloneObject = transform.gameObject;

        for (var a = 1; a < 10; a++ )
        {
            names.Add(name + (a));
        }

        for (var b = 0; b < objects.Count; b++)
        {
            for (var a = 0; a < timesCount[b]; a++)
            {
                GameObject a2 = Instantiate(objects[b], transform.position, Quaternion.Euler(0f, roomAngles[b, a], 0f)) as GameObject;
                a2.GetComponent<Model>().code = shortCode + codes[b, a];
                a2.GetComponent<Model>().name = name;
                if (a2.GetComponent<CreateCopy>()) Destroy(a2.GetComponent<CreateCopy>());
                a2.name = names[b] + " (" + shortCode + codes[b, a] + ")";
                cloneObject = a2;
            }
        }

        Destroy(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
