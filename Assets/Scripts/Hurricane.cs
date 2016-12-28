using UnityEngine;
using System.Collections;

public class Hurricane : MonoBehaviour 
{
    private float rotationSpeed;
    private float moveSpeed = 2f;

    private float rValue;
    private float gValue;
    private float bValue;

    private Renderer rend;

    void Start()
    {
        GameObject childHuricane = transform.FindChild("Hurricane").gameObject;
        rend = childHuricane.GetComponent<Renderer>();

        rotationSpeed = Random.RandomRange(15f, 25f);
        rValue = Random.RandomRange(0.5f, 1f);
        gValue = Random.RandomRange(0.5f, 1f);
        bValue = Random.RandomRange(0.5f, 1f);
    }

    void Update()
    {
        bValue -= 0.001f;
        rValue -= 0.001f;
        gValue -= 0.001f;

        if(bValue < 0 && rValue < 0 && gValue < 0)
        {
            Destroy(gameObject);
        }
    }

	void FixedUpdate () 
    {
        transform.localEulerAngles = transform.localEulerAngles += Vector3.forward * Time.deltaTime * rotationSpeed;
        transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
        rend.material.SetColor("_TintColor", new Color(rValue, gValue, bValue));
	}
}
