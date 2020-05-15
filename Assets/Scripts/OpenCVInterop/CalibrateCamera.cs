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
        public double reProjectionError;
        public UCameraCalibrationData(MatDoubleMarshaller distCoeffs, MatDoubleMarshaller cameraMatrix, double reProjectionError)
        {
            this.distCoeffs = distCoeffs;
            this.cameraMatrix = cameraMatrix;
            this.reProjectionError = reProjectionError;
        }
    }
    public static partial class Aruco
    {
        #if UNITY_EDITOR_WIN

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
            public unsafe static extern double CalibrateCameraCharuco(
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
            public unsafe static extern IntPtr CreateBooleanPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteBooleanPointer(IntPtr pointer);

        #elif UNITY_STANDALONE_WIN

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
            public unsafe static extern double CalibrateCameraCharuco(
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
            public unsafe static extern IntPtr CreateBooleanPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteBooleanPointer(IntPtr pointer);

        #elif UNITY_ANDROID

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
            public unsafe static extern double CalibrateCameraCharuco(
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
            public unsafe static extern IntPtr CreateBooleanPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteBooleanPointer(IntPtr pointer);
        #endif


        // find charuco board corners in frame with given board parameters and imbues marshaller objects with information found 
        unsafe public static bool UFindCharucoBoardCorners(Color32[] texture, int width, int height, CharucoBoardParameters boardParameters, DoubleVectorIntMarshaller allCharucoIds, DoubleVectorPoint2FMarshaller allCharucoCorners)
        {

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

            if(Marshal.ReadByte(foundBoardMarkers, 0) > 0)
            {
                foundBoard =  true;
            }

            DeleteBooleanPointer(foundBoardMarkers);

            return foundBoard;
        } 
        // find calibrates camera with collected data found by UFindCharucoBoardCorners method
        unsafe public static UCameraCalibrationData UCalibrateCameraCharuco(int width, int height, CharucoBoardParameters boardParameters, DoubleVectorIntMarshaller allCharucoIds, DoubleVectorPoint2FMarshaller allCharucoCorners) 
        {   
            MatDoubleMarshaller distCoeffs = new MatDoubleMarshaller();
            MatDoubleMarshaller cameraMatrix = new MatDoubleMarshaller();

            double reProjectionError = CalibrateCameraCharuco(
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
                cameraMatrix,
                reProjectionError
            );
            
            return calibrationData;
        }
    }

} 