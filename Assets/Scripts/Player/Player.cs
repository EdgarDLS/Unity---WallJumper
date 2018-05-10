using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 15;

    private Camera mainCamera;
    private bool jumpAction;

    private Vector3 jumpDirection;

	void Start ()
    {
        mainCamera = Camera.main;
	}
	
	void Update ()
    {
        if (jumpAction)
        {
            this.transform.position += jumpDirection * movementSpeed * Time.deltaTime;
        }

		if (Input.GetMouseButtonDown(0) && !jumpAction)
        {
            jumpDirection = (this.transform.position - mainCamera.transform.position).normalized;

            jumpAction = true;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        jumpAction = false;
    }
}
