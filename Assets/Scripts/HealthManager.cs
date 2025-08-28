using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [Header("Anim")]
    [SerializeField] private Animator animator;
    private int hurtTriggerHash;
    private int deadTriggerHash;

    [Header("Health Settings")]
    public int MaxHealth = 3;
    public int currentHealth;

    [Header("UI")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite FullHeartSprite;
    [SerializeField] private Sprite EmptyHeartSprite;

    [Header("Effects")]
    public GameObject damageEffectPrefab;

    private GameObject Player;
    private bool canTakeDamage = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        hurtTriggerHash = Animator.StringToHash("hurt");
        deadTriggerHash = Animator.StringToHash("dead");
    }

    private IEnumerator Start()
    {
        Debug.Log("[HealthManager] currentHealth = " + currentHealth);


        Player = FindObjectOfType<PlayerController>()?.gameObject;
        if (Player == null)
        {
            Debug.LogError("[HealthManager] Hittar ingen PlayerController i scenen.");
            yield break;
        }

        if (animator == null)
        {
            animator = Player.GetComponentInChildren<Animator>();
            if (animator == null)
                Debug.LogError("[HealthManager] Hittar ingen Animator på spelaren eller dess barn.");
        }

        //currentHealth = currentHealth;
        DisplayHearts();

        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }

    // Kalla detta när spelaren faktiskt tar skada
    public void HurtPlayer()
    {
        if (!canTakeDamage) return;

        Debug.Log("[HealthManager] currentHealth = " + currentHealth);

        Debug.Log("[HealthManager] HurtPlayer() kallad.");

        if (currentHealth > 0)
        {
            currentHealth--;
            Debug.Log($"[HealthManager] Player hurt! Current health: {currentHealth}");
            DisplayHearts();
            SpawnDamageEffect();
            PlayHurtAnimation();
        }

        if (currentHealth <= 0)
        {
            PlayDeathAnimation();
            StartCoroutine(DeathDelay());
        }
    }

    // Kalla detta om du vill återställa hälsa
    public void HealPlayer(int amount)
    {
        if (currentHealth < MaxHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth);
            DisplayHearts();
            Debug.Log("[HealthManager] Healed! Current health: " + currentHealth);
        }
    }

    // Separata metoder för att testa animationer utan att skada spelaren
    public void PlayHurtAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger(hurtTriggerHash);
            animator.SetTrigger(hurtTriggerHash);
            Debug.Log("[HealthManager] Hurt animation played.");
        }
    }

    public void PlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(deadTriggerHash);
            Debug.Log("[HealthManager] Death animation played.");
        }
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.Death();
    }

    private void DisplayHearts()
    {
        Debug.Log($"Updating UI hearts: currentHealth = {currentHealth}");
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = (i < currentHealth) ? FullHeartSprite : EmptyHeartSprite;
    }

    private void SpawnDamageEffect()
    {
        if (damageEffectPrefab != null && Player != null)
            Instantiate(damageEffectPrefab, Player.transform.position, Quaternion.identity);
    }

    public bool CanBeDamaged() => canTakeDamage;





}
