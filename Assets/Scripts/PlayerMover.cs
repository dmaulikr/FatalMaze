using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour 
{
    public float playerSpeed = 2f;
    private bool cameraFound = false;
    private GameObject mainCamera;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(!cameraFound && GameObject.FindWithTag("MainCamera"))
        {
            cameraFound = true;
            mainCamera = GameObject.FindWithTag("MainCamera");
        }
	}

    void FixedUpdate()
    {
        if(cameraFound)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            transform.rotation = Quaternion.Euler(0, mainCamera.transform.Find("Head").transform.localEulerAngles.y, 0);
        }

    }

}
