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
    private USingleMarkerPoseEstimationData poseEstimationData;
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    private float originalScale = 100;
    private float numOfFrames = 0;
    private bool runningOnAndroid;

    private static CharucoBoardParameters boardParameters = new CharucoBoardParameters(
        3, //squaresH 
        4, //squaresW
        0.06f, //squareLength
        0.045f //markersLength
    );

    void Start() {

        #if UNITY_ANDROID
            runningOnAndroid = true;
            numOfFrames = 40;
        #endif
        // setup for windows platforms
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN 
            runningOnAndroid = false;
            numOfFrames = 3;
        #endif

        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        _ARRootTransform = GameObject.FindWithTag("ARRoot").transform;
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
                Debug.Log("getting corners");
                if(sucess)
                {
                    numOfSuccessfulFrames++;
                }

                if(numOfSuccessfulFrames >= numOfFrames)
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

}