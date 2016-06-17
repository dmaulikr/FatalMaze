using UnityEngine;
using System.Collections;

public class Model : MonoBehaviour 
{
    public string code;
    public int id;
    public string shortCode;
    public string name;
    public int[] coords = new int[4];
    public Sprite image;

    void Start()
    {

    }

    public void saveStats()
    {
        coords[0] = id;
        coords[1] = (int)transform.position.x;
        coords[2] = (int)transform.position.z;
        coords[3] = (int)transform.localEulerAngles.y;
    }

}
