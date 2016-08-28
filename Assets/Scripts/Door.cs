using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
    public string objectRequired = "p2";
    public GameObject keyHole;
    public GameObject arrow;
    public float animDelay = 3;
    public string firstAnim;
    public Collider doorCollider;


    private bool opened = false;


    void Awake()
    {
        if(MainController.mainController.currentScene != 1)
        {
            Destroy(transform.GetComponent<Door>());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Placeable" && other.GetComponent<PickItem>() && !opened)
        {

            if(objectRequired == other.GetComponent<Model>().code)
            {
                other.GetComponent<PickItem>().followSpeed = 1;
                other.GetComponent<PickItem>().objectToFollow = keyHole;
                other.GetComponent<PickItem>().openLater(this.gameObject, firstAnim, animDelay);
                Destroy(arrow);
            }
        }
    }

    public void playAnim(string anim)
    {
        transform.GetComponent<Animation>().Play(anim);
        doorCollider.enabled = false;
        opened = true;
    }

}
