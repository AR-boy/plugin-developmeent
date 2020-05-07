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
            public unsafe static extern IntPtr CreateMatPointer();
            [DllImport("OpenCVUnity")]
            public static extern void CreateMatDouble(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern void CreateMatInt(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteMatPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern double GetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column, double value);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerOfIntDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfIntDataAt(IntPtr pointer, int row, int column, int value);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerRowNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerColNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerDimNum(IntPtr pointer);

        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateMatPointer();
            [DllImport("OpenCVUnity")]
            public static extern void CreateMatDouble(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern void CreateMatInt(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern void DeleteMatPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern double GetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column, double value);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerOfIntDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfIntDataAt(IntPtr pointer, int row, int column, int value);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerRowNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerColNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerDimNum(IntPtr pointer);

        #endif
    }

    public class MatDMarshaller : CustomMarshaller
    {
        private double[][] _matd;
        private IntPtr _nativeDataPointer;
        public MatDMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public MatDMarshaller(double[][] matd) {
            this._matd = matd;
            this.MarshalManagedToNative();
        }
        public MatDMarshaller() {
            NativeDataPointer = Marshaller.CreateMatPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._matd == null) {
                this.MarshalNativeToManaged();
            }
            return this._matd;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteMatPointer(NativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   

            // create an array to contain values copied over from native data structure
            int numOfRows = Marshaller.GetMatPointerRowNum(NativeDataPointer);
            int numOfCols = Marshaller.GetMatPointerColNum(NativeDataPointer);

            double[][] doubleArray = new double[numOfRows][];

            for(int i = 0; i < numOfRows; i++)
            {
                doubleArray[i] = new double[numOfCols];
                for(int j = 0; j < numOfCols; j++)
                {
                    doubleArray[i][j] = Marshaller.GetMatPointerOfDoubleDataAt(NativeDataPointer, i, j);
                }
            }
            this._matd = doubleArray;
        }
        public void MarshalManagedToNative()
        {            
            IntPtr nativeDataPointer = Marshaller.CreateMatPointer();
            
            int rowLength = this._matd.Length;
            int colLength = this._matd[0].Length;

            Marshaller.CreateMatDouble(nativeDataPointer, rowLength, colLength);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    double local = this._matd[i][j];
                    Marshaller.SetMatPointerOfDoubleDataAt(nativeDataPointer, i, j, local);
                }
            }

            NativeDataPointer = nativeDataPointer;
        } 
        ~MatDMarshaller()
        {
            this.DeleteNativePointer();
        }
    }

    public class MatIMarshaller : CustomMarshaller
    {
        private int[][] _mati;
        private IntPtr _nativeDataPointer;
        public MatIMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public MatIMarshaller(int[][] _mati) {
            this._mati = _mati;
            this.MarshalManagedToNative();
        }
        public MatIMarshaller() {
            NativeDataPointer = Marshaller.CreateMatPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._mati == null) {
                this.MarshalNativeToManaged();
            }
            return this._mati;
        }
        public void DeleteNativePointer() {
            Marshaller.DeleteMatPointer(NativeDataPointer);
        }
        public void MarshalNativeToManaged()
        {   

            // create an array to contain values copied over from native data structure
            int numOfRows = Marshaller.GetMatPointerRowNum(NativeDataPointer);
            int numOfCols = Marshaller.GetMatPointerColNum(NativeDataPointer);

            int[][] intArray = new int[numOfRows][];

            for(int i = 0; i < numOfRows; i++)
            {
                intArray[i] = new int[numOfCols];
                for(int j = 0; j < numOfCols; j++)
                {
                    intArray[i][j] = Marshaller.GetMatPointerOfIntDataAt(NativeDataPointer, i, j);
                }
            }
            this._mati = intArray;
        }
        public void MarshalManagedToNative()
        {            
            IntPtr nativeDataPointer = Marshaller.CreateMatPointer();
            
            int rowLength = this._mati.Length;
            int colLength = this._mati[0].Length;

            Marshaller.CreateMatInt(nativeDataPointer, rowLength, colLength);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Marshaller.SetMatPointerOfIntDataAt(nativeDataPointer, i, j, this._mati[i][j]);
                }
            }

            NativeDataPointer = nativeDataPointer;
        } 
        ~MatIMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
