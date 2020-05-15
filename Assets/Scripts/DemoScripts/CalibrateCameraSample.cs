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
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    private bool staticCalib = true;
    private bool scaleGotten = false;
    private float numOfFrames;
    private bool runningOnAndroid;
    // defined board information for real life specification of the board generated with GenerateCharucoBoard script
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
                // find charuco board corners in the frame
                bool sucess = Aruco.UFindCharucoBoardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height, boardParameters, allCharucoIds, allCharucoCorners);

                if(sucess)
                {
                    numOfSuccessfulFrames++;
                }

                if(numOfSuccessfulFrames >= numOfFrames)
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
        else 
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

                    if(numOfSuccessfulFrames >= numOfFrames)
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
        }
    }
}
