using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour 
{
    public float playerSpeed = 2f;
    private bool cameraFound = false;
    private GameObject mainCamera;
    private bool moving = true;
    private MainController mainController;

	// Use this for initialization
	void Start () 
    {
        mainController = MainController.mainController;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(!cameraFound && GameObject.FindWithTag("MainCamera"))
        {
            cameraFound = true;
            mainCamera = GameObject.FindWithTag("MainCamera");
        }
        if(Input.GetMouseButtonDown(0))
        {
            moving = !moving;
        }
	}

    void FixedUpdate()
    {
        if(cameraFound && moving)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
            if(mainCamera.transform.Find("Head")) transform.rotation = Quaternion.Euler(0, mainCamera.transform.Find("Head").transform.localEulerAngles.y, 0);
            else transform.rotation = Quaternion.Euler(0, mainCamera.transform.localEulerAngles.y, 0);
            if(mainController.isPlaying)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Placeable" && other.GetComponent<Model>().pickable && !other.GetComponent<PickItem>().picked)
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
