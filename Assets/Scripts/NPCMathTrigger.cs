using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class NPCMathTrigger : MonoBehaviour
{
    [Header("Math Question Panel")]
    public MathQuestion mathQuestionPanel;

    [Header("Reward")]
    public GameObject coinPrefab;
    public float spawnHeight = 1.5f; // Hur högt ovanför NPC:n coinen spawnar

    [Header("Enemy Spawn")]
    public GameObject enemyPrefab;
    public float spawnRadius = 2f;

    [Header("Feedback Text")]
    public TMP_Text feedbackText;
    public float feedbackDuration = 2f;

    private bool playerInRange = false;
    private bool hasRewarded = false;

    void Start()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowMathQuestion();
        }
    }

    private void ShowMathQuestion()
    {
        if (mathQuestionPanel == null)
        {
            Debug.LogWarning("MathQuestion-panel är inte satt på NPC!");
            return;
        }

        // Denna NPC blir "aktiv" i MathQuestion
        mathQuestionPanel.currentNPC = this;

        int a = UnityEngine.Random.Range(1, 10);
        int b = UnityEngine.Random.Range(1, 10);

        // Ställ frågan
        mathQuestionPanel.AskQuestion(a, b, OnPlayerAnswered);
    }

    private void OnPlayerAnswered(bool isCorrect)
    {
        if (isCorrect)
        {
            ShowFeedback("Rätt svar!");

            if (!hasRewarded)
            {
                hasRewarded = true;
                GiveReward();
            }
        }
        else
        {
            ShowFeedback("Fel svar!");

            // Spawn EN fiende vid denna NPC
            SpawnEnemy();
        }
    }

    private void GiveReward()
    {
        if (coinPrefab == null) return;

        Vector3 spawnPos = transform.position + new Vector3(0f, spawnHeight, 0f);
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        Vector2 spawnPos = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * spawnRadius;
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideFeedback));
        Invoke(nameof(HideFeedback), feedbackDuration);
    }

    private void HideFeedback()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
