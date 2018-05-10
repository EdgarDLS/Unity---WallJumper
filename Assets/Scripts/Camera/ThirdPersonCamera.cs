using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 20;
    public float rotationSmoothTime = 0.12f;
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

            transform.position = target.position - (this.transform.forward * distanceFromTarget);
        }


        if (Input.GetAxis("Mouse ScrollWheel") > .0f)       //UP
            distanceFromTarget -= 0.25f;
        else if (Input.GetAxis("Mouse ScrollWheel") < .0f)  //DOWN
            distanceFromTarget += 0.25f;


        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }  
    }
}
