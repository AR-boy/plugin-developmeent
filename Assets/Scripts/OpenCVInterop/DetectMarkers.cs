using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVInterop.Marshallers;
using UnityEngine;


namespace OpenCVInterop
{
    public struct UDetectMarkersData
    {
        public int[] markerIds;
        public List<List<Vector2>> markers;
        public List<List<Vector2>> rejectedCandidates;
        public IntPtr markerIdsPointer;
        public IntPtr markersPointer;
        public IntPtr rejectedCandidatesPointer;
        public UDetectMarkersData(int[] markerIds_param,  List<List<Vector2>> markers_param, List<List<Vector2>> rejectedCandidates_param, IntPtr markerIdsPointer_params,  IntPtr markersPointer_params,  IntPtr rejectedCandidatesPointer_params)
        {
            markerIds = markerIds_param;
            markers = markers_param;
            rejectedCandidates = rejectedCandidates_param;
            markerIdsPointer = markerIdsPointer_params;
            markersPointer = markersPointer_params;
            rejectedCandidatesPointer = rejectedCandidatesPointer_params;
        }
    }
    public static partial class Aruco
    {
        #if UNITY_EDITOR_WIN
            [DllImport("OpenCVUnity")]
            public unsafe static extern void DetectMarkers(
                Color32* textureData, int width, int height,
                IntPtr markerIds,  
                IntPtr markerCorners,
                IntPtr rejectedCandidates
            );
        #elif UNITY_STANDALONE_WIN
            [DllImport("OpenCVUnity")]
            public unsafe static extern void DetectMarkers(
                Color32* textureData, int width, int height,
                IntPtr markerIds,  
                IntPtr markerCorners,
                IntPtr rejectedCandidates
            );
        #elif UNITY_ANDROID
            [DllImport("OpenCVUnity")]
            public unsafe static extern void DetectMarkers(
                Color32* textureData, int width, int height,
                IntPtr markerIds,  
                IntPtr markerCorners,
                IntPtr rejectedCandidates
            );
        #endif

        unsafe public static UDetectMarkersData UDetectMarkers(Color32[] texture, int width, int height) 
        {   
            var watch = System.Diagnostics.Stopwatch.StartNew();
            VectorIntMarshaller markerIds = new VectorIntMarshaller();
            DoubleVectorPoint2FMarshaller markerCornerMarshaller = new DoubleVectorPoint2FMarshaller();
            DoubleVectorPoint2FMarshaller rejectedCandidateMarshaller = new DoubleVectorPoint2FMarshaller();


            fixed (Color32* texP = texture)
            {
                DetectMarkers(
                    texP, 
                    width, 
                    height, 
                    markerIds.NativeDataPointer, 
                    markerCornerMarshaller.NativeDataPointer, 
                    rejectedCandidateMarshaller.NativeDataPointer
                );
            }
            

            UDetectMarkersData markerData = new UDetectMarkersData(
                (int[]) markerIds.GetMangedObject(),
                (List<List<Vector2>>) markerCornerMarshaller.GetMangedObject(),
                (List<List<Vector2>>) rejectedCandidateMarshaller.GetMangedObject(),
                markerIds.NativeDataPointer,
                markerCornerMarshaller.NativeDataPointer,
                rejectedCandidateMarshaller.NativeDataPointer      
            );

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            
            return markerData;
        }
    }

} 
