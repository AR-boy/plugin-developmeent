// native c#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
// unity
using UnityEngine;

namespace OpenCVInterop.MarshalOpenCV
{
    public static partial class OpenCVMarshal
    {
        #if UNITY_EDITOR_WIN


            // marshalling helper methods for memory management, data retrival and sizing
            // bool :

            // Mat:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateMatPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteMatPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern float GetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerOfIntDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerRowNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerColNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerDimNum(IntPtr pointer);

            // VectorInt:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorIntSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorIntData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorIntPointer(IntPtr nativeDataPointer);

            // VectorVec3d:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVectorVec3dPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorVec3dPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorVec3dPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorVec3dPointer(IntPtr nativeDataPointer);

            // Vector2PointF:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVector2PointFPointer(IntPtr nativeDataPointer);

            // DoubleVector2PointF:
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

            // Vector3PointF:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateVector3PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVector3PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVector3PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVector3PointFPointer(IntPtr nativeDataPointer);
            
            // DoubleVector3PointF:
            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateDoubleVector3PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVector3PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector3PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector3PointFPointerDataAt(IntPtr detectMarkerPointer, int offsetByNumOfElement);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVector3PointFPointer(IntPtr nativeDataPointer);

        #elif UNITY_STANDALONE_WIN
            // marshalling helper methods for memory management, data retrival and sizing
            // VectorInt:
            [DllImport("OpenCVUnity")]
            public static extern IntPtr CreateVectorIntPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVectorIntSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVectorIntData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVectorIntPointer(IntPtr nativeDataPointer);
            // Vector2PointF:
            [DllImport("OpenCVUnity")]
            public static extern IntPtr CreateVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVector2PointFPointer(IntPtr nativeDataPointer);
            // DoubleVector2PointF:
            [DllImport("OpenCVUnity")]
            public static extern IntPtr CreateDoubleVector2PointFPointer();
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern IntPtr GetDoubleVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public  static extern IntPtr GetDoubleVector2PointFPointerDataAt(IntPtr detectMarkerPointer, int offsetByNumOfElement);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVector2PointFPointer(IntPtr nativeDataPointer);
        #elif UNITY_ANDROID
            // marshalling helper methods for memory management, data retrival and sizing
            // Vector2PointF:
            [DllImport("OpenCVUnity")]
            public static extern int GetVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public  static extern IntPtr GetVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteVector2PointFPointer(IntPtr nativeDataPointer);
            // DoubleVector2PointF:
            [DllImport("OpenCVUnity")]
            public static extern int GetDoubleVector2PointFPointerSize(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public  static extern IntPtr GetDoubleVector2PointFPointerData(IntPtr detectMarkerPointer);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteDoubleVector2PointFPointer(IntPtr nativeDataPointer);
        #endif
    }
    public struct Point2f
    {
        public float x,y;
        public Point2f(float x_param, float y_param)
        {
            x = x_param;
            y = y_param;
        }
    }
    public struct Point3f
    {
        public float x,y,z;
        public Point3f(float x_param, float y_param, float z_param)
        {
            x = x_param;
            y = y_param;
            z = z_param;
        }
    }

class MatIntMarshaller : ICustomMarshaler
    {
        static private ICustomMarshaler marshaler = new MatIntMarshaller();

        public static ICustomMarshaler GetInstance(string cookie) 
        {
            return marshaler;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            // Nothing to do
        }
        public void CleanUpNativeData(IntPtr nativeDataPointer)
        {
            // delete pointer to free memory on native sides
            OpenCVMarshal.DeleteMatPointer(nativeDataPointer);
            nativeDataPointer = IntPtr.Zero;
;
        }
        public int GetManagedDataSize(int[] managedObject)
        {
            return Marshal.SizeOf(typeof(int)) * managedObject.Length;
        }
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }
        public IntPtr MarshalManagedToNative(object managedObject)
        {
            return IntPtr.Zero;
            // no current implementation  
        }
        public object MarshalNativeToManaged(IntPtr nativeDataPointer)
        {
            int numOfRows = OpenCVMarshal.GetMatPointerRowNum(nativeDataPointer);
            int numOfCols = OpenCVMarshal.GetMatPointerColNum(nativeDataPointer);
            int numOfDim = OpenCVMarshal.GetMatPointerDimNum(nativeDataPointer);



            int[,] intArray = new int[numOfRows, numOfCols];

            for(int i = 0; i < numOfRows; i++)
            {
                    for(int j = 0; j < numOfCols; j++)
                {
                    intArray[i,j] = OpenCVMarshal.GetMatPointerOfIntDataAt(nativeDataPointer, i, j);
                }
            }
            
            return intArray;
        }
    }

class MatDoubleMarshaller : ICustomMarshaler
    {
        static private ICustomMarshaler marshaler = new MatDoubleMarshaller();

