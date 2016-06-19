using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class editorButton : MonoBehaviour, IPointerDownHandler
{
    public int modelNumber;
    public int loadScene;

    public void OnPointerDown(PointerEventData eventData)
    {
        SceneManager.LoadScene(loadScene);
    }

}
