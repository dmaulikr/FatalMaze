using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
    public string objectRequired = "p2";
    public GameObject keyHole;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Placeable" && other.GetComponent<PickItem>())
        {
            print(other.tag + " - " + other.GetComponent<Model>().code);
            if(objectRequired == other.GetComponent<Model>().code)
            {
                other.GetComponent<PickItem>().followSpeed = 1;
                other.GetComponent<PickItem>().objectToFollow = keyHole;
            }
        }
    }

}
