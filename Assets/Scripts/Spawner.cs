using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public GameObject objectToSpawn;
    public float interval;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(SpawnObject());
	}
	
    IEnumerator SpawnObject()
    {
        GameObject objectClone = Instantiate(objectToSpawn, transform.position, transform.rotation) as GameObject;
        yield return new WaitForSeconds(interval);
        StartCoroutine(SpawnObject());
    }

}
