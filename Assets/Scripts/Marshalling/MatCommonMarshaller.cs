using System;
using System.Runtime.InteropServices;


namespace OpenCVInterop.Marshallers
{
    public static partial class Marshaller
    {
        #if UNITY_EDITOR_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateMatPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteMatPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerRowNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerColNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerDimNum(IntPtr pointer);

        #elif UNITY_STANDALONE_WIN

            [DllImport("OpenCVUnity")]
            public unsafe static extern IntPtr CreateMatPointer();
            [DllImport("OpenCVUnity")]
            public static extern void DeleteMatPointer(IntPtr pointer);
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
            public static extern void DeleteMatPointer(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerRowNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerColNum(IntPtr pointer);
            [DllImport("OpenCVUnity")]
            public static extern int GetMatPointerDimNum(IntPtr pointer);

        #endif
    }
}