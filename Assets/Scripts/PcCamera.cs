using UnityEngine;
using System.Collections;

public class PcCamera : MonoBehaviour 
{
    public float xRotationEdge = 360;
    public Vector2 yRotationEdge = new Vector2(80, 280);
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);

    private Vector2 smoothMouse;
    private Vector2 absoluteMouse;
    private Vector2 targetDirection;
    private Quaternion targetOrientation;

	// Use this for initialization
	void Start () 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetDirection = transform.localRotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () 
    {

        // Allow the script to clamp based on a desired target value.
        targetOrientation = Quaternion.Euler(targetDirection);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero.
        absoluteMouse += smoothMouse;

        //rotate
        transform.localRotation = Quaternion.AngleAxis(absoluteMouse.x, targetOrientation * Vector3.up) * targetOrientation;
        transform.localRotation *= Quaternion.AngleAxis(absoluteMouse.y, targetOrientation * Vector3.left) * targetOrientation;
    }


}
