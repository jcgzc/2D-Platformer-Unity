using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    public float floatAmplitude = 0.25f; // How much it floats up and down
    public float floatFrequency = 1f;    // Speed of floating

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Float up and down using sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
