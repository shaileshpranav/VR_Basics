using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    // Start is called before the first frame update
    public float DefaultLength = 5.0f;
    public GameObject Dot;
    private LineRenderer LineRenderer;
    private void Awake(){
        LineRenderer = GetComponent<LineRenderer>();
    }
    private void Update() {
        UpdateLine();
    }
private void UpdateLine(){

    float targetLength = DefaultLength;

    RaycastHit hit = CreateRaycast(targetLength);

    Vector3 endPosition = transform.position + (transform.forward * targetLength);

    if (hit.collider != null)
        endPosition = hit.point;

    Dot.transform.position = endPosition;

    LineRenderer. SetPosition(0, transform.position);
    LineRenderer. SetPosition(1, endPosition);

    }

    private RaycastHit CreateRaycast (float length){
        RaycastHit hit;
        Ray ray = new Ray (transform.position, transform.forward);
        Physics.Raycast(ray, out hit, DefaultLength);
        return hit;
    }

}