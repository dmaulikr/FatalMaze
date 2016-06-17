﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditorUI : MonoBehaviour 
{
    public static EditorUI editorUI;
    public List<string> availableTunnels;
    public List<string> availablePlaceables;
    public Button modelButton;
    private List<GameObject> tunnelList;
    private List<Sprite> imageList;
    [System.NonSerialized]
    public bool panelVisible = false;
    public GameObject panel;
    private GameObject[] currentButtons = new GameObject[10];

	void Start () 
    {
        editorUI = this;
        tunnelList = MainController.mainController.allTunnels;
        imageList = MainController.mainController.images.GetComponent<Images>().images;
        //buildButtons();
	}

    void Update()
    {

    }

    public void buildButtons(string type = "tunnels")
    {
        panel.gameObject.SetActive(true);
        panelVisible = true;
        int bt_width = (int)modelButton.GetComponent<RectTransform>().rect.width;
        int bt_height = (int)modelButton.GetComponent<RectTransform>().rect.height;
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        int startX = bt_width/2 + 2;
        int defaultX = bt_width/2 + 2;
        int startY = -bt_height/2 - 2;
        List<string> list = new List<string>();

        if (type == "tunnels") list = availableTunnels;
        else if (type == "placeables") list = availablePlaceables;

        for (var a = 0; a < list.Count; a++)
        {
            Button buttonClone = Instantiate(modelButton, modelButton.transform.position, modelButton.transform.rotation) as Button;
            buttonClone.GetComponent<selectButton>().modelId = list[a];
            buttonClone.transform.position = new Vector3(startX, startY, 0f);
            buttonClone.transform.SetParent(transform, false);
            startX += bt_width + 2;
            if (startX + bt_width > screenWidth - 100)
            {
                startX = defaultX;
                startY -= bt_height + 2;
            }
        }
    }

    public void selectObject(string modelId)
    {
        CameraControl.mainCamera.selectObject(modelId);
        panel.gameObject.SetActive(false);
        panelVisible = false;
        removeButtons();
    }

    public void removeButtons()
    {
        GameObject[] currentButtons = GameObject.FindGameObjectsWithTag("SelectButton");
        for (int a = 0; a < currentButtons.Length; a++)
        {
            Destroy(currentButtons[a]);
        }
    }
	

}
