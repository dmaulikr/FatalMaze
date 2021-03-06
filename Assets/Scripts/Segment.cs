﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Segment : MonoBehaviour 
{
    public GameObject holder;
    public int id;

    private CameraControl mainCamera = CameraControl.mainCamera;
    private EditorUI editorUI = EditorUI.editorUI;
    private MainController mainController = MainController.mainController;
    private LineRendererScript lineRenderer = LineRendererScript.lineRenderer;
    public List<GameObject> tunnelSegments;

    void Start()
    {
        holder = null;
    }

    public bool updateSegment(Transform callerSegment, bool check)
    {
        if (tunnelSegments != null) tunnelSegments.Clear();
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, id));
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, lineRenderer.width * lineRenderer.height + lineRenderer.width + id + (int)id/lineRenderer.width + 1));
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, id + lineRenderer.width));
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, tunnelSegments[1].GetComponent<SegmentTunnel>().id - 1));

        if (mainCamera.findById(lineRenderer.segmentRoomList, id - 1)) tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentRoomList, id - 1));
        else tunnelSegments.Add(null);
        if (mainCamera.findById(lineRenderer.segmentRoomList, id)) tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentRoomList, id));
        else tunnelSegments.Add(null);
        if (mainCamera.findById(lineRenderer.segmentRoomList, id + lineRenderer.width)) tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentRoomList, id + lineRenderer.width));
        else tunnelSegments.Add(null);
        if (mainCamera.findById(lineRenderer.segmentRoomList, id + lineRenderer.width - 1)) tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentRoomList, id + lineRenderer.width - 1));
        else tunnelSegments.Add(null);

        string segmentType = "";
        int segmentValue = 0;
        for(int a = 0, b = 1; a < tunnelSegments.Count; a++) // additional value is used when connecting corridors and rooms
        {
            if (tunnelSegments[a] != null && tunnelSegments[a].GetComponent<SegmentTunnel>().type != "" && tunnelSegments[a].GetComponent<SegmentTunnel>().type != null)
            {
                segmentValue += b;
                if (tunnelSegments[a] != null && tunnelSegments[a].GetComponent<SegmentTunnel>().type != "" && tunnelSegments[a].GetComponent<SegmentTunnel>().type != null)
                {
                    segmentType = "";
                    segmentType = tunnelSegments[a].GetComponent<SegmentTunnel>().type;
                }
            }

            b *= 2;
        }

        string finalCode = (string)(segmentType + segmentValue);

        GameObject currentTile = new GameObject();
        currentTile.hideFlags = HideFlags.HideInHierarchy;

        if (mainCamera.findObject(mainController.allTunnels, finalCode))
        {
            currentTile = mainCamera.findObject(mainController.allTunnels, finalCode);
        }
        else if (mainCamera.findObject(mainController.allRooms, finalCode))
        {
            currentTile = mainCamera.findObject(mainController.allRooms, finalCode);
        }


        if (holder != null && !check)
        {
            Destroy(holder.gameObject);
            holder = null;
        }

        if (currentTile.GetComponent<Model>() && !check)
        {
            GameObject tileClone = Instantiate(currentTile, transform.position, currentTile.transform.rotation) as GameObject;
            holder = tileClone;
            holder.GetComponent<Model>().saveStats();
            holder.transform.parent = GameObject.Find("Tunnels").transform;
        }
        else if(currentTile.GetComponent<Model>() && check)
        {
            return true;
        }
        else if (!currentTile.GetComponent<Model>())
        {
            callerSegment.GetComponent<SegmentTunnel>().type = null;
        }

        return false;
    }

}
