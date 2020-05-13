using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using OpenCVInterop;
using OpenCVInterop.Marshallers;
using OpenCVInterop.Utilities;

public class CalibrateCameraSample : MonoBehaviour
{
    private WebCamTexture _webCamTexture;
    private Transform maincam;
    private List<List<Vector2>> _markerList;
    private DoubleVectorPoint2FMarshaller allCharucoCorners;
    private DoubleVectorIntMarshaller allCharucoIds;
    private UCameraCalibrationData calibData;
    private USingleMarkerPoseEstimationData poseEstimationData;
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    private bool staticCalib = true;
    private bool scaleGotten = false;
    private float numOfFrames;
    private bool runningOnAndroid;

    private static CharucoBoardParameters boardParameters = new CharucoBoardParameters(
        3, //squaresH 
        4, //squaresW
        0.06f, //squareLength
        0.045f //markersLength
    );


    void Start()
    {
        // setup for android platforms
        #if UNITY_ANDROID
            runningOnAndroid = true;
            numOfFrames = 40;
        #endif
        // setup for windows platforms
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN 
            runningOnAndroid = false;
            numOfFrames = 20;
        #endif

        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();

        allCharucoIds = new DoubleVectorIntMarshaller();
        allCharucoCorners = new DoubleVectorPoint2FMarshaller();
        numOfSuccessfulFrames = 0;
    }

    void Update()
    {
        if(!runningOnAndroid) 
        {
            if(notCalibrated && Input.GetKeyDown(KeyCode.Space))
            {
                bool sucess = Aruco.UFindCharucoBoardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height, boardParameters, allCharucoIds, allCharucoCorners);

                if(sucess)
                {
                    numOfSuccessfulFrames++;
                }

                if(numOfSuccessfulFrames >= 10)
                {
                    calibData = Aruco.UCalibrateCameraCharuco(_webCamTexture.width, _webCamTexture.height, boardParameters, allCharucoIds, allCharucoCorners);

                    CameraCalibSerializable calidSaveData;
                    calidSaveData.distortionCoefficients = (double[][]) calibData.distCoeffs.GetMangedObject();
                    calidSaveData.cameraMatrix = (double[][]) calibData.cameraMatrix.GetMangedObject();
                    Utilities.SaveCameraCalibrationParams(calidSaveData);

                    notCalibrated = false;
                }
            }
        } 
        else 
        {
            if(notCalibrated)
            {
                if (Input.touchCount > 0)
                {
                    bool sucess = Aruco.UFindCharucoBoardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height, boardParameters, allCharucoIds, allCharucoCorners);

                    if(sucess)
                    {
                        numOfSuccessfulFrames++;
                    }

                    if(numOfSuccessfulFrames >= 40)
                    {
                        calibData = Aruco.UCalibrateCameraCharuco(_webCamTexture.width, _webCamTexture.height, boardParameters, allCharucoIds, allCharucoCorners);

                        CameraCalibSerializable calidSaveData;
                        calidSaveData.distortionCoefficients = (double[][]) calibData.distCoeffs.GetMangedObject();
                        calidSaveData.cameraMatrix = (double[][]) calibData.cameraMatrix.GetMangedObject();
                        Utilities.SaveCameraCalibrationParams(calidSaveData);

                        notCalibrated = false;
                    }
                }
            } 
        }
    }
}
