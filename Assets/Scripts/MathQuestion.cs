using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


public class MathQuestion : MonoBehaviour
{
    [Header("Panel & UI")]
    public GameObject panel;
    public TMP_Text questionText;
    public Button[] answerButtons;

    [Header("Feedback")]
    public TMP_Text feedbackText;
    public float feedbackDuration = 2f;

    [Header("Coins")]
    public int playerCoins = 0;
    public GameObject coinPickupTextPrefab;
    public Transform coinPickupSpawnPoint;
    public float coinTextDuration = 1.5f;

    [HideInInspector]
    public NPCMathTrigger currentNPC;

    private int correctAnswer;
    private Action<bool> callback;

    void Start()
    {
        panel.SetActive(false);
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);

        foreach (var btn in answerButtons)
        {
            btn.onClick.AddListener(() => OnAnswerClicked(btn));
        }
    }

    public void AskQuestion(int a, int b, Action<bool> callback)
    {
        this.callback = callback;
        correctAnswer = a + b;
        questionText.text = $"Vad är {a} + {b}?";

        // Slumpa svarsalternativ
        HashSet<int> answers = new HashSet<int> { correctAnswer };
        while (answers.Count < answerButtons.Length)
        {
            int wrong = UnityEngine.Random.Range(correctAnswer - 3, correctAnswer + 4);
            if (wrong != correctAnswer && wrong > 0)
                answers.Add(wrong);
        }

        List<int> answerList = new List<int>(answers);
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answerList[i].ToString();
        }

        panel.SetActive(true);
    }

    private void OnAnswerClicked(Button clickedButton)
    {
        int chosen = int.Parse(clickedButton.GetComponentInChildren<TMP_Text>().text);
        bool isCorrect = chosen == correctAnswer;

        if (isCorrect)
        {
            playerCoins += 1;
            ShowFeedback("Rätt svar!");

            if (coinPickupTextPrefab != null && coinPickupSpawnPoint != null)
            {
                GameObject coinText = Instantiate(coinPickupTextPrefab, coinPickupSpawnPoint.position, Quaternion.identity);
                coinText.transform.SetParent(coinPickupSpawnPoint, true);
                Destroy(coinText, coinTextDuration);
            }
        }
        else
        {
            ShowFeedback("Fel svar!");

            // Spawn EN fiende vid aktuell NPC
            currentNPC?.SpawnEnemy();
        }

        panel.SetActive(false);
        callback?.Invoke(isCorrect);
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
}
