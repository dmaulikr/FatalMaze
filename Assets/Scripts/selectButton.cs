using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class selectButton : MonoBehaviour, IPointerUpHandler
{
    public string modelId;

    public void OnPointerUp(PointerEventData eventData)
    {
        EditorUI.editorUI.selectObject(modelId);
    }

}
