using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapPoint : MonoBehaviour 
{
    private bool taken = false;
    private GameObject previousHolder;

	private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Placeable" && other.GetComponent<Model>().snappable && !taken && !other.GetComponent<Model>().isSnapped)
        {
            other.GetComponent<Model>().isSnapped = true;
            taken = true;
            previousHolder = other.gameObject;
            other.transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            if(other.transform.position.x != transform.position.x)
            {
                print("move");
                other.transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            }
        }
        if(previousHolder == null)
        {
            taken = false;
        }
    }

}
