using UnityEngine;

public class Player : MonoBehaviour
{
    [Space]
    public float movementSpeed = 15;

    [Space]
    [SerializeField] private Transform checkpoint;

    private GameObject sittingTerrain;
    private Rigidbody myRigidbody;
    private Camera mainCamera;
    private bool jumpAction;
    private Vector3 jumpDirection;

    // Dead variables
    private bool isDead = false;
    private float deadLerp = 0.7f;
    private float currentDeadLerp;
    private Vector3 actualVelocity;

	void Start ()
    {
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
	
	void Update ()
    {
        if (!isDead)
        {
            if (Input.GetMouseButtonDown(0) && !jumpAction && Cursor.lockState == CursorLockMode.Locked)
            {
                jumpDirection = (this.transform.position - mainCamera.transform.position).normalized;

                if (CheckPlatform())
                {
                    this.transform.parent = null;
                    myRigidbody.AddForce(jumpDirection * movementSpeed);
                    jumpAction = true;
                }
            }
        }

        // Little effect once the player dies
        else
        {
            currentDeadLerp += Time.deltaTime;

            if (currentDeadLerp > deadLerp)
                currentDeadLerp = deadLerp;

            float perc = currentDeadLerp / deadLerp;

            myRigidbody.velocity = Vector3.Lerp(actualVelocity, Vector3.zero, perc);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Restart();
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

    private void Restart()
    {
        isDead = false;
        jumpAction = false;

        this.transform.position = checkpoint.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Lava"))
        {
            sittingTerrain = collision.gameObject;          // Set the new terrain where the player is sitting on
            jumpAction = false;                             // Let the player jump again
            myRigidbody.velocity = Vector3.zero;            // Velocity = 0 so it stops on collision

            this.transform.parent = collision.transform;
        }

        else
        {

            myRigidbody.velocity *= -1;
            currentDeadLerp = 0f;
            actualVelocity = myRigidbody.velocity;

            isDead = true;
        } 
    }
}
