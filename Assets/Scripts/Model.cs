using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Model : MonoBehaviour 
{
    public string code;
    public string shortCode;
    public string name;
    public string[] coords = new string[4];
    public Sprite image;
    private string idOnly;
    private CastInto castInto;

    void Start()
    {
        castInto = new CastInto();
        //id = returnInt.codeToId(code);
    }

    public void saveStats()
    {
        coords[0] = code;
        coords[1] = ((int)(transform.position.x)).ToString();
        coords[2] = ((int)(transform.position.z)).ToString();
        coords[3] = ((int)(transform.localEulerAngles.y)).ToString();
    }

}
