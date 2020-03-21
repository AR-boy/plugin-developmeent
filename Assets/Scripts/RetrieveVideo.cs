using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.MarshalOpenCV;



public class RetrieveVideo : MonoBehaviour
{    
    private DisplayPlaneAutosizer _scaler;
    private WebCamTexture _webCamTexture;
    // private CVMarker[] _markerList;
    private GameObject[] _cubeCoordList;
    private Transform[] _displayObjectList;
    private List<List<Point2f>> _markerList;
    private IntPtr objectPoints;
    private IntPtr imagePoints;
    private UCameraCalibrationData calibData;
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    void Start()
    {
        GameObject[] _cubeCoordList = GameObject.FindGameObjectsWithTag("3DObject");
        _displayObjectList = new Transform[_cubeCoordList.Length];
        for(int i = 0; i < _cubeCoordList.Length; i++)
        {
            _displayObjectList[i] = _cubeCoordList[i].GetComponent<Transform>();
        }

        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        _scaler = GetComponentInParent<DisplayPlaneAutosizer>();

        imagePoints = OpenCVMarshal.CreateDoubleVector2PointFPointer();
        objectPoints = OpenCVMarshal.CreateDoubleVector3PointFPointer();
        numOfSuccessfulFrames = 0;
    }

    void Transform3DObjects(List<List<Point2f>> markerList)
    {
        for(int i = 0; i < markerList.Count; i++)
        {
            _displayObjectList[i].position = new Vector3(0, _scaler.GetYScaleFactor() * markerList[i][0].y, -  markerList[i][0].x * _scaler.GetXScaleFactor());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("camera resolution: "+ _webCamTexture.width + "x" + _webCamTexture.height);
        // UDetectMarkersData _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
        if(notCalibrated &&  Input.GetKeyDown(KeyCode.Space) == true)
        {
            bool sucess = Aruco.UFindChessboardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height,  imagePoints, objectPoints);

            if(sucess)
            {
                numOfSuccessfulFrames++;
            }

            if(numOfSuccessfulFrames >= 5)
            {
                calibData = Aruco.UCalibrateCamera(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height,  imagePoints, objectPoints);
                Debug.Log("sahh: " + calibData.cameraMatrix);
                notCalibrated = false;
            }
        }
        
        // _markerList = _markerData.markers;
        // Transform3DObjects(_markerList);
    }
}