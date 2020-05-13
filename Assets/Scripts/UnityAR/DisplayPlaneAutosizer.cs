using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayPlaneAutosizer : MonoBehaviour
{
    private Camera displayCamera;
    private WebCamTexture deviceCamera;
    private bool lateStart = true;

    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        if(lateStart)
        {
            displayCamera = GameObject.FindWithTag("ARCamera").GetComponent<Camera>();
            if(!displayCamera)
            {
                Debug.LogError("No ARCamera found, bring the ARCamera prefab into the scene");
            }

            lateStart = false;
                    // find device camera
            deviceCamera = gameObject.GetComponent<InitWebCamera>().GetCamera();
            gameObject.transform.localScale = new Vector3(deviceCamera.width, deviceCamera.height, gameObject.transform.lossyScale.z);

            // position left corner of the pane to be located at x0y0z0
            gameObject.transform.position = new Vector3(-(gameObject.transform.lossyScale.x) / 2, -(gameObject.transform.lossyScale.y) / 2 , 0);
        }
    }
    public bool HasSetUp() {
        return !lateStart;
    }
    public Vector3 GetPanePosition()
    {
        return gameObject.transform.position;
    }
    public Vector2 GetPaneSize()
    {
        return new Vector2(gameObject.transform.lossyScale.x, gameObject.transform.lossyScale.y);
    }

}
