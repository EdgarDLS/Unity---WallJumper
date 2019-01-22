using UnityEngine;

public class Player : MonoBehaviour
{
    [Space]
    public float movementSpeed = 2400  ;                // Jump force
    public GameObject deathParticles;
    [SerializeField] AudioSource daethSound;
    [SerializeField] AudioSource godModeSound;

    [Space]
    [SerializeField] private Transform checkpoint;

    private MeshCollider terrainCollider;                // Collider of the terrain the player is sitting on | Used to ignore the collision between this and the player at the jumping moment 
    private GameObject sittingTerrain;
    private Rigidbody myRigidbody;
    private SphereCollider myCollider;
    private MeshRenderer myMesh;
    private AudioSource glitchSound;
    private Camera mainCamera;
    private bool jumpAction;
    private Vector3 jumpDirection;

    // Dead variables
    private bool isDead = false;
    //private float deadLerp = 0.7f;
    //private float currentDeadLerp;
    //private Vector3 actualVelocity;

    private bool godMode = false;

	void Start ()
    {
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        myCollider = this.gameObject.GetComponent<SphereCollider>();
        myMesh = this.gameObject.GetComponent<MeshRenderer>();
        glitchSound = this.gameObject.GetComponent<AudioSource>();
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

            else if (Input.GetKeyDown(KeyCode.G))
            {
                if (godMode) godMode = false;
                else godMode = true;
                godModeSound.Play();
            }
                
        }

        /** Got to be changed in the future **/
        // Little effect once the player dies
        //else
        //{
        //    currentDeadLerp += Time.deltaTime;

        //    if (currentDeadLerp > deadLerp)
        //        currentDeadLerp = deadLerp;

        //    float perc = currentDeadLerp / deadLerp;

        //    myRigidbody.velocity = Vector3.Lerp(actualVelocity, Vector3.zero, perc);
        //}

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
        glitchSound.Play();

        myMesh.enabled = true;

        if (terrainCollider != null) Physics.IgnoreCollision(myCollider, terrainCollider, false);

        myRigidbody.velocity = Vector3.zero;
        isDead = false;
        jumpAction = false;

        this.transform.position = checkpoint.transform.position;
    }

    public void Die()
    {
        //myRigidbody.velocity *= -1;
        //currentDeadLerp = 0f;
        //actualVelocity = myRigidbody.velocity;

        myRigidbody.velocity = Vector3.zero;
        myMesh.enabled = false;
        Destroy(Instantiate(deathParticles, this.transform.position, Quaternion.FromToRotation(Vector3.forward, jumpDirection)) as GameObject, 2);
        daethSound.Play();

        isDead = true;
    }

    public void PrepareNextLevel()
    {
        myRigidbody.velocity = Vector3.zero;
        myMesh.enabled = false;

        isDead = true;
    }

    public void SetCheckpoint(Transform _checkpoint)
    {
        checkpoint = _checkpoint;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (godMode)
        {
            if (terrainCollider != null) Physics.IgnoreCollision(myCollider, terrainCollider, false);       // Turn the avoiding false to collide with the same platform in the future.

            sittingTerrain = collision.gameObject;          // Set the new terrain where the player is sitting on
            jumpAction = false;                             // Let the player jump again
            myRigidbody.velocity = Vector3.zero;            // Velocity = 0 so it stops on collision
            terrainCollider = collision.gameObject.GetComponent<MeshCollider>();

            this.transform.parent = collision.transform;    // Parent so it moves with the platform
        }

        else if (!collision.gameObject.tag.Equals("Lava") && !collision.gameObject.tag.Equals("Projectile"))
        {
            if (terrainCollider != null) Physics.IgnoreCollision(myCollider, terrainCollider, false);       // Turn the avoiding false to collide with the same platform in the future.

            sittingTerrain = collision.gameObject;          // Set the new terrain where the player is sitting on
            jumpAction = false;                             // Let the player jump again
            myRigidbody.velocity = Vector3.zero;            // Velocity = 0 so it stops on collision
            terrainCollider = collision.gameObject.GetComponent<MeshCollider>();

            this.transform.parent = collision.transform;    // Parent so it moves with the platform
        }

        else
        {
            Die();
            GameMaster._GM.PlayerDead();
        } 
    }
}
