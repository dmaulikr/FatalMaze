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
            if(tunnelSegments[a].GetComponent<SegmentTunnel>().type != "")
            {
                segmentValue += b;
                segmentType = tunnelSegments[a].GetComponent<SegmentTunnel>().type;
            }
            b *= 2;
        }
        string finalCode = (string)(segmentType + segmentValue);
        GameObject currentTile = mainCamera.findObject(mainController.allTunnels, finalCode);

        if (currentTile)
        {
            Destroy(holder.gameObject);
            holder = Instantiate(currentTile, transform.position, currentTile.transform.rotation) as GameObject;
            holder.GetComponent<Model>().saveStats();
            holder.transform.parent = GameObject.Find("Tunnels").transform;
        }
    }

}
