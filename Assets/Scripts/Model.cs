using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Model : MonoBehaviour 
{
    public string code;
    [System.NonSerialized]
    public string shortCode;
    public string name;
    [System.NonSerialized]
    public string[] coords = new string[4];
    public Sprite image;
    private string idOnly;
    private CastInto castInto = new CastInto();
    public bool freeRotation = true;
    public bool pickable = false;

    void Awake()
    {
        shortCode = castInto.codeToShortCode(code);
    }

    public void saveStats()
    {
        coords[0] = code;
        coords[1] = ((int)(transform.position.x)).ToString();
        coords[2] = ((int)(transform.position.z)).ToString();
        coords[3] = ((int)(transform.localEulerAngles.y)).ToString();
    }

}
