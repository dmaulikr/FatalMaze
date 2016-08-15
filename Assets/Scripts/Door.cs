using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
    public string objectRequired = "p2";
    public GameObject keyHole;
    public float animDelay = 3;

    private string firstAnim = "Door1Open";

    void Awake()
    {
        if(MainController.mainController.currentScene != 1)
        {
            Destroy(transform.GetComponent<Door>());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Placeable" && other.GetComponent<PickItem>())
        {

            if(objectRequired == other.GetComponent<Model>().code)
            {
                other.GetComponent<PickItem>().followSpeed = 1;
                other.GetComponent<PickItem>().objectToFollow = keyHole;
                other.GetComponent<PickItem>().openLater(this.gameObject, firstAnim, animDelay);

                if(other.transform.eulerAngles.y < 120 || other.transform.eulerAngles.y > 260) // if the player comes from another side of the door we have to change keyHole rotation
                {
                    keyHole.transform.localEulerAngles += new Vector3(0, 180f, 0);
                }
            }
        }
    }

    public void playAnim(string anim)
    {
        transform.GetComponent<Animation>().Play(anim);
    }

}
