using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

using OpenCVInterop;
using OpenCVInterop.MarshalOpenCV;
using OpenCVInterop.Utilities;


struct BoardCornerMarkers {
    List<Point2f> bottomLeft;
    List<Point2f> topLeft;
    List<Point2f> bottomRight;
    List<Point2f> topRight;


    public BoardCornerMarkers(List<Point2f> bottomLeft_param, List<Point2f> topLeft_param, List<Point2f> bottomRight_param, List<Point2f> topRight_param) 
    {
        bottomLeft = bottomLeft_param;
        topLeft = topLeft_param;
        bottomRight = bottomRight_param;
        topRight = topRight_param;

    }
}
public class RetrieveVideo : MonoBehaviour
{    
    private DisplayPlaneAutosizer _scaler;
    private WebCamTexture _webCamTexture;
    private Transform maincam;
    // private CVMarker[] _markerList;
    private GameObject _cubeCoord;
    private Transform _displayObject;
    private List<List<Point2f>> _markerList;
    private IntPtr allCharucoCorners;
    private DoubleVectorIntMarshaller allCharucoIds;
    private UCameraCalibrationData calibData;
    private USingleMarkerPoseEstimationData poseEstimationData;
    private int numOfSuccessfulFrames;
    private bool notCalibrated = true;
    private bool staticCalib = true;
    private bool scaleGotten = false;
    #if UNITY_ANDROID
        private bool runningOnAndroid = false;
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN 
        private bool runningOnAndroid = false;
    #endif

    private float scale = 0;
    // private double[,] cameraMatrix = new double[,] {{909.34521484375, 0, 617.42626953125}, {0, 912.116333007813, 320.332061767578}, {0,0,1}};
    
    // private double[,] distCoeffs = new double[,] {-0.374482333660126, 0.098893016576767, 0.00986197963356972, 0.00361833954229951, 0.0794400200247765};
    void Start()
    {
        _cubeCoord = GameObject.FindGameObjectWithTag("3DObject");
        _displayObject = _cubeCoord.GetComponent<Transform>();
        maincam = GameObject.Find("Main Camera").GetComponent<Transform>();
        // for(int i = 0; i < _cubeCoordList.Length; i++)
        // {
        //     _displayObjectList[i] = _cubeCoordList[i].GetComponent<Transform>();
        // }

        _webCamTexture = GetComponentInParent<InitWebCamera>().GetCamera();
        _scaler = GetComponentInParent<DisplayPlaneAutosizer>();

        allCharucoIds = new DoubleVectorIntMarshaller();
        allCharucoCorners = OpenCVMarshal.CreateDoubleVector2PointFPointer();
        numOfSuccessfulFrames = 0;
    }
    Vector3 GetEulerAngles(IntPtr rvecPointer)
    {
        // returned angles is a 1x3 matrix
        double[][] eulerAnglesList = Aruco.UGetEulerAngles(rvecPointer);

        // create new vector containing Euler Angles in degrees
        Vector3 eualerAngles = new Vector3(
            // x and y are switched in opencv
            Mathf.Rad2Deg * (float) eulerAnglesList[0][1], 
            Mathf.Rad2Deg * (float) eulerAnglesList[0][0], 
            Mathf.Rad2Deg * (float) eulerAnglesList[0][2]
        );

        return eualerAngles;
    }
    /*
    BoardCornerMarkers GetBoardCornerMakers(List<List<Point2f>> markerList, int[] markersIds, Vector3 eulerAngles, int markersH)
    {
        List<Point2f> bottomLeft;
        List<Point2f> topLeft;
        List<Point2f> bottomRight;
        List<Point2f> topRight;
        // markers are always in order
        int numOfMarkers = markersIds.Length;

        int bottomLeftPosition = 0;
        int topLeftPosition = (markersH - 1);
        int bottomRightPosition = (numOfMarkers-1) - (markersH - 1);
        int topRightPosition = (numOfMarkers-1);

        for(int i = 0; i <  markersIds.Length; i++)
        {
            if(markersIds[i] == bottomLeftPosition)
            {
                bottomLeft = markerList[i];
            } 
            else if(markersIds[i] == topLeftPosition)
            {
                topLeft = markerList[i];
            }
            else if(markersIds[i] == bottomRightPosition)
            {
                bottomRight = markerList[i];
            }
            else if(markersIds[i] == topRightPosition)
            {
                topRight = markerList[i];
            }
        }
        return new BoardCornerMarkers(bottomLeft, topLeft, bottomRight, topRight);

    }
    */
    float tvecsToMeters(IntPtr markerCorners, int[] markerIds, IntPtr cameraMatrix, IntPtr distCoeffs)
    {
        // marker 0 is 11 cm away from
        float realSize = 0.11f;
        int originMarkerId = 30;
        int relativeMarkerPosition = 0;
        USingleMarkerPoseEstimationData poseEstimationData = Aruco.UEstimateSingleMarkerPose(markerCorners, cameraMatrix, distCoeffs);

        for(int i = 0; i< markerIds.Length; i++)
        {
            if(markerIds[i] == 0)
            {
                relativeMarkerPosition = i; 
            }
           
        }
        Point3d relativeMarkerTvec = poseEstimationData.tvecs[relativeMarkerPosition];

        float tvecToMeters = (float) Math.Sqrt(Math.Pow(relativeMarkerTvec.x, 2) + Math.Pow(relativeMarkerTvec.y, 2) + Math.Pow(relativeMarkerTvec.z, 2)) / realSize;
        
        return tvecToMeters;
    }
    float tvecsToMetersCam(IntPtr markerCorners, int[] markerIds, IntPtr cameraMatrix, IntPtr distCoeffs)
    {
        // marker 0 is 11 cm away from
        float realSize = 0.11f;
        int originMarkerId = 30;
        int relativeMarkerPosition = 0;
        USingleMarkerPoseEstimationData poseEstimationData = Aruco.UEstimateSingleMarkerPose(markerCorners, cameraMatrix, distCoeffs);

        for(int i = 0; i< markerIds.Length; i++)
        {
            if(markerIds[i] == 0)
            {
                relativeMarkerPosition = i; 
            }
           
        }
        Point3d relativeMarkerTvec = poseEstimationData.tvecs[relativeMarkerPosition];

        float tvecToMeters = (float) Math.Sqrt(Math.Pow(Math.Pow(relativeMarkerTvec.x, -1), 2) + Math.Pow(Math.Pow(relativeMarkerTvec.y, -1), 2) + Math.Pow(Math.Pow(relativeMarkerTvec.z, -1), 2)) / realSize;
        
        return tvecToMeters;
    }
    void Transform3DObjects(List<List<Point2f>> markerList, int[] markersIds, Point3d rvec, Point3d tvec, IntPtr rvecPointer)
    {

      
        if(markerList.Count != 0)
        {
            // Vector3 eulerAngles = GetEulerAngles(rvecPointer);
            ///* replaced by get EulerAngles

            double[][] eulerAnglesList = Aruco.UGetEulerAngles(rvecPointer);

            int rowLength = eulerAnglesList.Length;
            int colLength = eulerAnglesList[0].Length;

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    eulerAnglesList[i][j] = Mathf.Rad2Deg * eulerAnglesList[i][j];
                }
            }

