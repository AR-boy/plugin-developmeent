using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.Marshallers;
using OpenCVInterop.Utilities;
using Unity.Profiling;

public class EstimatePoseSample : MonoBehaviour
{    
    private WebCamTexture _webCamTexture;
    private Transform _ARRootTransform;
    private UCameraCalibrationData calibData;
    private USingleMarkerPoseEstimationData poseEstimationData;
    private float originalScale = 100;
    private bool notCalibrated = true;
    private static CharucoBoardParameters boardParameters = new CharucoBoardParameters(
        3, //squaresH 
        4, //squaresW
        0.06f,//squareLength
        0.045f //markersLength
    );

    void Start()
    {
        _ARRootTransform = GameObject.FindWithTag("ARRoot").transform;
        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
    }
    void TransformGameObjects(UBoardMarkerPoseEstimationDataEuler framePose, UDetectMarkersData frameMarkers)
    {
        double[][] eulerAngles = framePose.eulerAngles;
        Vec3d tvec = framePose.tvec;

        int rowLength = eulerAngles.Length;

        double[][] eulerAnglesList = new double[rowLength][];

        for (int i = 0; i < rowLength; i++)
        {
            int colLength = eulerAngles[0].Length;
            
            eulerAnglesList[i] = new double[colLength];
            for (int j = 0; j < colLength; j++)
            {
                eulerAnglesList[i][j] = Mathf.Rad2Deg * eulerAngles[i][j];
            }
        }

        float sum_x = 0;
        float sum_y = 0;
        int num_of_int = 0;

        for(int i = 0; i < frameMarkers.markers.Count; i++)
        {
            for(int j = 0; j < frameMarkers.markers[i].Count; j++)
            {
                sum_x += frameMarkers.markers[i][j].x;
                sum_y += frameMarkers.markers[i][j].y;
                num_of_int++;
            }
        }

        float scale = 1; 
        if(frameMarkers.markerIds.Length > 0)
        {
            scale =  (float) tvec.z / 0.56f;
        }

        if(frameMarkers.markerIds.Length > 2)
        {
            _ARRootTransform.position = new Vector3(-(sum_x / num_of_int), -(sum_y / num_of_int),  _ARRootTransform.position.z);

            if(
                eulerAnglesList[0][0] != 0 ||
                eulerAnglesList[0][1] != 0 ||
                eulerAnglesList[0][2] != 0
            )
            {
                _ARRootTransform.rotation = Quaternion.Euler(
                    new Vector3(
                        (float) eulerAnglesList[0][0], 
                        (float) eulerAnglesList[0][2], 
                        -(float) eulerAnglesList[0][1]
                    )
                );
            }
            
            if(!float.IsInfinity(scale) && scale !=0)
            {
                _ARRootTransform.localScale = new Vector3(originalScale / scale, originalScale / scale, originalScale / scale);
            }
                
        }

    }

    void Update()
    {

        if(notCalibrated && Input.GetKeyDown(KeyCode.Space))
        {
            calibData = Aruco.UStaticCalibrateCameraData();
            CameraCalibSerializable calidSaveData = Utilities.LoadCameraCalibrationParams();
            MatDoubleMarshaller distCoeffs = new MatDoubleMarshaller(calidSaveData.distortionCoefficients);
            MatDoubleMarshaller cameraMatrix = new MatDoubleMarshaller(calidSaveData.cameraMatrix);
            calibData =  new UCameraCalibrationData(distCoeffs, cameraMatrix);
            notCalibrated = false;
        }
        else if(!notCalibrated)
        {
            (
                UDetectMarkersData markerData, 
                UBoardMarkerPoseEstimationDataEuler poseEstimationData
            ) = Aruco.UEstimateCharucoBoardPose(
                _webCamTexture.GetPixels32(),
                _webCamTexture.width,
                _webCamTexture.height,
                boardParameters, 
                calibData.cameraMatrix.NativeDataPointer, 
                calibData.distCoeffs.NativeDataPointer
            );

            TransformGameObjects(poseEstimationData, markerData);
        }
                   
    }
}