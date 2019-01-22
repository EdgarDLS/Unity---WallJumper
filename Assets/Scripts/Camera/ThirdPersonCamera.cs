using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 20;
    public float rotationSmoothTime = 0.12f;
    public bool firstPersonView;
    [SerializeField] private Vector2 pitchMinMax = new Vector2(-40, 85);    // Not X and Y | Min and Max values

    [Space]
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromTarget = 8;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    [SerializeField] private float yaw;
    [SerializeField] private float pitch;


    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	void Update ()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            //Vector3 targetRotation = new Vector3(pitch, yaw);
            transform.eulerAngles = currentRotation;

            if (!firstPersonView) transform.position = target.position - (this.transform.forward * distanceFromTarget);
            else transform.position = target.position - (this.transform.forward * 0.25f);
        }

        // ScrollWheel | Camera distance from the target
        if (Input.GetAxis("Mouse ScrollWheel") > .0f && distanceFromTarget > 2f)       //UP
            distanceFromTarget -= 0.25f;
        else if (Input.GetAxis("Mouse ScrollWheel") < .0f && distanceFromTarget < 15f)  //DOWN
            distanceFromTarget += 0.25f;
        else if (Input.GetMouseButtonDown(1))
            firstPersonView = !firstPersonView;
            


        //if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.Escape))
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } 
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }   
        }  
    }
}