            float x = 960;
            float y = 540;
            // if(markersIds.Length == 12) 
            // {
                
            //     List<Point2f> markerCenterPoint = new List<Point2f>();
            //     foreach(List<Point2f> marker in markerList)
            //     {
            //         float marker_sum_x = 0;
            //         float marker_sum_y = 0;
            //         foreach(Point2f corner in marker)
            //         {
            //             marker_sum_x += Mathf.Round(corner.x);
            //             marker_sum_y += Mathf.Round(corner.y);

            //         }
            //         markerCenterPoint.Add(new Point2f(marker_sum_x / 4, marker_sum_y / 4));
            //     }
            //     float sum_x = 0;
            //     float sum_y = 0;
            //     foreach(Point2f center in markerCenterPoint) {
            //         sum_x += center.x;
            //         sum_y += center.y;
            //     }
            //     Point2f boardCenter = new Point2f(Mathf.Round(sum_x / markersIds.Length), Mathf.Round(sum_y / markersIds.Length));
            //     x = boardCenter.x;
            //     y = boardCenter.y;  
            // }


            _displayObject.position = new Vector3(-x‬, -y, 20);
            if(eulerAnglesList.Length > 0)
            {
                Debug.Log("x: "+ eulerAnglesList[0][0]+ " y: "+ eulerAnglesList[0][1]+ " z: "+ eulerAnglesList[0][2]);
                _displayObject.transform.rotation = Quaternion.Euler(new Vector3((float) eulerAnglesList[0][0], (float) eulerAnglesList[0][1], (float) eulerAnglesList[0][2]));
            }
            // Debug.Log("x: "+ tvec.x+ "tvec.y: "+ tvec.y);
            // if(tvec.x != 0 && tvec.y != 0 ) {
            //     Vector3 translateVec = new Vector3(Mathf.Pow((float)tvec.x, -1), (Mathf.Pow((float)tvec.y, -1) ), 0);
            //     _displayObject.transform.position =  translateVec;

            //     _displayObject.transform.position =  translateVec;
            //     _displayObject.transform.rotation =  Quaternion.Euler(new Vector3((float) eulerAnglesList[0][0], -1* (float) eulerAnglesList[0][1], (float) eulerAnglesList[0][2]));
            //     }

