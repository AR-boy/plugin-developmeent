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
            // Mat:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVectorIntPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVectorIntPointerDataAt(IntPtr pointer, int offset);
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVectorIntPointerSize(IntPtr pointer);

        #elif UNITY_STANDALONE_WIN
        
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVectorIntPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVectorIntPointerDataAt(IntPtr pointer, int offset);
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVectorIntPointerSize(IntPtr pointer);

        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVectorIntPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVectorIntPointerDataAt(IntPtr pointer, int offset);
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVectorIntPointerSize(IntPtr pointer);

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

            int numOfListsOfInts = Marshaller.GetDoubleVectorIntPointerSize(NativeDataPointer);
            List<int[]> listOfListOfInts = new List<int[]>();
            if(numOfListsOfInts > 0)
            {

                IntPtr nativeInternalDataPointer = NativeDataPointer;

                for(int i = 0; i < numOfListsOfInts; i++)
                {
                    listOfListOfInts.Add((int[]) VectorIntMarshaller.MarshalNativeToManaged(nativeInternalDataPointer));
                    // iterate over bytes in contigous memory to copy in ints into the array from native stream
                    nativeInternalDataPointer = Marshaller.GetDoubleVectorIntPointerDataAt(NativeDataPointer, i);
                }

            }
            this._doubleVectorInt = listOfListOfInts;
        }
        ~DoubleVectorIntMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
