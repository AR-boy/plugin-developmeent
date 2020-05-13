// native c#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
// unity 
using UnityEngine;


namespace OpenCVInterop.Marshallers
{
    public static partial class Marshaller
    {
        #if UNITY_EDITOR_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVector2PointFPointer(IntPtr nativeDataPointer);

        #elif UNITY_STANDALONE_WIN
        
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVector2PointFPointer(IntPtr nativeDataPointer);


        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVector2PointFPointer(IntPtr nativeDataPointer);

        #endif
    }

    public class VectorPoint2FMarshaller : CustomMarshaller
    {
        private List<Vector2> _vectorPoint2F;
        private IntPtr _nativeDataPointer;
        public VectorPoint2FMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public VectorPoint2FMarshaller(List<Vector2>  _vectorPoint2F) {
            this._vectorPoint2F = _vectorPoint2F;
        }
        public VectorPoint2FMarshaller() {
            NativeDataPointer = Marshaller.CreateVector2PointFPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._vectorPoint2F == null) {
                this.MarshalNativeToManaged();
            }
            return this._vectorPoint2F;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteVector2PointFPointer(NativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   

            List<Vector2> ListOfPoint2f = new List<Vector2>();
            int numOf2PointF = Marshaller.GetVector2PointFPointerSize(NativeDataPointer);

            if(numOf2PointF > 0)
            {
                IntPtr nativeInternalDataPointer = Marshaller.GetVector2PointFPointerData(NativeDataPointer);
                float[] coordStream = new float[numOf2PointF * 2];

                // iterate over bytes in contigous memory to copy in float values from native stream to managed array
                Marshal.Copy(nativeInternalDataPointer, coordStream, 0, numOf2PointF * 2);

                // create primitive-made structs from floatr array
                for(int i = 0; i < coordStream.Length ; i=i+2)
                {
                    ListOfPoint2f.Add(new Vector2(coordStream[i], coordStream[i+1]));
                }
            }
            this._vectorPoint2F = ListOfPoint2f;
        }
        public static object MarshalNativeToManaged(IntPtr pointerToPoint2fList)
        {   

            List<Vector2> ListOfPoint2f = new List<Vector2>();
            int numOf2PointF = Marshaller.GetVector2PointFPointerSize(pointerToPoint2fList);

            if(numOf2PointF > 0)
            {
                IntPtr nativeInternalDataPointer = Marshaller.GetVector2PointFPointerData(pointerToPoint2fList);
                float[] coordStream = new float[numOf2PointF * 2];

                // iterate over bytes in contigous memory to copy in float values from native stream to managed array
                Marshal.Copy(nativeInternalDataPointer, coordStream, 0, numOf2PointF * 2);

                // create primitive-made structs from floatr array
                for(int i = 0; i < coordStream.Length ; i=i+2)
                {
                    ListOfPoint2f.Add(new Vector2(coordStream[i], coordStream[i+1]));
                }
            }
            return ListOfPoint2f;
        }
        ~VectorPoint2FMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
