using UnityEngine;

public class NPCQuest : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform coinSpawnPoint;

    private int correctAnswer;
    private bool playerInRange;
    public bool HasRewarded { get; private set; } = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[NPCQuest] Player pressed E, showing math question.");
            ShowMathQuestion();
        }
    }



void ShowMathQuestion()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int op = Random.Range(0, 4);
        string question = "";

        Debug.Log("DialogManager.Instance = " + DialogManager.Instance);
        if (DialogManager.Instance == null)
        {
            Debug.LogError("DialogManager.Instance is null! Attempting to recreate or check scene.");
            // Optional: Recreate if needed (use with caution)
            GameObject dm = GameObject.Find("DialogManager");
            if (dm != null)
            {
                DialogManager.Instance = dm.GetComponent<DialogManager>();
                Debug.Log("Recovered DialogManager instance");
            }
            else
            {
                Debug.LogWarning("No DialogManager found in scene!");
                return;
            }
        }

        switch (op)
        {
            case 0:
                correctAnswer = a + b;
                question = $"{a} + {b} = ?";
                break;
            case 1:
                correctAnswer = a - b;
                question = $"{a} - {b} = ?";
                break;
            case 2:
                correctAnswer = a * b;
                question = $"{a} × {b} = ?";
                break;
            case 3:
                correctAnswer = a;
                b = Random.Range(1, 10);
                question = $"{a * b} ÷ {b} = ?";
                break;
        }

        DialogManager.Instance.StartDialog(this, question, correctAnswer);
    }


    public void GiveReward()
    {
        if (HasRewarded) return;
        HasRewarded = true;

        Vector3 spawnPos = coinSpawnPoint != null
            ? coinSpawnPoint.position
            : transform.position + new Vector3(1f, 1f, 0);

        GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (DialogManager.Instance != null)
                DialogManager.Instance.CloseDialog();
        }
    }
}