        public static ICustomMarshaler GetInstance(string cookie) 
        {
            return marshaler;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            // Nothing to do
        }
        public void CleanUpNativeData(IntPtr nativeDataPointer)
        {
            // delete pointer to free memory on native sides
            OpenCVMarshal.DeleteMatPointer(nativeDataPointer);
            nativeDataPointer = IntPtr.Zero;
        }
        public int GetManagedDataSize(int[] managedObject)
        {
            return Marshal.SizeOf(typeof(int)) * managedObject.Length;
        }
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }
        public IntPtr MarshalManagedToNative(object managedObject)
        {
            return IntPtr.Zero;
            // no current implementation  
        }
        public object MarshalNativeToManaged(IntPtr nativeDataPointer)
        {
            int numOfRows = OpenCVMarshal.GetMatPointerRowNum(nativeDataPointer);
            int numOfCols = OpenCVMarshal.GetMatPointerColNum(nativeDataPointer);
            int numOfDim = OpenCVMarshal.GetMatPointerDimNum(nativeDataPointer);

            double[,] doubleArray = new double[numOfRows, numOfCols];

            for(int i = 0; i < numOfRows; i++)
            {
                for(int j = 0; j < numOfCols; j++)
                {
                    doubleArray[i,j] = OpenCVMarshal.GetMatPointerOfDoubleDataAt(nativeDataPointer, i, j);
                }
            }

