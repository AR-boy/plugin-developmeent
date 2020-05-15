using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.Utilities;
public class DetectMarkersBuildSample : MonoBehaviour
{    
    private WebCamTexture _webCamTexture;
    private List<Transform> _markerDispalyers;
    private int numOfSuccessfulFrames;

    void Start()
    {
        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("3DObject");
        _markerDispalyers = new List<Transform>();
        foreach(GameObject cube in cubes)
        {
            _markerDispalyers.Add(cube.transform);
        }

    }

    void DrawMarkers(int[] markersIds, List<List<Vector2>> markers, List<List<Vector2>> rejectedCandidates)
    {
        for(int i = 0; i < markersIds.Length; i++)
        {
            _markerDispalyers[i].position = new Vector3(-markers[i][0].x, -markers[i][0].y, 10);
        }
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

            DrawMarkers(markersIds, markers, rejectedCandidates);    
        }
    }
}