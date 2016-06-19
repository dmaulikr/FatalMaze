using UnityEngine;
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

    public void updateSegment()
    {
        if (tunnelSegments != null) tunnelSegments.Clear();
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, id));
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, lineRenderer.width * lineRenderer.height + lineRenderer.width + id + (int)id/lineRenderer.width + 1));
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, id + lineRenderer.width));
        tunnelSegments.Add(mainCamera.findById(lineRenderer.segmentTunnelList, tunnelSegments[1].GetComponent<SegmentTunnel>().id - 1));

        string segmentType = "";
        int segmentValue = 0;
        for(int a = 0, b = 1; a < tunnelSegments.Count; a++)
        {
            if (tunnelSegments[a].GetComponent<SegmentTunnel>().type != "" && tunnelSegments[a].GetComponent<SegmentTunnel>().type != null)
            {
                segmentValue += b;
                segmentType = tunnelSegments[a].GetComponent<SegmentTunnel>().type;
            }
            b *= 2;
        }
        string finalCode = (string)(segmentType + segmentValue);
        GameObject currentTile = mainCamera.findObject(mainController.allTunnels, finalCode);

        if (holder != null)
        {
            Destroy(holder.gameObject);
            holder = null;
        }

        if (currentTile)
        {
            GameObject tileClone = Instantiate(currentTile, transform.position, currentTile.transform.rotation) as GameObject;
            holder = tileClone;
            holder.GetComponent<Model>().saveStats();
            holder.transform.parent = GameObject.Find("Tunnels").transform;
        }

    }

}
