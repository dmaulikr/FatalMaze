using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour 
{
    public string objectRequired = "p2";
    public List<AudioClip> soundList;
    public GameObject keyHole;
    public GameObject arrow;
    public float animDelay = 3;
    public List<string> animations = new List<string>();
    public Collider doorCollider;
    public Collider openDoorCollider;
    public bool openedDoor = false;
    private bool opened = false;
    private AudioSource audio;


    void Awake()
    {
        if (MainController.mainController.currentScene != 1)
        {
            Destroy(transform.GetComponent<Door>());
            openDoorCollider.enabled = false;
        }
    }

    void Start()
    {
        Destroy(arrow);
        if(GetComponent<AudioSource>())
        {
            audio = GetComponent<AudioSource>();
        }
        else
        {
            audio = new AudioSource();
        }
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Placeable" && other.GetComponent<PickItem>() && !opened)
        {
            if(objectRequired == other.GetComponent<Model>().code)
            {
                other.GetComponent<PickItem>().followSpeed = 1;
                other.GetComponent<PickItem>().objectToFollow = keyHole;
                other.GetComponent<PickItem>().openLater(this.gameObject, animations[0], animDelay, 0);
            }
        }
        if (other.tag == "PlayerBody" && openedDoor)
        {
            playAnim(animations[0], false, 0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (openedDoor && other.tag == "PlayerBody")
        {
            playAnim(animations[1], true, 1);
        }
    }

    public void playAnim(string anim, bool colliderEnable = false, int soundIndex = 0)
    {
        transform.GetComponent<Animation>().Play(anim);
        opened = true;

        audio.clip = soundList[soundIndex];
        audio.Play();

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
