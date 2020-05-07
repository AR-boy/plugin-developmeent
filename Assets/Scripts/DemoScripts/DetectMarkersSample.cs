using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.MarshalOpenCV;
using OpenCVInterop.Utilities;
public class DetectMarkersSample : MonoBehaviour
{    
    private DisplayPlaneAutosizer _scaler;
    private WebCamTexture _webCamTexture;
    private int numOfSuccessfulFrames;

    void Start()
    {
        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        _scaler = GetComponentInParent<DisplayPlaneAutosizer>();

    }

    void DrawMarkers(int[] markersIds, List<List<Point2f>> markers, List<List<Point2f>> rejectedCandidates)
    {
        for(int i = 0; i < markersIds.Length; i++)
        {
            for(int j = 0; j < markers[0].Count; j++)
            {
                Vector3 startVector;
                Vector3 endVector;
                if(j> 0 && j % (markers[i].Count - 1) == 0)
                {
                    Debug.Log("j: "+ j);
                    startVector = new Vector3(-markers[i][j].x, -markers[i][j].y, 10f);
                    endVector = new Vector3(-markers[i][0].x, -markers[i][0].y, 10f);

                    Debug.DrawLine(startVector, endVector, Color.red);
                }
                else
                {
                    startVector = new Vector3(-markers[i][j].x, -markers[i][j].y, 10f);
                    endVector = new Vector3(-markers[i][j+1].x, -markers[i][j+1].y, 10f);

                    Debug.DrawLine(startVector, endVector, Color.red);
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
     
            UDetectMarkersData _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
            if(_markerData.markerIds.Length > 0)
            {
                int[] markersIds = _markerData.markerIds;
                List<List<Point2f>> markers = _markerData.markers;
                List<List<Point2f>> rejectedCandidates = _markerData.rejectedCandidates;

                DrawMarkers(markersIds, markers, rejectedCandidates);    
            }
    }
}