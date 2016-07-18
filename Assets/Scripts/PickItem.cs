using UnityEngine;
using System.Collections;

public class PickItem : MonoBehaviour 
{

    public bool picked = false;
    public GameObject objectToFollow;
    public int followSpeed = 5;

	void Update () 
    {
	    if(picked)
        {
            transform.position = Vector3.Lerp(transform.position, objectToFollow.transform.position, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, objectToFollow.transform.rotation, 5 * Time.deltaTime);
            if (followSpeed < 100) followSpeed++;
        }
	}

    public void removeScripts()
    {
        if (transform.GetComponent<Rotator>()) transform.GetComponent<Rotator>().enabled = false;
    }

}
