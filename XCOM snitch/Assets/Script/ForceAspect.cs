using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//[RequireComponent(typeof(Camera))]
public class ForceAspect : MonoBehaviour
{

    public float aspect = 1;
    UnityEngine.Camera cam;
	void OnEnable ()
	{
	    cam = GetComponent<UnityEngine.Camera>();
        cam.aspect = aspect;
	}
}
