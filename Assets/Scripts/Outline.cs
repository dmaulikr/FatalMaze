using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Outline : MonoBehaviour
{
    public Color c1 = Color.green;
    [System.NonSerialized]
    public Vector3[] positions = new Vector3[5];
    [System.NonSerialized]
    public float width;
    public Material lineMaterial;

    // Use this for initialization
    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(width, width);
        lineRenderer.SetVertexCount(positions.Length);

        lineRenderer.SetPositions(positions);

    }
}
