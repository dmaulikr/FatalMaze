using UnityEngine;
using System.Collections;

public class MouseRotate : MonoBehaviour
{
    public float turnSpeed = 50f;

	void Update () 
    {

        // Look right and left
        if (Input.GetAxis("Mouse X") < 0)
        {
            //transform.Rotate(Vector3.down, turnSpeed * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            //transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }


        //// Look up and down
        //if(Input.GetAxis("Mouse Y") < 0)
        //{
        //    transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime);
        //}
        //if(Input.GetAxis("Mouse Y") > 0)
        //{
        //    transform.Rotate(Vector3.left, turnSpeed * Time.deltaTime);
        //}
	}
}
