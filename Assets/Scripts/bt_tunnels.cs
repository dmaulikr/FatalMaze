using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class bt_tunnels : MonoBehaviour, IPointerDownHandler
{
    public string select = "tunnels";
    public void OnPointerDown(PointerEventData eventData)
    {
        EditorUI.editorUI.buildButtons(select);
    }
}