            return doubleArray;
        }
    }
 class VectorIntMarshaller : ICustomMarshaler
    {
        static private ICustomMarshaler marshaler = new VectorIntMarshaller();

        public static ICustomMarshaler GetInstance(string cookie) 
        {
            return marshaler;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            // Nothing to do
        }
        public void CleanUpNativeData(IntPtr nativeDataPointer)
        {
            // delete pointer to free memory on native sides
            OpenCVMarshal.DeleteVectorIntPointer(nativeDataPointer);
            nativeDataPointer = IntPtr.Zero;
        }
        public int GetManagedDataSize(int[] managedObject)
        {
            return Marshal.SizeOf(typeof(int)) * managedObject.Length;
        }
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }
        public IntPtr MarshalManagedToNative(object managedObject)
        {
            return IntPtr.Zero;
            // no current implementation  
        }
        public object MarshalNativeToManaged(IntPtr nativeDataPointer)
        {
            int numOInts = OpenCVMarshal.GetVectorIntSize(nativeDataPointer);
            int[] intList = new int[numOInts];
            if(numOInts > 0)
            {
                IntPtr nativeInternalDataPointer = OpenCVMarshal.GetVectorIntData(nativeDataPointer);
                // iterate over bytes in contigous memory to copy in ints into the array from native stream
                Marshal.Copy(nativeInternalDataPointer, intList, 0, numOInts);
            }
            return intList;
        }
    }

    class VectorPoint2FMarshaller : ICustomMarshaler
    {
        static private ICustomMarshaler marshaler = new VectorPoint2FMarshaller();

        public static ICustomMarshaler GetInstance(string cookie) 
        {
            return marshaler;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            // Nothing to do
        }
        public void CleanUpNativeData(IntPtr nativeDataPointer)
        {
            // delete pointer to free memory on native sides
            OpenCVMarshal.DeleteVector2PointFPointer(nativeDataPointer);
            nativeDataPointer = IntPtr.Zero;
        }
        public int GetManagedDataSize(List<Point2f> managedObject)
        {
            return Marshal.SizeOf(typeof(Point2f)) * managedObject.Count;
        }
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }
        public IntPtr MarshalManagedToNative(object managedObject)
        {
            return IntPtr.Zero;
            // no current implementation  
        }
        public object MarshalNativeToManaged(IntPtr nativeDataPointer)
        {
            List<Point2f> Point2fList = new List<Point2f>();
            int numOf2FPoints = OpenCVMarshal.GetVector2PointFPointerSize(nativeDataPointer);
            if(numOf2FPoints > 0)
            {
                IntPtr nativeInternalDataPointer = OpenCVMarshal.GetVector2PointFPointerData(nativeDataPointer);
                float[] coordStream = new float[numOf2FPoints * 2];

                // iterate over bytes in contigous memory to copy in float values from native stream to managed array
                Marshal.Copy(nativeInternalDataPointer, coordStream, 0, numOf2FPoints * 2);

                // create primitive-made structs from floatr array
                for(int i = 0; i < coordStream.Length ; i=i+2)
                {
                    Point2fList.Add(new Point2f(coordStream[i], coordStream[i+1]));
                }
            }
            return Point2fList;
        }
    }

    class DoubleVector2PointFMarshaller : ICustomMarshaler 
    {
        private static ICustomMarshaler marshaler = new DoubleVector2PointFMarshaller();

        public static ICustomMarshaler GetInstance(string cookie) 
        {
            return marshaler;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            // Nothing to do
        }
        public void CleanUpNativeData(IntPtr nativeDataPointer)
        {
            // delete pointer to free memory on native sides
            OpenCVMarshal.DeleteDoubleVector2PointFPointer(nativeDataPointer);
            nativeDataPointer = IntPtr.Zero;
        }
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }
        public IntPtr MarshalManagedToNative(object managedObject)
        {
            return IntPtr.Zero;
            // no current implementation  
        }
        public object MarshalNativeToManaged(IntPtr nativeDataPointer)
        {
            List<List<Point2f>> Point2fList = new List<List<Point2f>>();
            int numOf2FPointLists = OpenCVMarshal.GetDoubleVector2PointFPointerSize(nativeDataPointer);
            if(numOf2FPointLists > 0)
            {
                // get pointer that points to the first internal list
                IntPtr nativeInternalDataPointer = OpenCVMarshal.GetDoubleVector2PointFPointerData(nativeDataPointer);

                for(int i = 0; i < numOf2FPointLists ; i++)
                {
                    VectorPoint2FMarshaller vectorPoint2FMarshaller =  new VectorPoint2FMarshaller();
                    Point2fList.Add((List<Point2f>) vectorPoint2FMarshaller.MarshalNativeToManaged(nativeInternalDataPointer));
                    // pointers are 8 bytes in 64 bit machines, need to make number 8 env variable 
                    nativeInternalDataPointer = OpenCVMarshal.GetDoubleVector2PointFPointerDataAt(nativeDataPointer, i);
                }
            }
            return Point2fList;
        }
    }

    class VectorPoint3FMarshaller : ICustomMarshaler
    {
        static private ICustomMarshaler marshaler = new VectorPoint3FMarshaller();

        public static ICustomMarshaler GetInstance(string cookie) 
        {
            return marshaler;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            // Nothing to do
        }
        public void CleanUpNativeData(IntPtr nativeDataPointer)
        {
            // delete pointer to free memory on native sides
            OpenCVMarshal.DeleteVector3PointFPointer(nativeDataPointer);
            nativeDataPointer = IntPtr.Zero;
        }
        public int GetManagedDataSize(List<Point3f> managedObject)
        {
            return Marshal.SizeOf(typeof(Point3f)) * managedObject.Count;
        }
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }
        public IntPtr MarshalManagedToNative(object managedObject)
        {
            return IntPtr.Zero;
            // no current implementation  
        }
        public object MarshalNativeToManaged(IntPtr nativeDataPointer)
        {
            List<Point3f> Point2fList = new List<Point3f>();
            int numOf2FPoints = OpenCVMarshal.GetVector2PointFPointerSize(nativeDataPointer);
            if(numOf2FPoints > 0)
            {
                IntPtr nativeInternalDataPointer = OpenCVMarshal.GetVector2PointFPointerData(nativeDataPointer);
                float[] coordStream = new float[numOf2FPoints * 2];

                // iterate over bytes in contigous memory to copy in float values from native stream to managed array
                Marshal.Copy(nativeInternalDataPointer, coordStream, 0, numOf2FPoints * 2);

                // create primitive-made structs from floatr array
                for(int i = 0; i < coordStream.Length ; i=i+3)
                {
                    Point2fList.Add(new Point3f(coordStream[i], coordStream[i+1], coordStream[i+3]));
                }
            }
            return Point2fList;
        }
    }
    class DoubleVector3PointFMarshaller : ICustomMarshaler 
    {
        private static ICustomMarshaler marshaler = new DoubleVector3PointFMarshaller();

        public static ICustomMarshaler GetInstance(string cookie) 
        {
            return marshaler;
        }
        public void CleanUpManagedData(object ManagedObj)
        {
            // Nothing to do
        }
        public void CleanUpNativeData(IntPtr nativeDataPointer)
        {
            // delete pointer to free memory on native sides
            OpenCVMarshal.DeleteDoubleVector3PointFPointer(nativeDataPointer);
            nativeDataPointer = IntPtr.Zero;
        }
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }
        public IntPtr MarshalManagedToNative(object managedObject)
        {
            return IntPtr.Zero;
            // no current implementation  
        }
        public object MarshalNativeToManaged(IntPtr nativeDataPointer)
        {
            List<List<Point3f>> Point2fList = new List<List<Point3f>>();
            int numOf2FPointLists = OpenCVMarshal.GetDoubleVector2PointFPointerSize(nativeDataPointer);
            if(numOf2FPointLists > 0)
            {
                // get pointer that points to the first internal list
                IntPtr nativeInternalDataPointer = OpenCVMarshal.GetDoubleVector2PointFPointerData(nativeDataPointer);

                for(int i = 0; i < numOf2FPointLists ; i++)
                {
                    VectorPoint3FMarshaller vectorPoint3FMarshaller =  new VectorPoint3FMarshaller();
                    Point2fList.Add((List<Point3f>) vectorPoint3FMarshaller.MarshalNativeToManaged(nativeInternalDataPointer));
                    // pointers are 8 bytes in 64 bit machines, need to make number 8 env variable 
                    nativeInternalDataPointer = OpenCVMarshal.GetDoubleVector2PointFPointerDataAt(nativeDataPointer, i);
                }
            }
            return Point2fList;
        }
    }

}

