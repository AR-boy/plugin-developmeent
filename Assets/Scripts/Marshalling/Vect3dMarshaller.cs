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
            // Vec3d:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVec3dPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVec3dPointer(IntPtr pointer);
        
        #elif UNITY_STANDALONE_WIN

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

    public class Vec3d
    {
        public double x,y,z;
        public Vec3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    class Vec3dMarshaller : CustomMarshaller
    {
        public static int VECTOR_3D_SIZE = 3;
        private Vec3d _vec3d;
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
            if(this._vec3d == null)
            {
                this.MarshalNativeToManaged();
            }
            return this._vec3d;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteVec3dPointer(NativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   

            // create an array to contain values copied over from native data structure
            double[] valueStream = new double[VECTOR_3D_SIZE];

            // iterate over bytes in contigous memory to copy in double values from native stream to managed array
            Marshal.Copy(NativeDataPointer, valueStream, 0, valueStream.Length);
            
            // 
            this._vec3d = new Vec3d(valueStream[0], valueStream[1], valueStream[2]);
        }

        public static Vec3d MarshalNativeToManaged(IntPtr nativeVec3dPoitner)
        {   

            // create an array to contain values copied over from native data structure
            double[] valueStream = new double[VECTOR_3D_SIZE];

            // iterate over bytes in contigous memory to copy in double values from native stream to managed array
            Marshal.Copy(nativeVec3dPoitner, valueStream, 0, valueStream.Length);
            
            // return new Vec3d
            return new Vec3d(valueStream[0], valueStream[1], valueStream[2]);
        }
        ~Vec3dMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}