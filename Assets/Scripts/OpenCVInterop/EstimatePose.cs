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
        public List<Point3d> rvecs;
        public List<Point3d> tvecs;
        public USingleMarkerPoseEstimationData(List<Point3d> rvecs_param, List<Point3d> tvecs_param)
        {
            rvecs = rvecs_param;
            tvecs = tvecs_param;
        }
    }    public static partial class Aruco
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
        #elif UNITY_STANDALONE_WIN
           
        #elif UNITY_ANDROID
           
        #endif

        public static USingleMarkerPoseEstimationData UEstimateSingleMarkerPose(IntPtr markerCorners, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.024f;

            IntPtr rvecs = OpenCVMarshal.CreateVectorVec3dPointer();
            IntPtr tvecs = OpenCVMarshal.CreateVectorVec3dPointer();

            EstimateSingleMarkerPose(markerLength, markerCorners, cameraMatrix, distCoeffs, rvecs, tvecs);

            VectorVec3dFMarshaller vectorVec3Marshaller = new VectorVec3dFMarshaller();

            USingleMarkerPoseEstimationData PoseEstimateData = new USingleMarkerPoseEstimationData(
                (List<Point3d>) vectorVec3Marshaller.MarshalNativeToManaged(rvecs),
                (List<Point3d>) vectorVec3Marshaller.MarshalNativeToManaged(tvecs)
            );

            return PoseEstimateData;
        }
    }
}