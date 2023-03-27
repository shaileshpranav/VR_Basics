using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    // public Transform controllerTransform;
    private Transform prev_Parent;
    public bool isGrabbed;
    public GameObject cube_g;
    private float grab_dist;
    private Vector3 offset;
    private float grabRadius = 0.2f;
    private Transform originalParent;
    private Transform controllerTransform;

    void Start()
    {
        originalParent = transform.parent;
    }

    void Update()
    {
        if (isGrabbed && controllerTransform != null)
        {
            transform.parent = controllerTransform;
        }
        else if (!isGrabbed && transform.parent != originalParent)
        {
            transform.parent = originalParent;
        }
        
    }

    public void Grab(float triggerPress, Transform controllerTransform)
    {
        Collider[] hitColliders = Physics.OverlapSphere(controllerTransform.position, grabRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject && triggerPress > 0f)
            {
                isGrabbed = true;
                this.controllerTransform = controllerTransform;
                break;
            }
        }

        if (!isGrabbed)
        {
            isGrabbed = false;
            this.controllerTransform = null;
            transform.parent = originalParent;
        }
    }

    public void Release()
    {
        isGrabbed = false;
        controllerTransform = null;
        transform.parent = originalParent;
    }


}
