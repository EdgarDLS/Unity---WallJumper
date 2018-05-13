using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    public float bulletForce = 2000;
    public float fireRate = 1f;
    private float fireTimer = 0;

    private Transform launchPosition;
    private Transform turretBody;

    private Transform target;

    [Space]
    [SerializeField] private int layerMask = 1 << 9;

    private void Start()
    {
        turretBody = this.transform.Find("Body");
        launchPosition = turretBody.Find("Barrel").transform.Find("launchPosition");

        layerMask = ~layerMask;
    }

    void Update()
    {
        if (target != null)
        {
            // Check if the player is in sight
            Vector3 direction = (target.position - this.transform.position).normalized;
            RaycastHit hit;

            Debug.DrawRay(this.transform.position, direction * 100, Color.red);

            if (Physics.Raycast(this.transform.position, direction, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.gameObject.tag.Equals("Player"))
                {
                    // Rotate to the players direction
                    turretBody.LookAt(target);

                    // Fire
                    if (fireTimer >= fireRate)
                    {
                        GameObject newBullet = Instantiate(bulletPrefab, launchPosition.position, launchPosition.rotation) as GameObject;
                        newBullet.GetComponent<TurretProjectile>().parentTurret = this.gameObject;  // So when the bullet hits the player, the bullet informs the turret and the turret stops shooting
                        
                        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * bulletForce);

                        fireTimer = 0;
                    }

                    fireTimer += Time.deltaTime;

                }
            }    
        }
    }

    public void TargetEliminated()
    {
        target = null;

        fireTimer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
            target = null;
    }
}
