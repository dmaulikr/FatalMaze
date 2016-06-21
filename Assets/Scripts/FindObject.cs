using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindObject
{

    public GameObject FindObjectByCode(List<GameObject> list, string id)
    {
        GameObject returnObject = list[0];
        bool found = false;

        for(int a = 0; a < list.Count; a++)
        {
            if(list[a].GetComponent<Model>().code == id)
            {
                returnObject = list[a];
                found = true;
                break;
            }
        }
        if (found) return returnObject;
        else return null;
    }
}
