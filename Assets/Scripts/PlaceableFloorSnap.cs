using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlaceableFloorSnap : MonoBehaviour 
{
	void Start () 
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if(currentScene == 1)
        {
            Destroy(this);
        }

        transform.GetComponent<BoxCollider>().isTrigger = true;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Placeable" && other.transform.GetComponent<BoxCollider>())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + other.transform.GetComponent<BoxCollider>().size.y - 1f, transform.position.z);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Placeable" && other.transform.GetComponent<BoxCollider>())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - other.transform.GetComponent<BoxCollider>().size.y + 1f, transform.position.z);
        }
    }

    void OnDestroy()
    {
        transform.GetComponent<BoxCollider>().isTrigger = false;
    }
}
