using UnityEngine;
using System.Collections;

public class materialColor : MonoBehaviour 
{
    public Color col = Color.green;
    private Renderer renderer;

	// Use this for initialization
	void Start ()
    {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = col;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
