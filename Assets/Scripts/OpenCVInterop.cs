using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public static class OpenCVIntr
{
    #if UNITY_EDITOR_WIN
        [DllImport("test_native")]
        private unsafe static extern int FindMarkers(Color32* textureData, int width, int height, int[] markerList);
    #elif UNITY_ANDROID
        [DllImport("test_native")]
        private unsafe static extern int FindMarkers(Color32* textureData, int width, int height, int[] markerList);
    #endif


}