using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct BoundingBox {
    public float min_x, max_x, min_y, max_y, min_z, max_z;
}
public class ARRootController : MonoBehaviour
{

    private Renderer[] childrenMeshes;
    private ARCameraController ARCamera;
    private Collider ARRootCollider;
    private BoundingBox childrenBounds;

    void Start()
    {
        ARCamera = GameObject.FindWithTag("ARCamera").GetComponent<ARCameraController>();
        if(ARCamera == null)
        {
            Debug.LogError("No ARCamera found, bring ARCamera prefab into the scene");
        }
        ARRootCollider = gameObject.GetComponent<Collider>();
        childrenMeshes = gameObject.GetComponentsInChildren<Renderer>();
        childrenBounds = new BoundingBox();

        ARRootCollider.bounds.SetMinMax(
            new Vector3(
                childrenBounds.min_x,
                childrenBounds.min_y,
                childrenBounds.min_z
            ), 
            new Vector3(
                childrenBounds.max_x, 
                childrenBounds.max_y, 
                childrenBounds.max_z
            )
        );

    }
    public BoundingBox CalculateBoundinBox()
    {

        BoundingBox bbox = new BoundingBox();
        // set up starting values
        bbox.max_x = childrenMeshes[0].bounds.max.x;
        bbox.min_x = childrenMeshes[0].bounds.min.x;

        bbox.max_y = childrenMeshes[0].bounds.max.y;
        bbox.min_y = childrenMeshes[0].bounds.min.y;

        bbox.max_z = childrenMeshes[0].bounds.max.z;
        bbox.min_z = childrenMeshes[0].bounds.min.z;
        // get the bounds of all the renderers inside ARRoot, and record min/max values
        foreach(Renderer renderer in childrenMeshes)
        {
            if(renderer.bounds.min.x < bbox.min_x)
            {
                bbox.min_x = renderer.bounds.min.x;
            }
            if(renderer.bounds.max.x > bbox.max_x)
            {
                bbox.max_x = renderer.bounds.max.x;
            }
            if(renderer.bounds.min.y < bbox.min_y)
            {
                bbox.min_y = renderer.bounds.min.y;
            }
            if(renderer.bounds.max.y > bbox.max_y)
            {
                bbox.max_y = renderer.bounds.max.y;
            }
            if(renderer.bounds.min.z < bbox.min_z)
            {
                bbox.min_z = renderer.bounds.min.z;
            }
            if(renderer.bounds.max.z > bbox.max_z)
            {
                bbox.max_z = renderer.bounds.max.z;
            }
        }
        return bbox;
    }
    void Update()
    {
        childrenBounds = new BoundingBox();

        ARRootCollider.bounds.SetMinMax(
            new Vector3(
                childrenBounds.min_x,
                childrenBounds.min_y,
                childrenBounds.min_z
            ), 
            new Vector3(
                childrenBounds.max_x, 
                childrenBounds.max_y, 
                childrenBounds.max_z
            )
        );
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
