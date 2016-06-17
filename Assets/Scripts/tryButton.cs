using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class tryButton : MonoBehaviour, IPointerDownHandler
{
    public int modelNumber;

    public void OnPointerDown(PointerEventData eventData)
    {
        CameraControl.mainCamera.tryLevel();
    }

}
