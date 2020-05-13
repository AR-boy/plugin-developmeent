using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCameraController : MonoBehaviour
{
    Camera ARCam;
    public static int INITIAL_CAMERA_Z_POSITION = 500;
    DisplayPlaneAutosizer BackgroundPaneScript;
    Vector3 displayPanePosition;
    Vector2 displayPaneSize;
    private bool lateStart = true;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if(lateStart)
        {
            ARCam = gameObject.GetComponent<Camera>();
            BackgroundPaneScript = GameObject.FindWithTag("DisplayPlane").GetComponent<DisplayPlaneAutosizer>();
            if(BackgroundPaneScript == null)
            {
                Debug.LogError("No BackgroundPane found, bring BackgroundPane prefab into the scene");
            }
            displayPanePosition = BackgroundPaneScript.GetPanePosition(); 
            if(BackgroundPaneScript.HasSetUp())
            {
                gameObject.transform.position = new Vector3(displayPanePosition.x, displayPanePosition.y, INITIAL_CAMERA_Z_POSITION);
                lateStart = false;
            }
        }
        displayPanePosition = BackgroundPaneScript.GetPanePosition(); 
        displayPaneSize = BackgroundPaneScript.GetPaneSize();
        float distance = gameObject.transform.position.z - displayPanePosition.z;
        // calculating FOV based on frustrum - https://docs.unity3d.com/Manual/FrustumSizeAtDistance.html
        ARCam.fieldOfView = 2.0f * Mathf.Atan(displayPaneSize.y * 0.5f / distance) * Mathf.Rad2Deg;
        ARCam.transform.LookAt(displayPanePosition);
    }
    public void PositionCameraZ(float newZPostion)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, INITIAL_CAMERA_Z_POSITION + newZPostion);
    }
}
