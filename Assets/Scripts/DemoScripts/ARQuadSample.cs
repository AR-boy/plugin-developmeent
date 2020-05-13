using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.Marshallers;
using OpenCVInterop.Utilities;
public struct QuadVertices {
    public Vector2 bottomLeft, bottomRight, topLeft, topRight;
    public QuadVertices(Vector2 bottomLeft, Vector2 bottomRight, Vector2 topLeft, Vector2 topRight)
    {
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
        this.topLeft = topLeft;
        this.topRight = topRight;
    } 
}
public class ARQuadSample : MonoBehaviour
{
    // Start is called before the first frame update
    private WebCamTexture _webCamTexture;
    private Transform quadTransform;
    private Mesh quadMesh;
    private GameObject quad;
    void Start()
    {
        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        quad = GameObject.FindWithTag("2DObject");
        quadTransform = quad.transform;
        quadMesh = quad.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        UDetectMarkersData _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
        if(_markerData.markerIds.Length > 0)
        {
            int[] markersIds = _markerData.markerIds;
            List<List<Vector2>> markers = _markerData.markers;
            List<List<Vector2>> rejectedCandidates = _markerData.rejectedCandidates;

            DrawQuad(markersIds, markers, rejectedCandidates);    
        }
    }
    QuadVertices GetQuadVertices(List<List<Vector2>> markers) {
        
        float sum_x = 0;
        float sum_y = 0;
        int k = 0;
        for(int i=0; i < markers.Count; i++)
        {
            List<Vector2> marker = markers[i];
            for(int j=0; j < markers[i].Count; j++)
            {
                sum_x += markers[i][j].x;
                sum_y += markers[i][j].y;
                k++;
            }
        }

        float avg_x = sum_x /(k);
        float avg_y = sum_y /(k);
        Vector2 avg_vector = new Vector2(-avg_x, -avg_y);
        QuadVertices quadVertices = new QuadVertices(avg_vector, avg_vector, avg_vector, avg_vector);


        for(int i=0; i < markers.Count; i++)
        {
            List<Vector2> marker = markers[i];
            for(int j=0; j < markers[i].Count; j++)
            {
                if(quadVertices.bottomLeft.x > -markers[i][j].x && quadVertices.bottomLeft.y > -markers[i][j].y)
                {
                    quadVertices.bottomLeft = -markers[i][j];
                }
                if(quadVertices.bottomRight.x < markers[i][j].x && quadVertices.bottomRight.y > -markers[i][j].y)
                {
                    quadVertices.bottomRight = -markers[i][j];
                }
                if(quadVertices.topRight.x < -markers[i][j].x && quadVertices.topRight.y < -markers[i][j].y)
                {
                    quadVertices.topRight = -markers[i][j];
                }
                if(quadVertices.topLeft.x > -markers[i][j].x && quadVertices.topLeft.y < -markers[i][j].y)
                {
                    quadVertices.topLeft = -markers[i][j];
                }
            }
        }
        return quadVertices;
    }
    void DrawQuad(int[] markersIds, List<List<Vector2>> markers, List<List<Vector2>> rejectedCandidates) {
        QuadVertices quadVectices = GetQuadVertices(markers);

        Vector3[] verticeList = new Vector3[4];
        verticeList[0] = new Vector3(quadVectices.bottomLeft.x, quadVectices.bottomLeft.y, 10);
        verticeList[1] = new Vector3(quadVectices.bottomRight.x, quadVectices.bottomRight.y, 10);
        verticeList[2] = new Vector3(quadVectices.topLeft.x, quadVectices.topLeft.y, 10);
        verticeList[3] = new Vector3(quadVectices.topRight.x, quadVectices.topRight.y, 10);

        quadMesh.vertices = verticeList;

        int[] triangles = new int[6];

        //  Lower left triangle.
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        //  Upper right triangle.   
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;

        quadMesh.triangles = triangles;

        Vector3[] normals = new Vector3[4];

        normals[0] = Vector3.forward;
        normals[1] = Vector3.forward;
        normals[2] = Vector3.forward;
        normals[3] = Vector3.forward;

        quadMesh.normals = normals;

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0f, 0f);
        uv[1] = new Vector2(1f, 0f);
        uv[2] = new Vector2(0f, 1f);
        uv[3] = new Vector2(1f, 1f);

        quadMesh.uv = uv;

    } 
}
