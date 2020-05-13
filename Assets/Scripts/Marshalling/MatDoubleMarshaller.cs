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
            public static extern void CreateMatDouble(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern double GetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column, double value);

        #elif UNITY_STANDALONE_WIN

            [DllImport("OpenCVUnity")]
            public static extern void CreateMatDouble(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern double GetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column, double value);

        #elif UNITY_ANDROID

            [DllImport("OpenCVUnity")]
            public static extern void CreateMatDouble(IntPtr pointer, int rows, int cols);
            [DllImport("OpenCVUnity")]
            public static extern double GetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column);
            [DllImport("OpenCVUnity")]
            public static extern void SetMatPointerOfDoubleDataAt(IntPtr pointer, int row, int column, double value);

        #endif
    }

    public class MatDoubleMarshaller : CustomMarshaller
    {
        private double[][] _matDouble;
        private IntPtr _nativeDataPointer;
        public MatDoubleMarshaller(IntPtr nativeDataPointer) {
            NativeDataPointer = nativeDataPointer;
            // marshal to struct with pointer info
            this.MarshalNativeToManaged();
        }
        public MatDoubleMarshaller(double[][] _matDouble) {
            this._matDouble = _matDouble;
            this.MarshalManagedToNative();
        }
        public MatDoubleMarshaller() {
            NativeDataPointer = Marshaller.CreateMatPointer();
        }
        public IntPtr NativeDataPointer 
        { 
            get => this._nativeDataPointer;
            set => this._nativeDataPointer = value; 
        }
        public object GetMangedObject()
        {
            if(this._matDouble == null) {
                this.MarshalNativeToManaged();
            }
            return this._matDouble;
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
            this._matDouble = doubleArray;
        }
        public void MarshalManagedToNative()
        {            
            IntPtr nativeDataPointer = Marshaller.CreateMatPointer();
            
            int rowLength = this._matDouble.Length;
            int colLength = this._matDouble[0].Length;

            Marshaller.CreateMatDouble(nativeDataPointer, rowLength, colLength);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    double local = this._matDouble[i][j];
                    Marshaller.SetMatPointerOfDoubleDataAt(nativeDataPointer, i, j, local);
                }
            }

            NativeDataPointer = nativeDataPointer;
        } 
        ~MatDoubleMarshaller()
        {
            this.DeleteNativePointer();
        }
    }
}
