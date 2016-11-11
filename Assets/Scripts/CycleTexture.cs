using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CycleTexture : MonoBehaviour 
{
	public float speed = 0.5f;
    public List<Texture> textures = new List<Texture>();
    public GameObject objectToTexture;
    private float index = 0;

    void Update() 
    {
        index += speed;
        if (index >= textures.Count) index = 0;
        objectToTexture.GetComponent<MeshRenderer>().material.mainTexture = textures[(int)index];
    }

}
