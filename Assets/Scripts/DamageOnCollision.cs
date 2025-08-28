using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && HealthManager.instance != null)
        {
            Debug.Log("Trigger entered by Player!");
            if (!HealthManager.instance.CanBeDamaged()) return;

            for (int i = 0; i < damageAmount; i++)
            {
                HealthManager.instance.HurtPlayer();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && HealthManager.instance != null)
        {
            Debug.Log("Collision entered by Player!");
            if (!HealthManager.instance.CanBeDamaged()) return;

            for (int i = 0; i < damageAmount; i++)
            {
                HealthManager.instance.HurtPlayer();
            }
        }
    }

}
