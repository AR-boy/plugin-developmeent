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
    private GameObject _cubeCoord;
    private Transform _displayObject;
    private List<List<Point2f>> _markerList;
    private IntPtr objectPoints;
    private IntPtr imagePoints;
    private UCameraCalibrationData calibData;
    private USingleMarkerPoseEstimationData poseEstimationData;
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    float rotationSpeed = 2f;
    private float staticScale = 0.002767244139823246f;
    // private double[,] cameraMatrix = new double[,] {{909.34521484375, 0, 617.42626953125}, {0, 912.116333007813, 320.332061767578}, {0,0,1}};
    
    // private double[,] distCoeffs = new double[,] {-0.374482333660126, 0.098893016576767, 0.00986197963356972, 0.00361833954229951, 0.0794400200247765};
    void Start()
    {
        _cubeCoord = GameObject.FindGameObjectWithTag("3DObject");
        _displayObject =  _cubeCoord.GetComponent<Transform>();
        // for(int i = 0; i < _cubeCoordList.Length; i++)
        // {
        //     _displayObjectList[i] = _cubeCoordList[i].GetComponent<Transform>();
        // }

        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        _scaler = GetComponentInParent<DisplayPlaneAutosizer>();

        imagePoints = OpenCVMarshal.CreateDoubleVector2PointFPointer();
        objectPoints = OpenCVMarshal.CreateDoubleVector3PointFPointer();
        numOfSuccessfulFrames = 0;
    }

    void Transform3DObjects(List<List<Point2f>> markerList, int[] markersIds, Point3d rvec, Point3d tvec, IntPtr rvecPointer)
    {

        if(markerList.Count != 0)
        {

            double[,] eulerAnglesList = Aruco.UGetEulerAngles(rvecPointer);

            int rowLength = eulerAnglesList.GetLength(0);
            int colLength = eulerAnglesList.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Debug.Log("i " + i + "   j "+j);
                    eulerAnglesList[i,j] =  (180 / Math.PI) * eulerAnglesList[i,j];
                }
            }

            Debug.Log("x: " + (float)eulerAnglesList[0,2]);
            Debug.Log("y: " + (float)eulerAnglesList[0,1]);
            Debug.Log("z: " + (float)eulerAnglesList[0,0]);

            float x = 960f;
            _displayObject.position = new Vector3(-x‬, -(540f), 20f);
            // GameObject.Find("Main Camera").transform.position = _displayObject.position;
            // Vector3 translateVec = new Vector3(Mathf.Pow((float)tvecs[0].x, -1) * 100, (Mathf.Pow((float)tvecs[0].y, -1) * 100), (Mathf.Pow((float)tvecs[0].z, -1) * 100));
            _displayObject.localEulerAngles = new Vector3((float)eulerAnglesList[0,1], -(float)eulerAnglesList[0,0], -(float)eulerAnglesList[0,2]); 
            //_displayObject.Rotate(new Vector3((float)eulerAnglesList[0,0], (float)eulerAnglesList[0     ,1], (float)eulerAnglesList[0,2]));
            _displayObject.localScale = new Vector3( -Mathf.Pow((float)tvec.z, -1) * 60,  Mathf.Pow((float)tvec.z, -1 ) * 60,  40); 
            // GameObject.Find("Main Camera").transform.Translate(new Vector3(something.X,  something.Y, something.Z  * 10000 ));
        // Vector3 cameraPos = new Vector3(500f, 260f, -640f);
        }
        // _displayObjectList[i].position =  new Vector3(0f, 0f, 0f);
        // _displayObjectList[i].Translate(((float) tvecs[i].z ), ((float) tvecs[i].y / st), ((float) tvecs[i].x / staticScale));
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
            // calibData = Aruco.UStaticCalibrateCameraData();
            Debug.Log("nonsesne");
            // notCalibrated = false;
            
        }


        if(!notCalibrated)
        {
            /*
            int rowLength = calibData.distortionCoefficients.GetLength(0);
            int colLength = calibData.distortionCoefficients.GetLength(1);
            string string1 = "";
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                string1 += " " + calibData.distortionCoefficients[i, j];
                }
            string1 += "\n";
            }

            int rowLength2 = calibData.cameraMatrix.GetLength(0);
            int colLengt2 = calibData.cameraMatrix.GetLength(1);
            string string2 = "";

            for (int i = 0; i < rowLength2; i++)
            {
                for (int j = 0; j < colLengt2; j++)
                {
                string2 += " " + calibData.cameraMatrix[i, j];
                }
            string2 += "\n";
            }


            Debug.Log("distortionCoefficients: " + string1);
            Debug.Log("cameraMatrix: " + string2);
            */

            _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
            if(_markerData.markerIds.Length >= (5 * 7))
            {
                int listOfIds = OpenCVMarshal.GetVectorIntSize(_markerData.markerIdsPointer);
                UBoardMarkerPoseEstimationData poseEstimationData = Aruco.UEstimateBoardMarkerPose(_markerData.markersPointer, _markerData.markerIdsPointer, calibData.cameraMatrixPointer, calibData.distCoeffsPointer);
                Point3d localPoseRvecs = poseEstimationData.rvec;
                Point3d localPoseTvecs = poseEstimationData.tvec;

                Transform3DObjects(_markerData.markers, _markerData.markerIds, localPoseRvecs, localPoseTvecs, poseEstimationData.rvecPointer);
            }
            // poseEstimationData = Aruco.UEstimateSingleMarkerPose(_markerData.markersPointer, calibData.cameraMatrixPointer, calibData.distCoeffsPointer);
            
        }
        
       
    }
}