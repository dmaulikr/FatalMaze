using UnityEngine;
using System.Collections;

public class Hurricane : MonoBehaviour 
{
    public bool movingCloser = true;
    public bool gettingSmaller = false;
    private Vector3 direction = Vector3.back;
    public float dissapearSpeed = 0.001f;
    public float appearSpeed = 0.01f;
    public float scaleSpeed = -0.001f;

    private float rotationSpeed;
    public float moveSpeed = 2f;

    private float rValue = 0;
    private float gValue = 0;
    private float bValue = 0;
    private float rValueMax = 0;
    private float gValueMax = 0;
    private float bValueMax = 0;
    private bool rValueMaxReached = false;
    private bool gValueMaxReached = false;
    private bool bValueMaxReached = false;

    private Renderer rend;

    void Start()
    {
        GameObject childHuricane = transform.FindChild("Hurricane").gameObject;
        rend = childHuricane.GetComponent<Renderer>();
        rend.material.SetColor("_TintColor", new Color(rValue, gValue, bValue));

        if(!movingCloser)
        {
            direction = Vector3.forward;
        }

        rotationSpeed = Random.RandomRange(15f, 35f);
        rValueMax = Random.RandomRange(0.4f, 0.9f);
        gValueMax = Random.RandomRange(0.4f, 0.9f);
        bValueMax = Random.RandomRange(0.4f, 0.9f);
    }

    void Update()
    {
        if(bValueMaxReached)
        {
            bValue -= dissapearSpeed;
        }
        else
        {
            bValue += appearSpeed;
        }

        if(rValueMaxReached)
        {
            rValue -= dissapearSpeed;
        }
        else
        {
            rValue += appearSpeed;
        }

        if(gValueMaxReached)
        {
            gValue -= dissapearSpeed;
        }
        else
        {
            gValue += appearSpeed;
        }

        if(bValue >= gValueMax)
        {
            bValueMaxReached = true;
        }
        if(rValue >= rValueMax)
        {
            rValueMaxReached = true;
        }
        if(gValue >= gValueMax)
        {
            gValueMaxReached = true;
        }

        if(bValue < 0 && rValue < 0 && gValue < 0)
        {
            Destroy(gameObject);
        }
    }

	void FixedUpdate () 
    {
        if(gettingSmaller)
        {
            transform.localScale += new Vector3(scaleSpeed, scaleSpeed, 0f);
        }
        transform.localEulerAngles = transform.localEulerAngles += Vector3.forward * Time.deltaTime * rotationSpeed;
        transform.Translate(direction * Time.deltaTime * moveSpeed);
        rend.material.SetColor("_TintColor", new Color(rValue, gValue, bValue));
	}
}
