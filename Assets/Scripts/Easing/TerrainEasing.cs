using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEasing : MonoBehaviour
{
    public bool bluePlatform = false;           // To check if its a blue platform, which means is a platform with movement and cant move till the tweening finishes

    [Space]
    [SerializeField] private Transform terrainMesh;
    public Transform initialPosition;


    EasingFunction.Ease easeFunction;

    Vector3[] easingVector = new Vector3[3];    // 0 - Position | 1 - Rotation | 2 - Scale
    Vector3[] initialVector = new Vector3[3];
    Vector3[] desiredVector = new Vector3[3];

    float currentLerpTime = 0;
    float delay = 0;
    float lerpTime = 0;

    // Sinus floating value after the lerp
    float floatingValue = 0;
    float floatingAmplitude = 0.0005f;
    float floatingDelay = 0;

    void Start()
    {
        Begin();
    }

    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }

        else if (currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            float t = currentLerpTime / lerpTime;

            // Position
            easingVector[0].x = EasingFunction.GetEasingFunction(easeFunction)(initialVector[0].x, desiredVector[0].x, t);
            easingVector[0].y = EasingFunction.GetEasingFunction(easeFunction)(initialVector[0].y, desiredVector[0].y, t);
            easingVector[0].z = EasingFunction.GetEasingFunction(easeFunction)(initialVector[0].z, desiredVector[0].z, t);

            //easingPosition = EasingFunction.EaseOutBounce(initialCoord, desiredCoord, t);

            // Rotation
            easingVector[1].x = EasingFunction.EaseOutCubic(initialVector[1].x, desiredVector[1].x, t);
            easingVector[1].y = EasingFunction.EaseOutCubic(initialVector[1].y, desiredVector[1].y, t);
            easingVector[1].z = EasingFunction.EaseOutCubic(initialVector[1].z, desiredVector[1].z, t);

            // Scale
            easingVector[2].x = EasingFunction.EaseOutBounce(initialVector[2].y, desiredVector[2].y, t);
            easingVector[2].y = EasingFunction.EaseOutBounce(initialVector[2].y, desiredVector[2].y, t);

            terrainMesh.localPosition = easingVector[0];
            terrainMesh.eulerAngles = easingVector[1];
            terrainMesh.localScale = easingVector[2];
        }

        else if (floatingDelay > 0)
        {
            floatingDelay -= Time.deltaTime;

            if (bluePlatform)
                terrainMesh.GetComponent<BluePlatform>().CanMove();
        }
        else
        {
            if (!bluePlatform)
            {
                floatingValue += Time.deltaTime;

                Vector3 newFloatingVector = terrainMesh.localPosition;
                newFloatingVector.y = newFloatingVector.y + floatingAmplitude * Mathf.Sin(floatingValue);

                terrainMesh.localPosition = newFloatingVector;
            }  
        }
    }

    public void Begin()
    {
        if (Settings.settings.EFFECT_TWEENING_ENABLED)
        {
            desiredVector[0] = terrainMesh.localPosition;
            easingVector[0] = initialPosition.localPosition;
            initialVector[0] = initialPosition.localPosition;

            // Since we start at the desire position, and we do this because its easier building the map like this, we got to set the position at the initialPosition so it 
            // lerps to the final position which is the one that we set manually while building.
            terrainMesh.localPosition = initialVector[0];


            easeFunction = Settings.settings.EASE_FUNCTION;

            if (Settings.settings.EFFECT_TWEENING_SCALE)
            {
                easingVector[2] = new Vector3(0.5f, 0.5f);
                initialVector[2] = easingVector[2];
                desiredVector[2] = transform.localScale;
            }
            else
            {
                easingVector[2] = transform.localScale;
                desiredVector[2] = transform.localScale;
                initialVector[2] = transform.localScale;
            }

            if (Settings.settings.EFFECT_TWEENING_ROTATION)
            {
                easingVector[1] = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(45, -45));
                initialVector[1] = easingVector[1];
                desiredVector[1] = transform.rotation.eulerAngles;
            }
            else
            {
                easingVector[1] = transform.rotation.eulerAngles;
                desiredVector[1] = transform.rotation.eulerAngles;
                initialVector[1] = transform.rotation.eulerAngles;
            }

            floatingValue = 0;
            floatingDelay = Random.Range(0.4f, 0.7f);
            currentLerpTime = 0;
            delay = Random.Range(0, Settings.settings.EFFECT_TWEENING_DELAY);
            lerpTime = Settings.settings.EFFECT_TWEENING_DURATION;
        }
        else
        {
            // Transform
            desiredVector[0] = terrainMesh.position;

            // Scale
            easingVector[2] = transform.localScale;
            desiredVector[2] = transform.localScale;
            initialVector[2] = transform.localScale;

            terrainMesh.position = desiredVector[0];
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(initialPosition.position, 0.25f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(initialPosition.position, 0.6f);

        Gizmos.DrawLine(initialPosition.position, terrainMesh.position);
    }
}
