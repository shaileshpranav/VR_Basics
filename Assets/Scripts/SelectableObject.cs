using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject cube;

    public void Highlight()
    {  
       // Get the Renderer component from the cube
       var cubeRenderer = cube.GetComponent<Renderer>();
       
       if(cubeRenderer.material.color == Color.red)
       {
        cubeRenderer.material.SetColor("_Color", Color.white);
       }
       else
       {
        cubeRenderer.material.SetColor("_Color", Color.red);
       }
    }
}
