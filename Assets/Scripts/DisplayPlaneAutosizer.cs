using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayPlaneAutosizer : MonoBehaviour
{
    private Camera displayCamera;
    private WebCamTexture deviceCamera;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.tag != "DisplayPlane")
        {
            Debug.LogError("Please attach the script to correct component");
        }

        displayCamera = GameObject.FindWithTag("DisplayCamera").GetComponent<Camera>();

        // will later update camera position
        if(!displayCamera)
        {
            Debug.LogError("No camera with 'DisplayCamera' tag attached");
        }

        // find device camera
        deviceCamera = gameObject.GetComponent<InitWebCamera>().GetCamera();

        // position middle of the plane to be located at x0y0z0
        gameObject.transform.position = new Vector3(-(gameObject.transform.lossyScale.x) / 2, -(gameObject.transform.lossyScale.y) / 2 , 0);
        // position the camera to point to the middle of the plane
        displayCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 500);

        // float cameraPos = (displayCamera.nearClipPlane + 0.01f);

        // gameObject.transform.position = displayCamera.transform.position  + displayCamera.transform.forward * cameraPos;
        // gameObject.transform.LookAt(displayCamera.transform);
        // transform.Rotate(90.0f, 0.0f, 0.0f);

        // float h = (Mathf.Tan(displayCamera.fieldOfView*Mathf.Deg2Rad*0.5f)*cameraPos*2f) /10.0f;

        // gameObject.transform.localScale = new Vector3(h*displayCamera.aspect,1.0f, h);
    }

    public float GetXScaleFactor()
    {
        return (gameObject.transform.lossyScale.x * 10) / deviceCamera.width;
    }

    public float GetYScaleFactor()
    {
        return (gameObject.transform.lossyScale.z * 10)  / deviceCamera.height;
    }

}
