﻿using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Model : MonoBehaviour 
{
    public string code;
    [System.NonSerialized]
    public string shortCode;
    public string name;
    [System.NonSerialized]
    public string[] coords = new string[5];
    public Sprite image;
    private string idOnly;
    [System.NonSerialized]
    public int idOnlyInt;
    private CastInto castInto = new CastInto();
    public bool freeRotation = true;
    public bool pickable = false;
    [System.NonSerialized]
    public bool isSnapped = false;
    public bool snappable = false;
    public bool shouldOptimize = true;

    void Awake()
    {
        shortCode = castInto.codeToShortCode(code);
    }

    void Start()
    {
        if (MainController.mainController.currentScene == 1)
        {
            for (int a = 0; a < transform.childCount; a++)
            {
                if (transform.GetChild(a).GetComponent<MeshRenderer>())
                {
                    transform.GetChild(a).GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }

    public void saveStats()
    {
        coords[0] = code;
        coords[1] = ((float)(System.Math.Round(transform.position.x, 2))).ToString();
        coords[2] = ((float)(System.Math.Round(transform.position.y, 2))).ToString();
        coords[3] = ((float)(System.Math.Round(transform.position.z, 2))).ToString();
        coords[4] = ((int)(transform.localEulerAngles.y)).ToString();
    }

}
