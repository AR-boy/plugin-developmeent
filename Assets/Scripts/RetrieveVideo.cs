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
    private USingleMarkerPoseEstimationData poseEstimationData;
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    private float staticScale = 0.002767244139823246f;
    // private double[,] cameraMatrix = new double[,] {{909.34521484375, 0, 617.42626953125}, {0, 912.116333007813, 320.332061767578}, {0,0,1}};
    
    // private double[,] distCoeffs = new double[,] {-0.374482333660126, 0.098893016576767, 0.00986197963356972, 0.00361833954229951, 0.0794400200247765};
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

    void Transform3DObjects(List<List<Point2f>> markerList, List<Point3d> rvecs, List<Point3d> tvecs)
    {
        for(int i = 0; i < markerList.Count; i++)
        {
            // Vector3 cameraPos = new Vector3(500f, 260f, -640f);
            // _displayObjectList[i].position = new Vector3(0, _scaler.GetYScaleFactor() * markerList[i][0].y, -  markerList[i][0].x * _scaler.GetXScaleFactor());
            _displayObjectList[i].position =  new Vector3(0f, 0f, 0f);
            _displayObjectList[i].Translate(((float) tvecs[i].z / staticScale), ((float) tvecs[i].y / staticScale), ((float) tvecs[i].x / staticScale));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("camera resolution: "+ _webCamTexture.width + "x" + _webCamTexture.height);
        // UDetectMarkersData _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
        UDetectMarkersData _markerData;
        if(notCalibrated && Input.GetKeyDown(KeyCode.Space) == true)
        {
            bool sucess = Aruco.UFindChessboardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height,  imagePoints, objectPoints);

            if(sucess)
            {
                numOfSuccessfulFrames++;
            }

            if(numOfSuccessfulFrames >= 5)
            {
                calibData = Aruco.UCalibrateCamera(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height,  imagePoints, objectPoints);

                notCalibrated = false;
            }
        }
        if(!notCalibrated)
        {
            _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
            poseEstimationData = Aruco.UEstimateSingleMarkerPose(_markerData.markersPointer, calibData.cameraMatrixPointer, calibData.distCoeffsPointer);
            List<Point3d> localPoseRvecs = poseEstimationData.rvecs;
            List<Point3d> localPoseTvecs = poseEstimationData.tvecs;

            Transform3DObjects(_markerData.markers, localPoseRvecs, localPoseTvecs);
        }
        
       
    }
}