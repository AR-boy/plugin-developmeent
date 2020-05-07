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
            // Vec3d:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVec3dPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVec3dPointer(IntPtr pointer);
        #elif UNITY_ANDROID
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVec3dPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVec3dPointer(IntPtr pointer);
        #endif
    }

    public struct Point3d
    {
        public double x,y,z;
        public Point3d(double x_param, double y_param, double z_param)
        {
            x = x_param;
            y = y_param;
            z = z_param;
        }
    }

    class Vec3dMarshaller : CustomMarshaller
    {
        private static int VECTOR_3D_SIZE = 3;
        private Point3d _vec3d;
        private IntPtr _nativeDataPointer;
        public Vec3dMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public Vec3dMarshaller() {
            NativeDataPointer = Marshaller.CreateVec3dPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            return this._vec3d;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteVec3dPointer(this._nativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   

            // create an array to contain values copied over from native data structure
            double[] valueStream = new double[VECTOR_3D_SIZE];

            // iterate over bytes in contigous memory to copy in double values from native stream to managed array
            Marshal.Copy(this._nativeDataPointer, valueStream, 0, valueStream.Length);
            
            // 
            this._vec3d = new Point3d(valueStream[0], valueStream[1], valueStream[2]);
        }
        ~Vec3dMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}