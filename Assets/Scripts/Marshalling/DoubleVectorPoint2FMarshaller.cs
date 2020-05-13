// native c#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVInterop.Marshallers;
// unity
using UnityEngine;

namespace OpenCVInterop.Marshallers
{
    public static partial class Marshaller
    {
        #if UNITY_EDITOR_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector2PointFPointerDataAt(IntPtr detectMarkerPointer, int offsetByNumOfElement);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVector2PointFPointer(IntPtr nativeDataPointer);

        #elif UNITY_STANDALONE_WIN
        
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector2PointFPointerDataAt(IntPtr detectMarkerPointer, int offsetByNumOfElement);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVector2PointFPointer(IntPtr nativeDataPointer);


        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector2PointFPointerDataAt(IntPtr detectMarkerPointer, int offsetByNumOfElement);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVector2PointFPointer(IntPtr nativeDataPointer);

        #endif
    }

    public class DoubleVectorPoint2FMarshaller : CustomMarshaller
    {
        private List<List<Vector2>> _doubleVectorPoint2F;
        private IntPtr _nativeDataPointer;
        public DoubleVectorPoint2FMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public DoubleVectorPoint2FMarshaller(List<List<Vector2>>  _doubleVectorPoint2F) {
            this._doubleVectorPoint2F = _doubleVectorPoint2F;
        }
        public DoubleVectorPoint2FMarshaller() {
            NativeDataPointer = Marshaller.CreateDoubleVector2PointFPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._doubleVectorPoint2F == null) {
                this.MarshalNativeToManaged();
            }
            return this._doubleVectorPoint2F;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteDoubleVector2PointFPointer(NativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   
            List<List<Vector2>> ListOfListOf2PointF = new List<List<Vector2>>();
            int numOf2PointFLists = Marshaller.GetDoubleVector2PointFPointerSize(NativeDataPointer);

            if(numOf2PointFLists > 0)
            {
                // get pointer that points to the first internal list
                IntPtr nativeInternalDataPointer = Marshaller.GetDoubleVector2PointFPointerData(NativeDataPointer);

                for(int i = 0; i < numOf2PointFLists; i++)
                {
                    if(i != 0)
                    {
                        nativeInternalDataPointer = Marshaller.GetDoubleVector2PointFPointerDataAt(NativeDataPointer, i);
                    }
                    ListOfListOf2PointF.Add((List<Vector2>) VectorPoint2FMarshaller.MarshalNativeToManaged(nativeInternalDataPointer));
                }
            }
            this._doubleVectorPoint2F = ListOfListOf2PointF;

        }
        ~DoubleVectorPoint2FMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
