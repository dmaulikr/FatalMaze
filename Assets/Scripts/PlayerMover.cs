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
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            if(mainCamera.transform.Find("Head")) transform.rotation = Quaternion.Euler(0, mainCamera.transform.Find("Head").transform.localEulerAngles.y, 0);
            else transform.rotation = Quaternion.Euler(0, mainCamera.transform.localEulerAngles.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Placeable" && other.GetComponent<Model>().pickable)
        {
            takeItem(other.gameObject);
        }
    }

    private void takeItem(GameObject item)
    {
        item.GetComponent<PickItem>().removeScripts();
        item.GetComponent<PickItem>().objectToFollow = transform.Find("Hand").gameObject;
        item.GetComponent<PickItem>().picked = true;
    }

}
