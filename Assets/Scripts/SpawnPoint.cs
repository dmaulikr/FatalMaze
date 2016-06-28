using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour 
{
    public GameObject spawn;

	void Start () 
    {
        if(MainController.mainController.currentScene == 1)
        {
            Instantiate(spawn, transform.position, transform.rotation);
            Destroy(gameObject);
        }

	}

}
