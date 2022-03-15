using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Solidify : MonoBehaviour
{
    public Shader flatShader;
    UnityEngine.Camera cam;
	void OnEnable ()
	{
	    cam = GetComponent<UnityEngine.Camera>();
        cam.SetReplacementShader(flatShader, "");
	}
	
}
