using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypointsPosition;       // Positions of the waypoints that are going to be created manually in the scene

    [Space]
    public float lerpTime = 1f;

    private bool movementReady = false;     // To check if the tweening finished.
    private int fromWaypoint = 0;            // Current waypoint the platform is trying to reach
    private int toWaypoint = 1;
    private float percentBetweenWaypoints;

    private float currentLerpTime = 0;

    void Update ()
    {
        if (movementReady)
        {
            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > lerpTime)
            {
                if (fromWaypoint == 0)
                {
                    fromWaypoint = 1;
                    toWaypoint = 0;
                }
                else
                {
                    fromWaypoint = 0;
                    toWaypoint = 1;
                }

                currentLerpTime = 0f;
            }


            float t = currentLerpTime / lerpTime;
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(waypointsPosition[fromWaypoint].position, waypointsPosition[toWaypoint].position, t);
        } 
    }

    public void CanMove()
    {
        movementReady = true;
    }

    void OnDrawGizmos()
    {
        for (int i= 0; i < waypointsPosition.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(waypointsPosition[i].position, 0.25f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(waypointsPosition[i].position, 0.6f);

            Gizmos.DrawLine(waypointsPosition[i].position, this.transform.position);
        }
    }
}
