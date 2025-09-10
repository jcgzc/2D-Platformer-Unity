using UnityEngine;

public class NPCHealthTrigger : NPCMathTrigger
{
    [Header("Health Reward")]
    public GameObject healthPickupPrefab;  // Prefab with HealthPickup.cs
    public float healthSpawnHeight = 1.5f; // separate adjustable spawn height

    protected override void GiveReward()
    {
        if (healthPickupPrefab == null) return;

        Vector3 spawnPos = transform.position + new Vector3(0f, healthSpawnHeight, 0f);
        Instantiate(healthPickupPrefab, spawnPos, Quaternion.identity);
    }
}
