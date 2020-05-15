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

        #elif UNITY_STANDALONE_WIN

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
           
        #elif UNITY_ANDROID

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
            
        #endif

        public unsafe static (UDetectMarkersData, UBoardMarkerPoseEstimationDataEuler) UEstimateCharucoBoardPose(Color32[] texture, int width, int height, CharucoBoardParameters boardParameters, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
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