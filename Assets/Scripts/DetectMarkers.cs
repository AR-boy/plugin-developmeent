using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


internal static class OpenCVInteropDetectMarkers
{
    [DllImport("DetectMarkers")]
    internal static extern int Init(ref int outCameraWidth, ref int outCameraHeight);

    [DllImport("DetectMarkers")]
    internal static extern int Close();

    [DllImport("DetectMarkers")]
    internal unsafe static extern void DetectMarkers();

    [DllImport("DetectMarkers")]
    internal unsafe static extern void DrawDetectMarkers();
}

public class DetectMarkers : MonoBehaviour
{
    private bool _ready;
    // Start is called before the first frame update
    void Start()
    {
        int camWidth = 0, camHeight = 0;
        Debug.Log("starting init...");
        int result = OpenCVInteropDetectMarkers.Init(ref camWidth, ref camHeight); 
        
        if(result < 0)
        {
            if(result == -1)
            {
                Debug.LogWarningFormat("[{0}] Failed to open camera stream.", GetType());
            }

            return;
        }
        Debug.Log("finished init"); 
        _ready = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!_ready)
            return;
        unsafe
        {
            OpenCVInteropDetectMarkers.DetectMarkers();
            OpenCVInteropDetectMarkers.DrawDetectMarkers();
        }
    }
    
    void OnApplicationQuit()
    {
        if(_ready)
        {
            OpenCVInteropDetectMarkers.Close();
        }
    }
}
