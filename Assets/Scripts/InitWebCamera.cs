using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InitWebCamera : MonoBehaviour
{
    public Vector2 desiredResolution = new Vector2(1280, 720);

    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        private bool useDesiredResolution = true;
    #else
        private bool useDesiredResolution = false;
    #endif   
    private WebCamTexture webCamTexture;
    // Start is called before the first frame update
    void Start()
    {
        // initialise camera with device defaults
        if(useDesiredResolution)
        {
            webCamTexture = new WebCamTexture((int)desiredResolution.x, (int)desiredResolution.y);
        }
        else
        {
            webCamTexture = new WebCamTexture();
        }
        // select camera - necessary for android
        webCamTexture.deviceName = WebCamTexture.devices[0].name;
        // map video input to mesh renderer of the plane  
        this.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
        // start the camera
        webCamTexture.Play();
    }

    public WebCamTexture GetCamera()
    {
        return webCamTexture;
    }
}
