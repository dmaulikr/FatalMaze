using UnityEngine;
using System.Collections;

public class HideWhenNotSeen : MonoBehaviour 
{

    void OnTriggerEnter(Collider other)
    {
        toogleMesh(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        toogleMesh(other, false);
    }

    void toogleMesh(Collider other, bool toogle)
    {
        for(int a = 0; a < other.transform.childCount; a++)
        {
            if (other.transform.GetChild(a).GetComponent<MeshRenderer>())
            {
                other.transform.GetChild(a).GetComponent<MeshRenderer>().enabled = toogle;
            }
        }
    }


}
