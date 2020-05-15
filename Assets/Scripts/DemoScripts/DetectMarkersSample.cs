using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.Utilities;
public class DetectMarkersSample : MonoBehaviour
{    
    private WebCamTexture _webCamTexture;

    void Start()
    {
        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();

    }

    void DrawMarkers(int[] markersIds, List<List<Vector2>> markers, List<List<Vector2>> rejectedCandidates)
    {
        // draw line gizmo at detected marker posiotns
        for(int i = 0; i < markersIds.Length; i++)
        {
            for(int j = 0; j < markers[0].Count; j++)
            {
                Vector3 startVector;
                Vector3 endVector;
                if(j> 0 && j % (markers[i].Count - 1) == 0)
                {
                    startVector = new Vector3(-markers[i][j].x, -markers[i][j].y, 1);
                    endVector = new Vector3(-markers[i][0].x, -markers[i][0].y, 1);

                    Debug.DrawLine(startVector, endVector, Color.green);
                }
                else
                {
                    startVector = new Vector3(-markers[i][j].x, -markers[i][j].y, 1);
                    endVector = new Vector3(-markers[i][j+1].x, -markers[i][j+1].y, 1);

                    Debug.DrawLine(startVector, endVector, Color.red);
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
            // detect markers in the frame with specified board parameters 
            UDetectMarkersData _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
            if(_markerData.markerIds.Length > 0)
            {
                int[] markersIds = _markerData.markerIds;
                List<List<Vector2>> markers = _markerData.markers;
                List<List<Vector2>> rejectedCandidates = _markerData.rejectedCandidates;

                DrawMarkers(markersIds, markers, rejectedCandidates);    
            }
    }
}