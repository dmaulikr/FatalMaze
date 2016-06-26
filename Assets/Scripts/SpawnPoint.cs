using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour 
{
    public GameObject spawn;

	void Start () 
    {
        Instantiate(spawn, transform.position, transform.rotation);
        Destroy(gameObject);
	}

}
