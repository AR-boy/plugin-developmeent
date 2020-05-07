// native c#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
// unity
using UnityEngine;

namespace OpenCVInterop
{
    public static partial class Marshaller
    {
        #if UNITY_EDITOR_WIN
            // Mat:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVectorIntPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVectorIntPointerDataAt(IntPtr pointer, int offset);
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVectorIntPointerSize(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorIntData(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorIntSize(IntPtr pointer);

        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVectorIntPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVectorIntPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVectorIntPointerSize(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorIntData(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorIntSize(IntPtr pointer);

        #endif
    }

    public class DoubleVectorIntMarshaller : CustomMarshaller
    {
        private List<int[]> _doubleVectorInt;
        private IntPtr _nativeDataPointer;
        public DoubleVectorIntMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public DoubleVectorIntMarshaller(List<int[]>  _doubleVectorInt) {
            this._doubleVectorInt = _doubleVectorInt;
        }
        public DoubleVectorIntMarshaller() {
            NativeDataPointer = Marshaller.CreateDoubleVectorIntPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._doubleVectorInt == null) {
                this.MarshalNativeToManaged();
            }
            return this._doubleVectorInt;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteDoubleVectorIntPointer(NativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   

            // create an array to contain values copied over from native data structure
            int outerVectorSize = Marshaller.GetDoubleVectorIntPointerSize(NativeDataPointer);

            List<int[]> twoDIntList = new List<int[]>();
            int[] intList;
            for(int i = 0; i < outerVectorSize; i++)
            {

                IntPtr innerIntVectorPointer = Marshaller.GetDoubleVectorIntPointerDataAt(NativeDataPointer, i);
                int innerIntVectorSize = Marshaller.GetVectorIntSize(innerIntVectorPointer);
                intList = new int[innerIntVectorSize];
                for(int j = 0; j < innerIntVectorSize; j++)
                {
                    IntPtr innerDataPointer = Marshaller.GetVectorIntData(NativeDataPointer);
                    // iterate over bytes in contigous memory to copy in ints into the array from native stream
                    Marshal.Copy(innerDataPointer, intList, 0, innerIntVectorSize);
                }
                twoDIntList.Add(intList);
            }
            this._doubleVectorInt = twoDIntList;
        }
        ~DoubleVectorIntMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
