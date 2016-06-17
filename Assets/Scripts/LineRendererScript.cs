using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineRendererScript : MonoBehaviour 
{
    public static LineRendererScript lineRenderer;
    public Color lineColor = Color.blue;
    public GameObject line;
    public GameObject segment;
    public GameObject segmentTunnel;
    private CameraControl mainCamera;
    public List<GameObject> segmentList;
    public List<GameObject> segmentTunnelList;

    public int width = 20;
    public int height = 20;
    private int segmentSize = 6;
    private int previousZoom = 0;
    private float lineWidth = 0.2f;
    private float zoomLevel = 0;

    //1st - 2nd - x; 3rd - 4th -y
    public int[] range = new int[4];

	void Start () 
    {
        mainCamera = CameraControl.mainCamera;
        lineRenderer = this;

        zoomLevel = mainCamera.transform.position.y;
        lineWidth = zoomLevel / 250;
        if (width % 2 != 0) width++;
        if (height % 2 != 0) height++;

        range[0] = -height/2;
        range[1] = height/2;
        range[2] = -width/2;
        range[3] = width/2;
 
        buildLines();
        buildSegments();
	}

    void Update()
    {
        zoomLevel = mainCamera.transform.position.y;
        if((int)zoomLevel%5 == 0 && (int)previousZoom != (int)zoomLevel)
        {
            previousZoom = (int)zoomLevel;
            lineWidth = zoomLevel / 250;
            reloadLines();
        }

    }

    private void buildLines()
    {
        for(int a = range[0], b = 0; a <= range[1]; a++, b++)
        {
            GameObject lineClone = Instantiate(line, transform.position, transform.rotation) as GameObject;
            lineClone.GetComponent<Line>().pos1 = new Vector3(width * segmentSize/2, 0f, a * segmentSize);
            lineClone.GetComponent<Line>().pos2 = new Vector3(-width * segmentSize/2, 0f, a * segmentSize);
            lineClone.GetComponent<Line>().width = lineWidth;
            lineClone.GetComponent<Line>().c1 = lineColor;
            lineClone.transform.parent = transform;
        }
        for(int a = range[2], b = 0; a <= range[3]; a++, b++)
        {
            GameObject lineClone = Instantiate(line, transform.position, transform.rotation) as GameObject;
            lineClone.GetComponent<Line>().pos1 = new Vector3(a * segmentSize, 0f, height * segmentSize/2);
            lineClone.GetComponent<Line>().pos2 = new Vector3(a * segmentSize, 0f, -height * segmentSize/2);
            lineClone.GetComponent<Line>().width = lineWidth;
            lineClone.GetComponent<Line>().c1 = lineColor;
            lineClone.transform.parent = transform;
        }

    }

    private void buildSegments()
    {
        ////// This will render segments which will hold tiles //////

        int totalSegments = width * height;
        int currentX = range[2];
        int currentY = range[1];

        for(int a = 0, c = 0, d = 1; a < totalSegments; a++, c++, d++)
        {
            GameObject segmentClone = Instantiate(segment, new Vector3(currentX * segmentSize + segmentSize/2, 0, currentY * segmentSize - segmentSize/2), transform.rotation) as GameObject;
            segmentClone.GetComponent<Segment>().id = c;
            segmentClone.transform.parent = transform;
            segmentList.Add(segmentClone);
            currentX++;
            if (d == width)
            {
                d = 0;
                currentY--;
                currentX = range[2];
            }
        }

        buildTunnelTiles();
    }

    private void buildTunnelTiles()
    {
        int upperTileCount = width * height;
        int currentX = range[2];
        int currentY = range[1];

        for(int a = 0, c = 0, d = 1, e = 0; a < upperTileCount + width; a++, c++, d++)
        {
            // Vertical segments

            GameObject segmentTunnelN = Instantiate(segmentTunnel, new Vector3(currentX * segmentSize + segmentSize / 2, 0, currentY * segmentSize), transform.rotation) as GameObject;
            segmentTunnelN.GetComponent<SegmentTunnel>().id = c;
            segmentTunnelN.transform.parent = transform;
            segmentTunnelList.Add(segmentTunnelN);

            // Horizontal segments

            if(a < upperTileCount)
            {
                GameObject segmentTunnelW = Instantiate(segmentTunnel, new Vector3(currentX * segmentSize, 0, currentY * segmentSize - segmentSize / 2), Quaternion.Euler(Vector3.up * 90)) as GameObject;
                segmentTunnelW.GetComponent<SegmentTunnel>().id = height * width + width + c + e;
                segmentTunnelW.transform.parent = transform;
                segmentTunnelList.Add(segmentTunnelW);
            }

            currentX++;

            if (d == width)
            {
                e++;
                if (a < upperTileCount) // Last horizontal segment line
                {
                    GameObject segmentTunnelE = Instantiate(segmentTunnel, new Vector3(currentX * segmentSize, 0, currentY * segmentSize - segmentSize / 2), Quaternion.Euler(Vector3.up * 90)) as GameObject;
                    segmentTunnelE.GetComponent<SegmentTunnel>().id = height * width + width + c + e;
                    segmentTunnelE.transform.parent = transform;
                    segmentTunnelList.Add(segmentTunnelE);
                }

                currentY--;
                currentX = range[2];
                d = 0;
            }
        }



    }

    private void reloadLines()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        for(int a = 0; a < lines.Length; a++)
        {
            Destroy(lines[a]);
        }
        buildLines();
    }
	
}
