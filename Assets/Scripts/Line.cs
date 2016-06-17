using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour 
{
    public Color c1 = Color.green;
    public Vector3 pos1;
    public Vector3 pos2;
    public float width = 0.03f;
    public Material lineMaterial;

	// Use this for initialization
	void Start () 
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(width, width);
        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);
	}
}
