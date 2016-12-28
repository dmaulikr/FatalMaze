using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardboardReadyScreen : MonoBehaviour 
{

    public int timeToWait = 10;
    public MainController mainController;
    public Text statusText;
    // 0 - pc, 1 - cardboard
    public int forGameType;
	void Start () 
    {
        mainController = MainController.mainController;
        if(mainController.currentGameType != forGameType)
        {
            Destroy(gameObject);
        }
        StartCoroutine(StartGame());	
	}

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);

        timeToWait--;

        if (mainController.isReady)
        {
            statusText.text = "Ready in: " + timeToWait + " seconds.";
        }

        if (timeToWait > 0)
        {
            StartCoroutine(StartGame());
        }
        else if (mainController.isReady)
        {
            mainController.addGameCamera();
            mainController.isPlaying = true;
            StartCoroutine(DelayedDestroy());
        }

    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
