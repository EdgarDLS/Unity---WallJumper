using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distance;

	void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	void Update ()
    {
        
		if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
	}
}
