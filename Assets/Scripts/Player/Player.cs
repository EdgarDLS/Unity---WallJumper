using UnityEngine;

public class Player : MonoBehaviour
{
    [Space]
    public float movementSpeed = 2400  ;                // Jump force
    public MeshCollider terrainCollider;                // Collider of the terrain the player is sitting on | Used to ignore the collision between this and the player at the jumping moment 

    [Space]
    [SerializeField] private Transform checkpoint;

    private GameObject sittingTerrain;
    private Rigidbody myRigidbody;
    private SphereCollider myCollider;
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
        myCollider = this.gameObject.GetComponent<SphereCollider>();
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
                    /** Try in the future instead of ignoring collision, a set of raycast with the size of the sphere **/
                    // To ignore the collision of the terrain the player is sitting on, to avoid different behaviors due to the collision at the moment of trying to jump.
                    if (terrainCollider != null) Physics.IgnoreCollision(myCollider, terrainCollider, true);

                    this.transform.parent = null;
                    myRigidbody.AddForce(jumpDirection * movementSpeed);
                    jumpAction = true;
                }
            }
        }

        /** Got to be changed in the future **/
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
        GameMaster._GM.GlitchEffect();

        if (terrainCollider != null) Physics.IgnoreCollision(myCollider, terrainCollider, false);

        myRigidbody.velocity = Vector3.zero;
        isDead = false;
        jumpAction = false;

    
    }

    // NOT BEING USED FOR THE MOMENT
    public void Die()
    {
        myRigidbody.velocity = Vector3.zero;
        isDead = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Lava") && !collision.gameObject.tag.Equals("Projectile"))
        {
            if (terrainCollider != null) Physics.IgnoreCollision(myCollider, terrainCollider, false);

            sittingTerrain = collision.gameObject;          // Set the new terrain where the player is sitting on
            jumpAction = false;                             // Let the player jump again
            myRigidbody.velocity = Vector3.zero;            // Velocity = 0 so it stops on collision
            terrainCollider = collision.gameObject.GetComponent<MeshCollider>();

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
