using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 1;        // Skada per träff
    public float damageCooldown = 1f;   // Tid mellan skador

    private bool canDamage = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && canDamage)
        {
            if (HealthManager.instance != null)
                HealthManager.instance.HurtPlayer();

            canDamage = false;
            Invoke(nameof(ResetDamage), damageCooldown);
        }
    }

    private void ResetDamage()
    {
        canDamage = true;
    }
}
