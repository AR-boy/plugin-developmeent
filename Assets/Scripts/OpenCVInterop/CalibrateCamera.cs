using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVInterop.MarshalOpenCV;
using UnityEngine;


namespace OpenCVInterop
{
    /*
    public struct UCameraCalibrationData
    {
        public double[][] distortionCoefficients;
        public double[][] cameraMatrix;
        public IntPtr distCoeffsPointer;
        public IntPtr cameraMatrixPointer;
        public UCameraCalibrationData(double[][] distortionCoefficients_param, double[][] cameraMatrix_param, IntPtr distCoeffsPointer_param, IntPtr cameraMatrixPointer_param)
        {
            distortionCoefficients = distortionCoefficients_param;
            cameraMatrix = cameraMatrix_param;
            distCoeffsPointer = distCoeffsPointer_param;
            cameraMatrixPointer = cameraMatrixPointer_param;
        }
    }
    */
    public struct UCameraCalibrationData
    {
        public MatDMarshaller cameraMatrix;
        public MatDMarshaller distCoeffs;
        public UCameraCalibrationData(MatDMarshaller distCoeffs_param, MatDMarshaller cameraMatrix_param)
        {
            distCoeffs = distCoeffs_param;
            cameraMatrix = cameraMatrix_param;
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
            public static extern void RotationVectorsToEulerAngles(
                IntPtr rvecs,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateBooleanPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteBooleanPointer(IntPtr pointer);
        #endif


        unsafe public static bool UFindCharucoBoardCorners(Color32[] texture, int width, int height, DoubleVectorIntMarshaller allCharucoIds, IntPtr allCharucoCorners)
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
                    cornersW,
                    cornersH,
                    squareLength,
                    markersLength,
                    foundBoardMarkers,
                    allCharucoIds.NativeDataPointer,
                    allCharucoCorners
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
        unsafe public static bool UFindChessboardCorners(Color32[] texture, int width, int height, IntPtr imagePoints, IntPtr objectPoints)
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
                    cornersW,
                    cornersH,
                    cornerLength,
                    cornerSeparation,
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
            
            MatDMarshaller distCoeffs = new MatDMarshaller();
            MatDMarshaller cameraMatrix = new MatDMarshaller();

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

            MatDMarshaller eulerAngles = new MatDMarshaller();
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
            MatDMarshaller matDoubleMarshaller = new MatDMarshaller();
            RotationVectorsToEulerAngles(
                rvecs,
                matDoubleMarshaller.NativeDataPointer
            );

            double[][] eulerAngles = (double[][]) matDoubleMarshaller.GetMangedObject();

            return eulerAngles;
        }
        unsafe public static UCameraCalibrationData UCalibrateCamera(Color32[] texture, int width, int height, DoubleVectorIntMarshaller allCharucoIds, IntPtr allCharucoCorners) 
        {   

            var watch = System.Diagnostics.Stopwatch.StartNew();

            // IntPtr distCoeffs = OpenCVMarshal.CreateMatPointer();
            MatDMarshaller distCoeffs = new MatDMarshaller();
            // IntPtr cameraMatrix = OpenCVMarshal.CreateMatPointer();
            MatDMarshaller cameraMatrix = new MatDMarshaller();

            fixed (Color32* texP = texture)
            {
                CalibrateCamera(
                    texP,
                    width,
                    height,
                    allCharucoIds.NativeDataPointer,
                    allCharucoCorners,
                    cameraMatrix.NativeDataPointer,
                    distCoeffs.NativeDataPointer
                );
            }
            
            OpenCVMarshal.DeleteDoubleVector3PointFPointer(allCharucoCorners);


            UCameraCalibrationData calibrationData = new UCameraCalibrationData(
                distCoeffs,
                cameraMatrix
            );

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            
            return calibrationData;
        }
        unsafe public static UCameraCalibrationData UCalibrateCameraCharuco(int width, int height, DoubleVectorIntMarshaller allCharucoIds, IntPtr allCharucoCorners) 
        {   

            var watch = System.Diagnostics.Stopwatch.StartNew();
            int cornersH = 9;
            int cornersW = 6;
            float squareLength = 0.025f;
            float markersLength = 0.0125f;
            // IntPtr distCoeffs = OpenCVMarshal.CreateMatPointer();
            MatDMarshaller distCoeffs = new MatDMarshaller();
            // IntPtr cameraMatrix = OpenCVMarshal.CreateMatPointer();
            MatDMarshaller cameraMatrix = new MatDMarshaller();

            CalibrateCameraCharuco(
                height,
                width,
                cornersW,
                cornersH,
                squareLength,
                markersLength,
                allCharucoIds.NativeDataPointer,
                allCharucoCorners,
                cameraMatrix.NativeDataPointer,
                distCoeffs.NativeDataPointer
            );
            
            OpenCVMarshal.DeleteDoubleVector3PointFPointer(allCharucoCorners);


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