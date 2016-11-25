using UnityEngine;
using System.Collections;

public class PickItem : MonoBehaviour 
{

    public bool picked = false;
    public GameObject objectToFollow;
    public int followSpeed = 5;

    private float countDown = 0;
    private bool beingUsed = false;
    private GameObject doorToOpen;
    private string doorAnim;
    private int soundIndex;

	void Update () 
    {
	    if(picked)
        {
            transform.position = Vector3.Lerp(transform.position, objectToFollow.transform.position, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, objectToFollow.transform.rotation, 5 * Time.deltaTime);
            if (followSpeed < 100) followSpeed++;
        }

        if(countDown > 0 && beingUsed)
        {
            countDown -= Time.deltaTime;
        }
        else if(beingUsed)
        {
            Destroy(transform.gameObject);
            doorToOpen.GetComponent<Door>().playAnim(doorAnim, false, soundIndex);
        }

	}

    public void removeScripts()
    {
        if (transform.GetComponent<Rotator>()) transform.GetComponent<Rotator>().enabled = false;
    }

    public void openLater(GameObject callbackObject, string animation, float countdown, int sound)
    {
        soundIndex = sound;
        countDown = countdown;
        beingUsed = true;
        doorToOpen = callbackObject;
        doorAnim = animation;
    }
}
