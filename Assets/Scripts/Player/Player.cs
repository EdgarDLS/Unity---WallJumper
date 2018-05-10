using UnityEngine;

public class Player : MonoBehaviour
{
    [Space]
    public float movementSpeed = 15;

    private GameObject sittingTerrain;

    private Rigidbody myRigidbody;
    private Camera mainCamera;
    private bool jumpAction;

    private Vector3 jumpDirection;

	void Start ()
    {
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
	
	void Update ()
    {
		if (Input.GetMouseButtonDown(0) && !jumpAction && Cursor.lockState == CursorLockMode.Locked)
        {
            jumpDirection = (this.transform.position - mainCamera.transform.position).normalized;

            if (CheckPlatform())
            {
                myRigidbody.AddForce(jumpDirection * movementSpeed);
                jumpAction = true;
            }
        }
	}

    // Check if the direction the player is trying to jump in does not belong to the same terrain he is on, to avoid bugs
    private bool CheckPlatform()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, jumpDirection, out hit, Mathf.Infinity))
            if (hit.collider.gameObject.Equals(sittingTerrain))
                return false;

        return true;
    }

    void OnCollisionEnter(Collision collision)
    {
        sittingTerrain = collision.gameObject;      // Set the new terrain where the player is sitting on
        jumpAction = false;                         // Let the player jump again
        myRigidbody.velocity = Vector3.zero;        // Velocity = 0 so it stops on collision


    }
}
