using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVInterop.Marshallers;
using OpenCVInterop.Utilities;
using UnityEngine;
using Unity.Profiling;

namespace OpenCVInterop
{
    public struct USingleMarkerPoseEstimationData
    {
        public IntPtr rvecsPointer;
        public IntPtr tvecsPointer;
        public List<Vec3d> rvecs;
        public List<Vec3d> tvecs;
        public USingleMarkerPoseEstimationData(List<Vec3d> rvecs_param, List<Vec3d> tvecs_param, IntPtr rvecsPointer_param, IntPtr tvecsPointer_param)
        {
            rvecs = rvecs_param;
            tvecs = tvecs_param;
            rvecsPointer = rvecsPointer_param;
            tvecsPointer = tvecsPointer_param;
        }
    }
    public struct UBoardMarkerPoseEstimationData
    {
        public IntPtr rvecPointer;
        public IntPtr tvecPointer;
        public Vec3d rvec;
        public Vec3d tvec;
        public UBoardMarkerPoseEstimationData(Vec3d rvec_param, Vec3d tvec_param, IntPtr rvecPointer_param, IntPtr tvecPointer_param)
        {
            rvec = rvec_param;
            tvec = tvec_param;
            rvecPointer = rvecPointer_param;
            tvecPointer = tvecPointer_param;
        }
    }

    public struct UBoardMarkerPoseEstimationDataEuler
    {
        public Vec3d rvec;
        public Vec3d tvec;
        public double[][] eulerAngles;

        public UBoardMarkerPoseEstimationDataEuler(Vec3d rvec_param, Vec3d tvec_param, double[][] eulerAngles)
        {
            rvec = rvec_param;
            tvec = tvec_param;
            this.eulerAngles = eulerAngles;
        }
    }


    public static partial class Aruco
    {
        static ProfilerMarker estimateCharucoBoardPosePerfMarker = new ProfilerMarker("OpenCVIntrop.EstimateCharucoBoardPose");

