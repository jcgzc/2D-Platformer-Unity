using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnDelay : MonoBehaviour
{
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    void Start()
    {
        // Enable after short delay
        Invoke("EnableCollider", 0.5f);
    }

    void EnableCollider()
    {
        if (col != null)
            col.enabled = true;
    }
}

