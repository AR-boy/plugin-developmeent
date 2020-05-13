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
            public static extern void CreateMatInt(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerOfIntDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfIntDataAt(IntPtr pointer, int row, int column, int value);

        #elif UNITY_STANDALONE_WIN

            [DllImport("OpenCVUnity")]
            public static extern void CreateMatInt(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerOfIntDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfIntDataAt(IntPtr pointer, int row, int column, int value);

        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public static extern void CreateMatInt(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerOfIntDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfIntDataAt(IntPtr pointer, int row, int column, int value);

        #endif
    }
    public class MatIntMarshaller : CustomMarshaller
    {
        private int[][] _matInt;
        private IntPtr _nativeDataPointer;
        public MatIntMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public MatIntMarshaller(int[][] _matInt) {
            this._matInt = _matInt;
            this.MarshalManagedToNative();
        }
        public MatIntMarshaller() {
            NativeDataPointer = Marshaller.CreateMatPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._matInt == null) {
                this.MarshalNativeToManaged();
            }
            return this._matInt;
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
            this._matInt = intArray;
        }
        public void MarshalManagedToNative()
        {            
            IntPtr nativeDataPointer = Marshaller.CreateMatPointer();
            
            int rowLength = this._matInt.Length;
            int colLength = this._matInt[0].Length;

            Marshaller.CreateMatInt(nativeDataPointer, rowLength, colLength);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Marshaller.SetMatPointerOfIntDataAt(nativeDataPointer, i, j, this._matInt[i][j]);
                }
            }

            NativeDataPointer = nativeDataPointer;
        } 
        ~MatIntMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
