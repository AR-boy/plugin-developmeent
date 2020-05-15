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

        float scale = Utilities.CalculateScale(tvec, boardParameters.markerLength, originalScale);

        if(frameMarkers.markerIds.Length > 2)
        {
            _ARRootTransform.position = Utilities.CalculateBoardAveragePosition(_ARRootTransform.position, frameMarkers.markers);
            _ARRootTransform.rotation = Utilities.CalculateEulerAngleRotation(eulerAngles, _ARRootTransform.rotation);
            _ARRootTransform.localScale = new Vector3(scale, scale, scale);
        }

    }

    void Update()
    {

        if(notCalibrated && Input.GetKeyDown(KeyCode.Space))
        {
            CameraCalibSerializable calidSaveData = Utilities.LoadCameraCalibrationParams();
            MatDoubleMarshaller distCoeffs = new MatDoubleMarshaller(calidSaveData.distortionCoefficients);
            MatDoubleMarshaller cameraMatrix = new MatDoubleMarshaller(calidSaveData.cameraMatrix);
            double reProjectionError = calidSaveData.reProjectionError;
            calibData = new UCameraCalibrationData(distCoeffs, cameraMatrix, reProjectionError);
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