using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class editorButton : MonoBehaviour, IPointerDownHandler
{
    public int modelNumber;
    public int loadScene;
    public int gameMode = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        MainController.mainController.currentGameType = gameMode; // sets if it's standalone or cardboard
        MainController.mainController.updateGameMode();
        SceneManager.LoadScene(loadScene);
    }

}
