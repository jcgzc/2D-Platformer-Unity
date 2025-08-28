using UnityEngine;
using System.Collections;

public class WaterDamageZone : MonoBehaviour
{
    public float damageInterval = 1.5f; // sekunder mellan skador
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ApplyDamageOverTime(other));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines(); // Slutar skada när spelaren lämnar
        }
    }

    private IEnumerator ApplyDamageOverTime(Collider2D player)
    {
        while (true)
        {
            if (HealthManager.instance != null)
            {
                HealthManager.instance.HurtPlayer();
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
