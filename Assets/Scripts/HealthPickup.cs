using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && HealthManager.instance != null)
        {
            Debug.Log("Player picked up health!");
            HealthManager.instance.HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }
}


