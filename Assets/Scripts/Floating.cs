using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float speed = 1.1f;
    public float amplitude = 0.1f;

    Vector3 floatY;
    float x, y;


    void Start()
    {
        x = transform.localPosition.x;
        y = transform.localPosition.y;
    }

    void Update()
    {
        floatY.y = (Mathf.Sin(Time.time * speed) * amplitude);
        transform.localPosition = new Vector3(x, y + floatY.y, 0);
    }
}
