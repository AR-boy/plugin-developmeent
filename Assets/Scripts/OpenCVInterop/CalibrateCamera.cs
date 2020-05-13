using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVInterop.Marshallers;
using OpenCVInterop.Utilities;
using UnityEngine;


namespace OpenCVInterop
{

    public struct UCameraCalibrationData
    {
        public MatDoubleMarshaller cameraMatrix;
        public MatDoubleMarshaller distCoeffs;
        public UCameraCalibrationData(MatDoubleMarshaller distCoeffs, MatDoubleMarshaller cameraMatrix)
        {
            this.distCoeffs = distCoeffs;
            this.cameraMatrix = cameraMatrix;
        }
    }
    public static partial class Aruco
    {
        #if UNITY_EDITOR_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern void FindChessboardCorners(
                Color32* textureData, int width, int height,
                int cornersW, int cornersH,
                float cornerLength, float cornerSeparation,
                IntPtr foundBoardMarkers,
                IntPtr image_points, 
                IntPtr object_points
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void DetectCharucoBoard(
                Color32* textureData,
                int width,
                int height,
                int squaresW,
                int squaresH,
                float squareLength,
                float markersLength,
                IntPtr foundBoardMarkers,
                IntPtr allCharucoIds,
                IntPtr allCharucoCorners
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void CalibrateCameraCharuco(
                int height,
                int width,
                int squaresW,
                int squaresH,
                float squareLength,
                float markersLength,
                IntPtr allCharucoIds,
                IntPtr allCharucoCorners,
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void CalibrateCamera(
                Color32* textureData, int width, int height,
                IntPtr image_points,  
                IntPtr object_points,
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients
            );
            [DllImport("OpenCVUnity")]
            public static extern void StaticCameraCalibData(
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients,
                bool isSevenTwenty
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorToEulerAngles(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorToEulerAnglesV2(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorsToEulerAngles(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateBooleanPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteBooleanPointer(IntPtr pointer);

        #elif UNITY_STANDALONE_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern void FindChessboardCorners(
                Color32* textureData, int width, int height,
                int cornersW, int cornersH,
                float cornerLength, float cornerSeparation,
                IntPtr foundBoardMarkers,
                IntPtr image_points, 
                IntPtr object_points
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void DetectCharucoBoard(
                Color32* textureData,
                int width,
                int height,
                int squaresW,
                int squaresH,
                float squareLength,
                float markersLength,
                IntPtr foundBoardMarkers,
                IntPtr allCharucoIds,
                IntPtr allCharucoCorners
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void CalibrateCameraCharuco(
                int height,
                int width,
                int squaresW,
                int squaresH,
                float squareLength,
                float markersLength,
                IntPtr allCharucoIds,
                IntPtr allCharucoCorners,
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void CalibrateCamera(
                Color32* textureData, int width, int height,
                IntPtr image_points,  
                IntPtr object_points,
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients
            );
            [DllImport("OpenCVUnity")]
            public static extern void StaticCameraCalibData(
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients,
                bool isSevenTwenty
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorToEulerAngles(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorToEulerAnglesV2(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorsToEulerAngles(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateBooleanPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteBooleanPointer(IntPtr pointer);

        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern void FindChessboardCorners(
                Color32* textureData, int width, int height,
                int cornersW, int cornersH,
                float cornerLength, float cornerSeparation,
                IntPtr foundBoardMarkers,
                IntPtr image_points, 
                IntPtr object_points
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void DetectCharucoBoard(
                Color32* textureData,
                int width,
                int height,
                int squaresW,
                int squaresH,
                float squareLength,
                float markersLength,
                IntPtr foundBoardMarkers,
                IntPtr allCharucoIds,
                IntPtr allCharucoCorners
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void CalibrateCameraCharuco(
                int height,
                int width,
                int squaresW,
                int squaresH,
                float squareLength,
                float markersLength,
                IntPtr allCharucoIds,
                IntPtr allCharucoCorners,
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void CalibrateCamera(
                Color32* textureData, int width, int height,
                IntPtr image_points,  
                IntPtr object_points,
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients
            );
            [DllImport("OpenCVUnity")]
            public static extern void StaticCameraCalibData(
                IntPtr cameraMatrix,
                IntPtr distortionCoefficients,
                bool isSevenTwenty
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorToEulerAngles(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorToEulerAnglesV2(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public static extern void RotationVectorsToEulerAngles(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateBooleanPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteBooleanPointer(IntPtr pointer);
        #endif


        unsafe public static bool UFindCharucoBoardCorners(Color32[] texture, int width, int height, CharucoBoardParameters boardParameters, DoubleVectorIntMarshaller allCharucoIds, DoubleVectorPoint2FMarshaller allCharucoCorners)
        {
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            int cornersH = 9;
            int cornersW = 6;
            float squareLength = 0.025f;
            float markersLength = 0.0125f;
            bool foundBoard = false;
            IntPtr foundBoardMarkers = CreateBooleanPointer();

            fixed (Color32* texP = texture)
            {
                DetectCharucoBoard(
                    texP,
                    width,
                    height,
                    boardParameters.squaresW,
                    boardParameters.squaresH,
                    boardParameters.squareLength,
                    boardParameters.markerLength,
                    foundBoardMarkers,
                    allCharucoIds.NativeDataPointer,
                    allCharucoCorners.NativeDataPointer
                );
            }
            


            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            if(Marshal.ReadByte(foundBoardMarkers, 0) > 0)
            {
                foundBoard =  true;
            }

            DeleteBooleanPointer(foundBoardMarkers);

            return foundBoard;
        }
        unsafe public static bool UFindChessboardCorners(Color32[] texture, int width, int height, ArucoBoardParameters boardParameters, IntPtr imagePoints, IntPtr objectPoints)
        {
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            int cornersH = 9;
            int cornersW = 6;
            float cornerLength = 0.024f;
            float cornerSeparation = 0.024f;
            bool foundBoard = false;
            IntPtr foundBoardMarkers = CreateBooleanPointer();

            fixed (Color32* texP = texture)
            {
                FindChessboardCorners(
                    texP,
                    width,
                    height,
                    boardParameters.squaresW,
                    boardParameters.squaresH,
                    boardParameters.markerLength,
                    boardParameters.markerSeperation,
                    foundBoardMarkers,
                    imagePoints,
                    objectPoints
                );
            }
            


            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            if(Marshal.ReadByte(foundBoardMarkers, 0) > 0)
            {
                foundBoard =  true;
            }

            DeleteBooleanPointer(foundBoardMarkers);

            return foundBoard;
        }  
        public static UCameraCalibrationData UStaticCalibrateCameraData() 
        {   
            
            MatDoubleMarshaller distCoeffs = new MatDoubleMarshaller();
            MatDoubleMarshaller cameraMatrix = new MatDoubleMarshaller();

            StaticCameraCalibData(
                cameraMatrix.NativeDataPointer,
                distCoeffs.NativeDataPointer,
                true
            );

            UCameraCalibrationData calibrationData = new UCameraCalibrationData(
                distCoeffs,
                cameraMatrix
            );
            
            return calibrationData;
        }
        public static double[][] UGetEulerAngles(IntPtr rvec)
        {

            MatDoubleMarshaller eulerAngles = new MatDoubleMarshaller();
            RotationVectorToEulerAnglesV2(
                rvec,
                eulerAngles.NativeDataPointer
            );

            double[][] eulerAngleValues = (double[][]) eulerAngles.GetMangedObject();

            return eulerAngleValues;
        }
        // rename needed
        public static double[][] UGetEulerAnglesMultiple(IntPtr rvecs)
        {
            MatDoubleMarshaller matDoubleMarshaller = new MatDoubleMarshaller();
            RotationVectorsToEulerAngles(
                rvecs,
                matDoubleMarshaller.NativeDataPointer
            );

            double[][] eulerAngles = (double[][]) matDoubleMarshaller.GetMangedObject();

            return eulerAngles;
        }
        unsafe public static UCameraCalibrationData UCalibrateCamera(Color32[] texture, int width, int height, DoubleVectorIntMarshaller allCharucoIds, DoubleVectorPoint2FMarshaller allCharucoCorners) 
        {   

            var watch = System.Diagnostics.Stopwatch.StartNew();

            // IntPtr distCoeffs = OpenCVMarshal.CreateMatPointer();
            MatDoubleMarshaller distCoeffs = new MatDoubleMarshaller();
            // IntPtr cameraMatrix = OpenCVMarshal.CreateMatPointer();
            MatDoubleMarshaller cameraMatrix = new MatDoubleMarshaller();

            fixed (Color32* texP = texture)
            {
                CalibrateCamera(
                    texP,
                    width,
                    height,
                    allCharucoIds.NativeDataPointer,
                    allCharucoCorners.NativeDataPointer,
                    cameraMatrix.NativeDataPointer,
                    distCoeffs.NativeDataPointer
                );
            }


            UCameraCalibrationData calibrationData = new UCameraCalibrationData(
                distCoeffs,
                cameraMatrix
            );

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            
            return calibrationData;
        }
        unsafe public static UCameraCalibrationData UCalibrateCameraCharuco(int width, int height, CharucoBoardParameters boardParameters, DoubleVectorIntMarshaller allCharucoIds, DoubleVectorPoint2FMarshaller allCharucoCorners) 
        {   

            var watch = System.Diagnostics.Stopwatch.StartNew();
            int cornersH = 9;
            int cornersW = 6;
            float squareLength = 0.025f;
            float markersLength = 0.0125f;
            // IntPtr distCoeffs = OpenCVMarshal.CreateMatPointer();
            MatDoubleMarshaller distCoeffs = new MatDoubleMarshaller();
            // IntPtr cameraMatrix = OpenCVMarshal.CreateMatPointer();
            MatDoubleMarshaller cameraMatrix = new MatDoubleMarshaller();

            CalibrateCameraCharuco(
                height,
                width,
                boardParameters.squaresW,
                boardParameters.squaresH,
                boardParameters.squareLength,
                boardParameters.markerLength,
                allCharucoIds.NativeDataPointer,
                allCharucoCorners.NativeDataPointer,
                cameraMatrix.NativeDataPointer,
                distCoeffs.NativeDataPointer
            );


            UCameraCalibrationData calibrationData = new UCameraCalibrationData(
                distCoeffs,
                cameraMatrix
            );

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            
            return calibrationData;
        }
    }

} 