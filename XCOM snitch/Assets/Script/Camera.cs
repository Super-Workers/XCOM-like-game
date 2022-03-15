using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Transform cameraTransform;

    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;
    public float maxZoom;
    public float minZoom;

    public Vector3 newZoom;
    public Vector3 newPosition;
    public Quaternion newRotation;
    public GameObject roof;
    //public GameObject roofTiles;

    public static object main { get; internal set; }

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
        roof = GameObject.FindGameObjectWithTag("Roof");
    }

    
    void Update()
    {
        HandleMovementInput();

        if (newZoom.z >= -10) //&& !roofTiles.activeInHierarchy)
        {
            roof.SetActive(false);
        }
        else
        {
            roof.SetActive(true);
        }
    }

    void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if ((Input.mouseScrollDelta.y > 0) && (newZoom.z < maxZoom))
        {
            newZoom += zoomAmount;
        }
        if ((Input.mouseScrollDelta.y < 0) && (newZoom.z > minZoom))
        {
            newZoom -= zoomAmount;
        }

        transform.position = Vector3.Lerp(newPosition, transform.position, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(newRotation, transform.rotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

}