            // _displayObj  ect.position = new Vector3(-Mathf.Pow((float)tvec.y, -1) * scale + _displayObject.position.y,  Mathf.Pow((float)tvec.z, -1 ) * scale + _displayObject.position.x,  Mathf.Pow((float)tvec.z, -1 ) * scale + _displayObject.position.z); 
            // if(scale == 0)
            // {
            //     _displayObject.localScale = new Vector3( -Mathf.Pow((float)tvec.z, -1) * 200,  Mathf.Pow((float)tvec.z, -1 ) * 200,  Mathf.Pow((float)tvec.z, -1 ) * 200); 
            // } 
            // else
            // {
            //     _displayObject.localScale = new Vector3( -Mathf.Pow((float)tvec.z, -1) * scale * 10,  Mathf.Pow((float)tvec.z, -1 ) * scale * 10,  Mathf.Pow((float)tvec.z, -1 ) * scale * 10); 
            // }
            
         
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        // UDetectMarkersData _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
        UDetectMarkersData _markerData;
        if(!runningOnAndroid) 
        {
            bool spacePressed = Input.GetKeyDown(KeyCode.Space);
            if(notCalibrated && spacePressed)
            {
                if(!staticCalib)
                {
                    bool sucess = Aruco.UFindCharucoBoardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height,  allCharucoIds, allCharucoCorners);

                    if(sucess)
                    {
                        numOfSuccessfulFrames++;
                    }

                    if(numOfSuccessfulFrames >= 20)
                    {
                        calibData = Aruco.UCalibrateCameraCharuco(_webCamTexture.width, _webCamTexture.height,  allCharucoIds, allCharucoCorners);
                        CameraCalibSerializable calidSaveData;
                        calidSaveData.distortionCoefficients = (double[][]) calibData.distCoeffs.GetMangedObject();
                        calidSaveData.cameraMatrix = (double[][]) calibData.cameraMatrix.GetMangedObject();
                        Utilities.SaveCameraCalibrationParams(calidSaveData);
                        // calidSaveData = Utilities.LoadCameraCalibrationParams();
                        notCalibrated = false;
                    }
                }
                else 
                {
                    calibData = Aruco.UStaticCalibrateCameraData();
                    CameraCalibSerializable calidSaveData = Utilities.LoadCameraCalibrationParams();
                    MatDMarshaller distCoeffs = new MatDMarshaller(calidSaveData.distortionCoefficients);
                    MatDMarshaller cameraMatrix = new MatDMarshaller(calidSaveData.cameraMatrix);
                    calibData =  new UCameraCalibrationData(distCoeffs, cameraMatrix);
                    notCalibrated = false;
                }
                
            }
        } 
        else 
        {
            if(notCalibrated)
            {
                if(!staticCalib)
                {
                    if (Input.touchCount > 0)
                    {
                        bool sucess = Aruco.UFindCharucoBoardCorners(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height,  allCharucoIds, allCharucoCorners);

                        if(sucess)
                        {
                            numOfSuccessfulFrames++;
                        }

                        if(numOfSuccessfulFrames >= 40)
                        {
                            calibData = Aruco.UCalibrateCameraCharuco(_webCamTexture.width, _webCamTexture.height,  allCharucoIds, allCharucoCorners);

                            notCalibrated = false;
                        }
                    }
                    
                }
                else 
                {
                    calibData = Aruco.UStaticCalibrateCameraData();
                    notCalibrated = false;
                }
            } 
        }
        // Debug.Log(1/Time.deltaTime);
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


            */

            _markerData = Aruco.UDetectMarkers(_webCamTexture.GetPixels32(), _webCamTexture.width, _webCamTexture.height);
            if(_markerData.markerIds.Length > 0)
            {
                // UBoardMarkerPoseEstimationData poseEstimationData = Aruco.UEstimateCharucoBoardPose(_webCamTexture.GetPixels32(), _markerData.markersPointer, _markerData.markerIdsPointer, calibData.cameraMatrix.NativeDataPointer, calibData.distCoeffs.NativeDataPointer);
                UBoardMarkerPoseEstimationData poseEstimationData = Aruco.UEstimateBoardMarkerPose(_markerData.markersPointer, _markerData.markerIdsPointer, calibData.cameraMatrix.NativeDataPointer, calibData.distCoeffs.NativeDataPointer);
                // if(!scaleGotten &&  _markerData.markerIds.Length >= 35)
                // {
                //     scale = tvecsToMeters(_markerData.markersPointer, _markerData.markerIds, calibData.cameraMatrixPointer, calibData.distCoeffsPointer);
                //     scaleGotten = true;
                // }
                Point3d localPoseRvecs = poseEstimationData.rvec;
                Point3d localPoseTvecs = poseEstimationData.tvec;

                Transform3DObjects(_markerData.markers, _markerData.markerIds, localPoseRvecs, localPoseTvecs, poseEstimationData.rvecPointer);
            }
                // poseEstimationData = Aruco.UEstimateSingleMarkerPose(_markerData.markersPointer, calibData.cameraMatrixPointer, calibData.distCoeffsPointer);
                
        }
    }
}