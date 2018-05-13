using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public GameObject parentTurret;

    private void OnCollisionEnter(Collision objectCollided)
    {
        if (objectCollided.gameObject.layer != parentTurret.layer)
        {
            if (objectCollided.transform.tag.Equals("Player"))
            {
                parentTurret.GetComponent<Turret>().TargetEliminated();
            }

            // So the trail stays on the scene before get removed like the projectile
            Transform trailChild = this.transform.Find("Trail");
            trailChild.parent = null;

            Destroy(trailChild.gameObject, 1);
            Destroy(this.gameObject);
        }
    }
}
