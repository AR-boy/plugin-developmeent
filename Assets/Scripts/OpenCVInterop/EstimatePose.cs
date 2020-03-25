using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVInterop.MarshalOpenCV;
using UnityEngine;


namespace OpenCVInterop
{
    public struct USingleMarkerPoseEstimationData
    {
        public IntPtr rvecsPointer;
        public IntPtr tvecsPointer;
        public List<Point3d> rvecs;
        public List<Point3d> tvecs;
        public USingleMarkerPoseEstimationData(List<Point3d> rvecs_param, List<Point3d> tvecs_param, IntPtr rvecsPointer_param, IntPtr tvecsPointer_param)
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
        public Point3d rvec;
        public Point3d tvec;
        public UBoardMarkerPoseEstimationData(Point3d rvec_param, Point3d tvec_param, IntPtr rvecPointer_param, IntPtr tvecPointer_param)
        {
            rvec = rvec_param;
            tvec = tvec_param;
            rvecPointer = rvecPointer_param;
            tvecPointer = tvecPointer_param;
        }
    }
    public static partial class Aruco
    {
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
        #elif UNITY_STANDALONE_WIN
           
        #elif UNITY_ANDROID
           
        #endif

        public static USingleMarkerPoseEstimationData UEstimateSingleMarkerPose(IntPtr markerCorners, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.045f;

            IntPtr rvecs = OpenCVMarshal.CreateVectorVec3dPointer();
            IntPtr tvecs = OpenCVMarshal.CreateVectorVec3dPointer();

            EstimateSingleMarkerPose(markerLength, markerCorners, cameraMatrix, distCoeffs, rvecs, tvecs);

            VectorVec3dFMarshaller vectorVec3Marshaller = new VectorVec3dFMarshaller();

            USingleMarkerPoseEstimationData PoseEstimateData = new USingleMarkerPoseEstimationData(
                (List<Point3d>) vectorVec3Marshaller.MarshalNativeToManaged(rvecs),
                (List<Point3d>) vectorVec3Marshaller.MarshalNativeToManaged(tvecs),
                rvecs,
                tvecs
            );

            return PoseEstimateData;
        }

        public static UBoardMarkerPoseEstimationData UEstimateBoardMarkerPose(IntPtr markerCorners, IntPtr markerIds, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.04f;
            float markerSeparation = 0.01f;
            int markersX = 5;
            int markersY = 7;

            IntPtr rvec = OpenCVMarshal.CreateVec3dPointer();
            IntPtr tvec = OpenCVMarshal.CreateVec3dPointer();
            int numOfMarker = OpenCVMarshal.GetVectorIntSize(markerIds);


            EstimateArucoBoardPose(
                markerLength,
                markerSeparation,
                markersX,
                markersY,
                markerCorners,
                markerIds,
                cameraMatrix,
                distCoeffs,
                rvec,
                tvec
            );

            Vec3dMarshaller Vec3dMarshaller = new Vec3dMarshaller();

            UBoardMarkerPoseEstimationData PoseEstimateData = new UBoardMarkerPoseEstimationData(
                (Point3d) Vec3dMarshaller.MarshalNativeToManaged(rvec),
                (Point3d) Vec3dMarshaller.MarshalNativeToManaged(tvec),
                rvec,
                tvec
            );

            return PoseEstimateData;
        }
    }
}