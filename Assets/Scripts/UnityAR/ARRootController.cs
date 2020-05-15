using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARRootController : MonoBehaviour
{
    private ARCameraController ARCamera;
    private BoxCollider ARRootCollider;

    void Start()
    {
        ARCamera = GameObject.FindWithTag("ARCamera").GetComponent<ARCameraController>();
        if(ARCamera == null)
        {
            Debug.LogError("No ARCamera found, bring ARCamera prefab into the scene");
        }
        ARRootCollider = gameObject.GetComponent<BoxCollider>();

    }

    void OnTriggerEnter(Collider other)
    {
        // if game object intersect background 
        if(other.gameObject.tag.Equals("DisplayPlane"))
        {
            if(ARRootCollider.bounds.min.z <= 0)
            {
                float z_transform_val = Mathf.Abs(ARRootCollider.bounds.min.z)+ 50 ;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z_transform_val);
                ARCamera.PositionCameraZ(z_transform_val);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("DisplayPlane"))
        {

            if(ARRootCollider.bounds.min.z <= 0)
            {
                float z_transform_val = Mathf.Abs(ARRootCollider.bounds.min.z)+ 50 ;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z_transform_val);
                ARCamera.PositionCameraZ(z_transform_val);
            }
        }
    }

}
