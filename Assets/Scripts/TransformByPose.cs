using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.MarshalOpenCV;
using OpenCVInterop.Utilities;

public class TransformByPose : MonoBehaviour
{    
    private DisplayPlaneAutosizer _scaler;
    private WebCamTexture _webCamTexture;
    private Transform maincam;
    private GameObject[] _cubeCoords;
    private List<Transform> _displayObjects;
    private UCameraCalibrationData calibData;
    private USingleMarkerPoseEstimationData poseEstimationData;

    private float originalScale = 100;
    private bool notCalibrated = true;

    void Start()
    {
        _cubeCoords = GameObject.FindGameObjectsWithTag("3DObject");
        print("_cubeCoords: "+ _cubeCoords.Length);
        _displayObjects = new List<Transform>();
        maincam = GameObject.Find("Main Camera").GetComponent<Transform>();
        for(int i = 0; i < _cubeCoords.Length; i++)
        { 
            _displayObjects.Add(_cubeCoords[i].GetComponent<Transform>());
        }

        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        _scaler = GetComponentInParent<DisplayPlaneAutosizer>();

    }
    void TransformGameObjects(Point3d rvec, Point3d tvec, double[][] eulerAngles, UDetectMarkersData frameMarkers, USingleMarkerPoseEstimationData markerPoses)
    {

        int rowLength = eulerAngles.Length;
        int colLength = eulerAngles[0].Length;
        double[][] eulerAnglesList = new double[rowLength][];

        for (int i = 0; i < rowLength; i++)
        {
            eulerAnglesList[i] = new double[colLength];
            for (int j = 0; j < colLength; j++)
            {
                eulerAnglesList[i][j] = Mathf.Rad2Deg * eulerAngles[i][j];
            }
        }

        // marker4Index = IndexOf<int>(frameMarkers.markerIds, 4);
        int marker1Index = Array.IndexOf(frameMarkers.markerIds, 1);
        int marker5Index = Array.IndexOf(frameMarkers.markerIds, 5);
        int marker3Index = Array.IndexOf(frameMarkers.markerIds, 3);
        float scale = 1; 
        if( marker5Index > -1 && marker1Index > -1)
        {
            Point3d marker5Tvec = markerPoses.tvecs[marker5Index];
            Point3d marker1Tvec = markerPoses.tvecs[marker1Index];
            Vector3 scalingVector = new Vector3(
                (float) (marker1Tvec.x + marker5Tvec.x), 
                (float) (marker1Tvec.y + marker5Tvec.y), 
                (float) (marker1Tvec.z + marker5Tvec.z)
            ); 
            scale = scalingVector.magnitude / 1.048f;
        }
        float sumx = 0;
        float sumy = 0;
        for(int i = 0; i < frameMarkers.markerIds.Length; i ++)
        {
            sumx += frameMarkers.markers[i][0].x;
            sumy += frameMarkers.markers[i][0].y;
        }
        // _displayObject.position = new Vector3(0, 0, 20);
        // for(int i = 0; i < frameMarkers.markerIds.Length; i ++)
        // {
            

            if(eulerAnglesList[0][1] > 0)
            {
                _displayObjects[0].position = new Vector3( -sumx / frameMarkers.markerIds.Length, -sumy / frameMarkers.markerIds.Length, 120);
                // Debug.Log("x: "+eulerAnglesList[0][0]+ " y: "+ eulerAnglesList[0][1]+ " z: "+ eulerAnglesList[0][2]);
                _displayObjects[0].transform.rotation = Quaternion.Euler(new Vector3((float) eulerAnglesList[0][0], (float) eulerAnglesList[0][2], -(float) eulerAnglesList[0][1]));
                _displayObjects[0].transform.localScale = new Vector3(originalScale / scale, originalScale / scale, originalScale / scale);
            }
        // }
           
        

         
    }

    // Update is called once per frame
    void Update()
    {

        if(notCalibrated && Input.GetKeyDown(KeyCode.Space))
        {
            calibData = Aruco.UStaticCalibrateCameraData();
            CameraCalibSerializable calidSaveData = Utilities.LoadCameraCalibrationParams();
            MatDMarshaller distCoeffs = new MatDMarshaller(calidSaveData.distortionCoefficients);
            MatDMarshaller cameraMatrix = new MatDMarshaller(calidSaveData.cameraMatrix);
            calibData =  new UCameraCalibrationData(distCoeffs, cameraMatrix);
            notCalibrated = false;
        }
        else if(!notCalibrated)
        {
            // UBoardMarkerPoseEstimationDataEuler poseEstimationData = Aruco.UEstimateArucoBoardPose(_webCamTexture.GetPixels32(), calibData.cameraMatrix.NativeDataPointer, calibData.distCoeffs.NativeDataPointer);
            UDetectMarkersData frameMarkers = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), 1920, 1080);
            USingleMarkerPoseEstimationData markerPoses = Aruco.UEstimateSingleMarkerPose(frameMarkers.markersPointer, calibData.cameraMatrix.NativeDataPointer, calibData.distCoeffs.NativeDataPointer);
            UBoardMarkerPoseEstimationDataEuler poseEstimationData = Aruco.UEstimateCharucoBoardPose(_webCamTexture.GetPixels32(), calibData.cameraMatrix.NativeDataPointer, calibData.distCoeffs.NativeDataPointer);
            Point3d localPoseRvec = poseEstimationData.rvec;
            Point3d localPoseTvec = poseEstimationData.tvec;
            double[][] localPoseRvecEuler = poseEstimationData.eulerAngles;

            TransformGameObjects(localPoseRvec, localPoseTvec, localPoseRvecEuler, frameMarkers, markerPoses);
            // OpenCVMarshal.DeleteVectorIntPointer(frameMarkers.markerIdsPointer);
            // OpenCVMarshal.DeleteVector2PointFPointer(frameMarkers.markersPointer);
            // OpenCVMarshal.CreateVector2PointFPointer(frameMarkers.rejectedCandidatesPointer);

            // OpenCVMarshal.DeleteVectorVec3dPointer(markerPoses.tvecsPointer);

            // IntPtr markerIds = OpenCVMarshal.CreateVectorIntPointer();
            // IntPtr markers = OpenCVMarshal.CreateVector2PointFPointer();
            // IntPtr rejectedCandidates = OpenCVMarshal.CreateDoubleVector2PointFPointer();
        }
                   
    }
}