using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapPoint : MonoBehaviour 
{

    public string forObject;
    private bool taken = false;
    private GameObject previousHolder;

	private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Placeable" && forObject == other.GetComponent<Model>().code && !taken)
        {
            other.GetComponent<Model>().isSnapped = true;
            other.transform.position = transform.position;
            taken = true;
            previousHolder = other.gameObject;
        }
        if(previousHolder == null)
        {
            taken = false;
        }
    }

}
