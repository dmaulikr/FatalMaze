using UnityEngine;
using System.Collections;

public class CreateCopy : MonoBehaviour {
    public string[] codes = { "rd32", "rd64", "rd128" };
    [System.NonSerialized]
    public int[] angles = { 0, 90, 180 };
	// Use this for initialization
	void Start ()
    {
        GameObject cloneObject = transform.gameObject;
        for (var a = 0; a < 3; a++)
        {
            GameObject a2 = Instantiate(transform.gameObject, transform.position, Quaternion.Euler(0f, angles[a], 0f)) as GameObject;
            a2.GetComponent<Model>().code = codes[a];
            a2.GetComponent<CreateCopy>().enabled = false;
            cloneObject = a2;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
