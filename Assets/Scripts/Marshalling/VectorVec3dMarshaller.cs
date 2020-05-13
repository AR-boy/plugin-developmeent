// native c#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace OpenCVInterop.Marshallers
{
    public static partial class Marshaller
    {
        #if UNITY_EDITOR_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVectorVec3dPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorVec3dPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorVec3dPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorVec3dPointerDataAt(IntPtr detectMarkerPointer, int offset);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorVec3dPointer(IntPtr nativeDataPointer);

        #elif UNITY_STANDALONE_WIN
        
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVectorVec3dPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorVec3dPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorVec3dPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorVec3dPointerDataAt(IntPtr detectMarkerPointer, int offset);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorVec3dPointer(IntPtr nativeDataPointer);


        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVectorVec3dPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorVec3dPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorVec3dPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorVec3dPointerDataAt(IntPtr detectMarkerPointer, int offset);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorVec3dPointer(IntPtr nativeDataPointer);

        #endif
    }

    public class VectorVec3dMarshaller : CustomMarshaller
    {
        private List<Vec3d> _vectorVec3d;
        private IntPtr _nativeDataPointer;
        public VectorVec3dMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public VectorVec3dMarshaller(List<Vec3d>  _vectorVec3d) {
            this._vectorVec3d = _vectorVec3d;
        }
        public VectorVec3dMarshaller() {
            NativeDataPointer = Marshaller.CreateVectorVec3dPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._vectorVec3d == null) {
                this.MarshalNativeToManaged();
            }
            return this._vectorVec3d;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteVectorVec3dPointer(NativeDataPointer);
        }

        public void MarshalNativeToManaged()
        {

            List<Vec3d> vec3dList = new List<Vec3d>();
            int numOfVec3d = Marshaller.GetVectorVec3dPointerSize(NativeDataPointer);

            if(numOfVec3d > 0)
            {
                IntPtr nativeInternalDataPointer = Marshaller.GetVectorVec3dPointerData(NativeDataPointer);

                // create primitive-made structs from floatr array
                for(int i = 0; i < numOfVec3d ; i++)
                {
                    vec3dList.Add(Vec3dMarshaller.MarshalNativeToManaged(nativeInternalDataPointer));
                    nativeInternalDataPointer = Marshaller.GetVectorVec3dPointerDataAt(NativeDataPointer, i);
                }
            }
            this._vectorVec3d = vec3dList;
        }
         ~VectorVec3dMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}