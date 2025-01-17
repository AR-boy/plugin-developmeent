using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using OpenCVInterop;
using OpenCVInterop.Marshallers;
using OpenCVInterop.Utilities;

public class AndroidCalibrateCameraAndEstimatePoseSample : MonoBehaviour {
    private WebCamTexture _webCamTexture;
    private Transform maincam;
    private Transform _ARRootTransform;
    private List<List<Vector2>> _markerList;
    private DoubleVectorPoint2FMarshaller allCharucoCorners;
    private DoubleVectorIntMarshaller allCharucoIds;
    private UCameraCalibrationData calibData;
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    private float originalScale = 100;
    private float numOfFrames = 40;

    // defined board information for real life specification of the board generated with GenerateCharucoBoard script
    private static CharucoBoardParameters boardParameters = new CharucoBoardParameters(
        3, //squaresH 
        4, //squaresW
        0.06f, //squareLength
        0.045f //markersLength
    );

    void Start() {
        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        _ARRootTransform = GameObject.FindWithTag("ARRoot").transform;
        allCharucoIds = new DoubleVectorIntMarshaller();
        allCharucoCorners = new DoubleVectorPoint2FMarshaller();
        numOfSuccessfulFrames = 0;
    }
    void Update()
    {
        if(notCalibrated)
        {
            if (Input.touchCount > 0)
            {
                // find charuco board corners in the frame
                bool sucess = Aruco.UFindCharucoBoardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height, boardParameters, allCharucoIds, allCharucoCorners);

                if(sucess)
                {
                    numOfSuccessfulFrames++;
                }

                if(numOfSuccessfulFrames >= 40)
                {
                    // calibrate camera after defined amount of frames have been recorded
                    calibData = Aruco.UCalibrateCameraCharuco(_webCamTexture.width, _webCamTexture.height, boardParameters, allCharucoIds, allCharucoCorners);

                    CameraCalibSerializable calidSaveData;
                    calidSaveData.distortionCoefficients = (double[][]) calibData.distCoeffs.GetMangedObject();
                    calidSaveData.cameraMatrix = (double[][]) calibData.cameraMatrix.GetMangedObject();
                    calidSaveData.reProjectionError = calibData.reProjectionError;
                    // save obtained calibration settings
                    Utilities.SaveCameraCalibrationParams(calidSaveData);

                    notCalibrated = false;
                }
            }
        } 
        if(!notCalibrated)
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
}