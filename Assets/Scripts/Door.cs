using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour 
{
    public string objectRequired = "p2";
    public GameObject keyHole;
    public GameObject arrow;
    public float animDelay = 3;
    public List<string> animations = new List<string>();
    public Collider doorCollider;
    public Collider closedDoorCollider;
    public Collider openedDoorCollider;
    public bool openedDoor = false;


    private bool opened = false;


    void Awake()
    {
        if (MainController.mainController.currentScene != 1)
        {
            Destroy(transform.GetComponent<Door>());
        }
    }

    void Start()
    {
        if (openedDoor)
        {
            closedDoorCollider.enabled = false;
            playAnim(animations[0]);
            Destroy(arrow);
        }
        else
        {
            openedDoorCollider.enabled = false;
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
                other.GetComponent<PickItem>().openLater(this.gameObject, animations[0], animDelay);
                Destroy(arrow);
            }
        }
        if (openedDoor && other.tag == "PlayerBody")
        {
            playAnim(animations[1], true);
        }
    }

    public void playAnim(string anim, bool colliderEnable = false)
    {
        transform.GetComponent<Animation>().Play(anim);
        opened = true;
        if (!colliderEnable)
        {
            doorCollider.enabled = false;
        }
        else
        {
            doorCollider.enabled = true;
        }

    }

}
