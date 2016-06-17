using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        Destroy(GetComponent<MeshRenderer>());
	}

}
