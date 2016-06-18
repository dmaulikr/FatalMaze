using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class CastInto
{
    public int codeToId(string code)
    {
        int value;
        Regex rgx = new Regex(@"[^\d]");
        int.TryParse(rgx.Replace(code, ""), out value);
        return value;
    }

    public string floatToString(float value)
    {
        return value.ToString();
    }

    public int stringToInt(string value)
    {
        int returnVal;
        int.TryParse(value, out returnVal);
        return returnVal;
    }

}
