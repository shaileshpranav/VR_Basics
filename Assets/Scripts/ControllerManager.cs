using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public GameObject selectedObject = null;
    // public LineRenderer pointerLine;
    public float maxDistance = 100f;
    // public LayerMask layerMask;
    public Transform controllerTransform;
    private Transform endPoint;
    private float grab_radius = 1f;
    private GameObject hitObject;

// Grabbable
    public GameObject grabbedObject = null;
    public float moveSpeed = 1f;
    private Vector3 grabOffset = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        // endPoint = new GameObject("EndPoint").transform;
        // endPoint.position = transform.position + (transform.forward * maxDistance);
        // pointerLine.enabled = true;
        // pointerLine.useWorldSpace = true;
    }

    void Update()
    {
        if (GetButtonPress())
        {
            CastRay();
        }
       GetTriggerPress();
       GetJoyVal();
       GetJoyPress();

    }
    
    Vector3 GetPointingDir()
    {
        Vector3 controllerForward = controllerTransform.TransformDirection(Vector3.forward);
        Vector3 normalizedDirection = controllerForward.normalized;
        return normalizedDirection;
    }
    Vector3 GetPosition()
    {
        Vector3 controllerPosition = controllerTransform.position;
        return controllerPosition;
    }
    bool GetButtonPress()
    {
       return OVRInput.GetUp(OVRInput.RawButton.X);
    }
    void CastRay()
    {
        Debug.Log("In CastRay");
        
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("IgnoreRaycast");
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))//, maxDistance, layerMask))
        {
            selectedObject = hit.collider.gameObject;
            selectedObject.GetComponent<SelectableObject>().Highlight();
            endPoint.position = hit.point;
        }
        else
        {
            selectedObject = null;
            endPoint.position = transform.position + transform.forward * maxDistance;
            // lineRenderer.SetPosition(1, transform.position + transform.forward * maxDistance);
        }

    }

// grabbable
    void GetTriggerPress()
    {
        if(OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0)
        {
            // GrabObject(0.5f);
            GrabObject(OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger));
        }

        else
        {
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<GrabbableObject>().Release();
            grabbedObject = null;
        }
        }
    }


    void GrabObject(float pressedValue) 
    {
        if (grabbedObject == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(controllerTransform.position, grab_radius);
            foreach (var hitCollider in hitColliders)
            {
                GrabbableObject grabbable = hitCollider.GetComponent<GrabbableObject>();
                if (grabbable != null)
                {
                    grabbedObject = hitCollider.gameObject;
                    grabbable.Grab(pressedValue, controllerTransform);
                    break;
                }
            }
            // RaycastHit hit;

            // if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, maxDistance))
            // {
            //     if (hit.collider.gameObject.GetComponent<GrabbableObject>() != null)
            //     {
            //     grabbedObject = hit.collider.gameObject;
            //     grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            //     grabbedObject.GetComponent<GrabbableObject>().Grab(pressedValue, controllerTransform);
            //     grabbedObject.transform.position = controllerTransform.position + controllerTransform.forward * 0.5f;
            //     grabbedObject.transform.parent = controllerTransform;
            //     }
            // }
        }
    }


    void GetJoyPress()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch)>0 && grabbedObject == null)
        {
            GrabObject_ray();
        }

    }
    void GetJoyVal()
    {
        if (grabbedObject != null)
        {
            float vertical = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Touch).y;

            Vector3 moveDirection = controllerTransform.forward * vertical * moveSpeed;

            grabbedObject.transform.position += moveDirection * Time.deltaTime;
        }
    }
    
    void GrabObject_ray()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch)>0)
        {
            RaycastHit hit;
            if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit))
            {
                if(hit.collider.gameObject.GetComponent<GrabbableObject>() != null)
                {
                grabbedObject = hit.collider.gameObject;
                // grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.transform.SetParent(controllerTransform);
                // grabOffset = grabbedObject.transform.position - controllerTransform.position;
                grabbedObject.transform.position = controllerTransform.position; // + grabOffset;
                }
            }
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) < 0 && grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
            // grabOffset = Vector3.zero;
        }

        if (grabbedObject != null)
        {
            grabbedObject.transform.position = controllerTransform.position + controllerTransform.forward * 0.5f;
        }
    }

}
