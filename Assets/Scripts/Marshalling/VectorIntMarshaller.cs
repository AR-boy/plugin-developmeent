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
            public unsafe static extern IntPtr CreateVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorIntSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorIntData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorIntPointer(IntPtr nativeDataPointer);

        #elif UNITY_STANDALONE_WIN
        
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorIntSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorIntData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorIntPointer(IntPtr nativeDataPointer);

        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorIntSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorIntData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorIntPointer(IntPtr nativeDataPointer);

        #endif
    }

    public class VectorIntMarshaller : CustomMarshaller
    {
        private int[] _vectorInt;
        private IntPtr _nativeDataPointer;
        public VectorIntMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public VectorIntMarshaller(int[]  _vectorInt) {
            this._vectorInt = _vectorInt;
        }
        public VectorIntMarshaller() {
            NativeDataPointer = Marshaller.CreateVector2PointFPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._vectorInt == null) {
                this.MarshalNativeToManaged();
            }
            return this._vectorInt;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteVector2PointFPointer(NativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   
            int numOInts = Marshaller.GetVectorIntSize(NativeDataPointer);
            int[] intList = new int[numOInts];
            if(numOInts > 0)
            {
                IntPtr nativeInternalDataPointer = Marshaller.GetVectorIntData(NativeDataPointer);
                // iterate over bytes in contigous memory to copy in ints into the array from native stream
                Marshal.Copy(nativeInternalDataPointer, intList, 0, numOInts);
            }

            numOInts = Marshaller.GetVectorIntSize(NativeDataPointer);

            this._vectorInt = intList;
        }
        public static object MarshalNativeToManaged(IntPtr pointerToIntList)
        {   
            int numOInts = Marshaller.GetVectorIntSize(pointerToIntList);
            int[] intList = new int[numOInts];
            if(numOInts > 0)
            {
                IntPtr nativeInternalDataPointer = Marshaller.GetVectorIntData(pointerToIntList);
                // iterate over bytes in contigous memory to copy in ints into the array from native stream
                Marshal.Copy(nativeInternalDataPointer, intList, 0, numOInts);
            }

            numOInts = Marshaller.GetVectorIntSize(pointerToIntList);

            return intList;

        }
        ~VectorIntMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
