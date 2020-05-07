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

    public struct UBoardMarkerPoseEstimationDataEuler
    {
        public Point3d rvec;
        public Point3d tvec;
        public double[][] eulerAngles;

        public UBoardMarkerPoseEstimationDataEuler(Point3d rvec_param, Point3d tvec_param, double[][] eulerAngles)
        {
            rvec = rvec_param;
            tvec = tvec_param;
            this.eulerAngles = eulerAngles;
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
            [DllImport("OpenCVUnity")]
            public unsafe static extern void EstimateCharucoBoardPose(
                Color32* textureData,
                int width,
                int height,
                float markerLength,
                float squareLength,
                int markersX,
                int markersY,
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

        public unsafe static UBoardMarkerPoseEstimationDataEuler UEstimateArucoBoardPose(Color32[] texture, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.04f;
            float markerSeparation = 0.01f;
            int staticWidth = 1920;
            int staticHeight = 1080;
            int markersX = 4;
            int markersY = 3;
            // int markersY = 9;
            // int markersX = 6;
            // float markerSeparation = 0.053f;
            // float markerLength = 0.0125f;

            Vec3dMarshaller tvecMarshaller = new Vec3dMarshaller();
            Vec3dMarshaller rvecMarshaller = new Vec3dMarshaller();
            MatDMarshaller eulerAngles = new MatDMarshaller();

            
            fixed (Color32* texP = texture)
            {
                EstimateArucoBoardPoseV2(
                    texP,
                    staticWidth,
                    staticHeight,
                    markerLength,
                    markerSeparation,
                    markersX,
                    markersY,
                    cameraMatrix,
                    distCoeffs,
                    rvecMarshaller.NativeDataPointer,
                    tvecMarshaller.NativeDataPointer,
                    eulerAngles.NativeDataPointer
                );
            }

            rvecMarshaller.MarshalNativeToManaged();
            tvecMarshaller.MarshalNativeToManaged();
            eulerAngles.MarshalNativeToManaged();

            UBoardMarkerPoseEstimationDataEuler PoseEstimateData = new UBoardMarkerPoseEstimationDataEuler(
                (Point3d) rvecMarshaller.GetMangedObject(),
                (Point3d) tvecMarshaller.GetMangedObject(),
                (double[][]) eulerAngles.GetMangedObject()
            );

            return PoseEstimateData;
        }


        public static UBoardMarkerPoseEstimationData UEstimateBoardMarkerPose(IntPtr markerCorners, IntPtr markerIds, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            float markerLength = 0.04f;
            float markerSeparation = 0.01f;
            int markersX = 4;
            int markersY = 3;
            // int markersY = 9;
            // int markersX = 6;
            // float markerSeparation = 0.053f;
            // float markerLength = 0.0125f;

            Vec3dMarshaller tvecMarshaller = new Vec3dMarshaller();
            Vec3dMarshaller rvecMarshaller = new Vec3dMarshaller();

            EstimateArucoBoardPose(
                markerLength,
                markerSeparation,
                markersX,
                markersY,
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
                (Point3d) rvecMarshaller.GetMangedObject(),
                (Point3d) tvecMarshaller.GetMangedObject(),
                rvecMarshaller.NativeDataPointer,
                tvecMarshaller.NativeDataPointer
            );

            return PoseEstimateData;
        }

        public unsafe static UBoardMarkerPoseEstimationDataEuler UEstimateCharucoBoardPose(Color32[] texture, IntPtr cameraMatrix, IntPtr distCoeffs)
        {
            int width = 1920;
            int height = 1080;
            int cornersH = 3;
            int cornersW = 4;
            float markerLength = 0.045f;
            float squareLength = 0.06f;

            Vec3dMarshaller tvecMarshaller = new Vec3dMarshaller();
            Vec3dMarshaller rvecMarshaller = new Vec3dMarshaller();
            MatDMarshaller eulerAngles = new MatDMarshaller();

            fixed (Color32* texP = texture)
            {
                EstimateCharucoBoardPose(
                    texP,
                    width,
                    height,
                    markerLength,
                    squareLength,
                    cornersW,
                    cornersH,
                    cameraMatrix,
                    distCoeffs,
                    rvecMarshaller.NativeDataPointer,
                    tvecMarshaller.NativeDataPointer,
                    eulerAngles.NativeDataPointer
                );
            }
            
            UBoardMarkerPoseEstimationDataEuler PoseEstimateData = new UBoardMarkerPoseEstimationDataEuler(
                (Point3d) rvecMarshaller.GetMangedObject(),
                (Point3d) tvecMarshaller.GetMangedObject(),
                (double[][]) eulerAngles.GetMangedObject()
            );

            return PoseEstimateData;
        }

    }
}