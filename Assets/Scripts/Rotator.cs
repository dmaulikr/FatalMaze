using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{

    public float speed = 2f;
    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;
	

	void FixedUpdate () 
    {
        if (rotateX) transform.localEulerAngles += Vector3.right * Time.deltaTime * speed;
        if (rotateY) transform.localEulerAngles += Vector3.up * Time.deltaTime * speed;
        if (rotateZ) transform.localEulerAngles += Vector3.forward * Time.deltaTime * speed;
	}
}
