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

    // defined board information for real life specification of the board generated with GenerateCharucoBoard script
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
    // transform game object
    void TransformGameObjects(UBoardMarkerPoseEstimationDataEuler framePose, UDetectMarkersData frameMarkers)
    {
        double[][] eulerAngles = framePose.eulerAngles;
        Vec3d tvec = framePose.tvec;
        // get 3D object's scale
        float scale = Utilities.CalculateScale(tvec, boardParameters.markerLength, originalScale);
        // if more than 2 markers found transform based on average position and euler angle rotation
        if(frameMarkers.markerIds.Length > 2)
        {
            _ARRootTransform.position = Utilities.CalculateBoardAveragePosition(_ARRootTransform.position, frameMarkers.markers);
            _ARRootTransform.rotation = Utilities.CalculateEulerAngleRotation(eulerAngles, _ARRootTransform.rotation);
            _ARRootTransform.localScale = new Vector3(scale, scale, scale);
        }

    }

    void Update()
    {

        // if space is down do AR tranformation
        if(notCalibrated && Input.GetKeyDown(KeyCode.Space))
        {
            // laod camera calibrations setting
            CameraCalibSerializable calidSaveData = Utilities.LoadCameraCalibrationParams();
            // initialise marhsallers
            MatDoubleMarshaller distCoeffs = new MatDoubleMarshaller(calidSaveData.distortionCoefficients);
            MatDoubleMarshaller cameraMatrix = new MatDoubleMarshaller(calidSaveData.cameraMatrix);
            double reProjectionError = calidSaveData.reProjectionError;
            // get calibration data
            calibData = new UCameraCalibrationData(distCoeffs, cameraMatrix, reProjectionError);
            notCalibrated = false;
        }
        else if(!notCalibrated)
        {
            // estimate charuco board pose
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