        #if UNITY_EDITOR_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateSingleMarkerPose(
                float markerLength,
                IntPtr markerCorners,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvecs,
                IntPtr tvecs
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateArucoBoardPose(
                float markerLength,
                float markerSeparation,
                int markersX,
                int markersY,
                IntPtr markerCorners,
                IntPtr markerIds,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateCharucoBoardPose(
                Color32* textureData,
                int width,
                int height,
                float markerLength,
                float squareLength,
                int markersX,
                int markersY,
                IntPtr markerIds,
                IntPtr markerCorners,
                IntPtr rejectedCandidates,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateArucoBoardPoseV2(
                Color32* textureData,
                int width,
                int height,
                float markerLength,
                float markerSeparation,
                int markersX,
                int markersY,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec,
                IntPtr eulerAngles
            );

        #elif UNITY_STANDALONE_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateSingleMarkerPose(
                float markerLength,
                IntPtr markerCorners,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvecs,
                IntPtr tvecs
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateArucoBoardPose(
                float markerLength,
                float markerSeparation,
                int markersX,
                int markersY,
                IntPtr markerCorners,
                IntPtr markerIds,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateCharucoBoardPose(
                Color32* textureData,
                int width,
                int height,
                float markerLength,
                float squareLength,
                int markersX,
                int markersY,
                IntPtr markerIds,
                IntPtr markerCorners,
                IntPtr rejectedCandidates,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateArucoBoardPoseV2(
                Color32* textureData,
                int width,
                int height,
                float markerLength,
                float markerSeparation,
                int markersX,
                int markersY,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec,
                IntPtr eulerAngles
            );
           
        #elif UNITY_ANDROID
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateSingleMarkerPose(
                float markerLength,
                IntPtr markerCorners,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvecs,
                IntPtr tvecs
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateArucoBoardPose(
                float markerLength,
                float markerSeparation,
                int markersX,
                int markersY,
                IntPtr markerCorners,
                IntPtr markerIds,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateCharucoBoardPose(
                Color32* textureData,
                int width,
                int height,
                float markerLength,
                float squareLength,
                int markersX,
                int markersY,
                IntPtr markerIds,
                IntPtr markerCorners,
                IntPtr rejectedCandidates,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec,
                IntPtr eulerAngles
            );
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateArucoBoardPoseV2(
                Color32* textureData,
                int width,
                int height,
                float markerLength,
                float markerSeparation,
                int markersX,
                int markersY,
                IntPtr cameraMatrix,
                IntPtr distCoeffs,
                IntPtr rvec,
                IntPtr tvec,
                IntPtr eulerAngles
            );
        #endif

        public static USingleMarkerPoseEstimationData UEstimateSingleMarkerPose(DoubleVectorPoint2FMarshaller markerCorners, int markerLenght, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.045f;
            VectorVec3dMarshaller rvecs = new VectorVec3dMarshaller();
            VectorVec3dMarshaller tvecs = new VectorVec3dMarshaller();

            EstimateSingleMarkerPose(
                markerLength, 
                markerCorners.NativeDataPointer, 
                cameraMatrix, 
                distCoeffs, 
                rvecs.NativeDataPointer, 
                tvecs.NativeDataPointer
            );


            USingleMarkerPoseEstimationData PoseEstimateData = new USingleMarkerPoseEstimationData(
                (List<Vec3d>) rvecs.GetMangedObject(),
                (List<Vec3d>) tvecs.GetMangedObject(),
                rvecs.NativeDataPointer,
                tvecs.NativeDataPointer
            );

            return PoseEstimateData;
        }

        public unsafe static UBoardMarkerPoseEstimationDataEuler UEstimateArucoBoardPose(Color32[] texture, int width, int height, ArucoBoardParameters boardParameters, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.04f;
            float markerSeparation = 0.01f;
            int markersX = 4;
            int markersY = 3;

            Vec3dMarshaller tvecMarshaller = new Vec3dMarshaller();
            Vec3dMarshaller rvecMarshaller = new Vec3dMarshaller();
            MatDoubleMarshaller eulerAngles = new MatDoubleMarshaller();

            
            fixed (Color32* texP = texture)
            {
                EstimateArucoBoardPoseV2(
                    texP,
                    width,
                    height,
                    boardParameters.markerLength,
                    boardParameters.markerSeperation,
                    boardParameters.squaresW,
                    boardParameters.squaresH,
                    cameraMatrix,
                    distCoeffs,
                    rvecMarshaller.NativeDataPointer,
                    tvecMarshaller.NativeDataPointer,
                    eulerAngles.NativeDataPointer
                );
            }


            UBoardMarkerPoseEstimationDataEuler PoseEstimateData = new UBoardMarkerPoseEstimationDataEuler(
                (Vec3d) rvecMarshaller.GetMangedObject(),
                (Vec3d) tvecMarshaller.GetMangedObject(),
                (double[][]) eulerAngles.GetMangedObject()
            );

            return PoseEstimateData;
        }


        public static UBoardMarkerPoseEstimationData UEstimateBoardMarkerPose(IntPtr markerCorners, int width, int height, ArucoBoardParameters boardParameters, IntPtr markerIds, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.04f;
            float markerSeparation = 0.01f;
            int markersX = 4;
            int markersY = 3;

            Vec3dMarshaller tvecMarshaller = new Vec3dMarshaller();
            Vec3dMarshaller rvecMarshaller = new Vec3dMarshaller();

            EstimateArucoBoardPose(
                boardParameters.markerLength,
                boardParameters.markerSeperation,
                boardParameters.squaresW,
                boardParameters.squaresH,
                markerCorners,
                markerIds,
                cameraMatrix,
                distCoeffs,
                rvecMarshaller.NativeDataPointer,
                tvecMarshaller.NativeDataPointer
            );

            rvecMarshaller.MarshalNativeToManaged();
            tvecMarshaller.MarshalNativeToManaged();

            UBoardMarkerPoseEstimationData PoseEstimateData = new UBoardMarkerPoseEstimationData(
                (Vec3d) rvecMarshaller.GetMangedObject(),
                (Vec3d) tvecMarshaller.GetMangedObject(),
                rvecMarshaller.NativeDataPointer,
                tvecMarshaller.NativeDataPointer
            );

            return PoseEstimateData;
        }

        public unsafe static (UDetectMarkersData, UBoardMarkerPoseEstimationDataEuler) UEstimateCharucoBoardPose(Color32[] texture, int width, int height, CharucoBoardParameters boardParameters, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            int cornersH = 3;
            int cornersW = 4;
            float markerLength = 0.045f;
            float squareLength = 0.06f;
            // marshallers
            Vec3dMarshaller tvecMarshaller = new Vec3dMarshaller();
            Vec3dMarshaller rvecMarshaller = new Vec3dMarshaller();
            MatDoubleMarshaller eulerAngles = new MatDoubleMarshaller();
            // marker pointers
            VectorIntMarshaller markerIds = new VectorIntMarshaller();
            DoubleVectorPoint2FMarshaller markerCornerMarshaller = new DoubleVectorPoint2FMarshaller();
            DoubleVectorPoint2FMarshaller rejectedCandidateMarshaller = new DoubleVectorPoint2FMarshaller();

            estimateCharucoBoardPosePerfMarker.Begin();    
            fixed (Color32* texP = texture)
            {
                EstimateCharucoBoardPose(
                    texP,
                    width,
                    height,
                    // markerLength,
                    // squareLength,
                    // cornersW,
                    // cornersH,
                    boardParameters.markerLength,
                    boardParameters.squareLength,
                    boardParameters.squaresW,
                    boardParameters.squaresH,
                    markerIds.NativeDataPointer,
                    markerCornerMarshaller.NativeDataPointer,
                    rejectedCandidateMarshaller.NativeDataPointer,
                    cameraMatrix,
                    distCoeffs,
                    rvecMarshaller.NativeDataPointer,
                    tvecMarshaller.NativeDataPointer,
                    eulerAngles.NativeDataPointer
                );
            }
            estimateCharucoBoardPosePerfMarker.End();   

            UDetectMarkersData markerData = new UDetectMarkersData(
                (int[]) markerIds.GetMangedObject(),
                (List<List<Vector2>>) markerCornerMarshaller.GetMangedObject(),
                (List<List<Vector2>>) rejectedCandidateMarshaller.GetMangedObject(),
                markerIds.NativeDataPointer,
                markerCornerMarshaller.NativeDataPointer,
                rejectedCandidateMarshaller.NativeDataPointer      
            );

            UBoardMarkerPoseEstimationDataEuler PoseEstimateData = new UBoardMarkerPoseEstimationDataEuler(
                (Vec3d) rvecMarshaller.GetMangedObject(),
                (Vec3d) tvecMarshaller.GetMangedObject(),
                (double[][]) eulerAngles.GetMangedObject()
            );

            return (markerData: markerData, PoseEstimateData: PoseEstimateData);
        }

    }
}