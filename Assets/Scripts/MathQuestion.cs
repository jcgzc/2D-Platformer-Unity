using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


public class MathQuestion : MonoBehaviour
{
    public enum Operation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    [Header("Level Settings")]
    //[Range(1, 5)]
    //public int currentLevel = 1;  // 1–5 nivåer
    public int questionsCorrect = 0; // Räknar antal rätt
    public int questionsToLevelUp = 5; // Antal rätt för att gå till nästa nivå
    public int maxLevel = 5;         // Maxnivå

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

    // Välj operation baserat på level
    private Operation GetOperationForLevel(int level)
    {
        switch (level)
        {
            case 1: return Operation.Addition;
            case 2: return Operation.Subtraction;
            case 3: return Operation.Multiplication;
            case 4: return Operation.Division;
            case 5:
                // slumpa mellan alla 4 operationer
                int randomOp = UnityEngine.Random.Range(0, 4); // 0–3
                return (Operation)randomOp;
            default:
                return Operation.Addition;
        }
    }

    public void AskQuestion(int a, int b, Action<bool> callback)
    {
        this.callback = callback;

        Operation operation = GetOperationForLevel(GameManager.instance.mathLevel);


        switch (operation)
        {
            case Operation.Addition:
                correctAnswer = a + b;
                questionText.text = $"Vad är {a} + {b}?";
                break;

            case Operation.Subtraction:
                if (b > a) (a, b) = (b, a); // undvik negativa
                correctAnswer = a - b;
                questionText.text = $"Vad är {a} - {b}?";
                break;

            case Operation.Multiplication:
                correctAnswer = a * b;
                questionText.text = $"Vad är {a} × {b}?";
                break;

            case Operation.Division:
                // gör division jämn
                int result = UnityEngine.Random.Range(2, 10);
                correctAnswer = result;
                b = UnityEngine.Random.Range(2, 10);
                a = correctAnswer * b;
                questionText.text = $"Vad är {a} ÷ {b}?";
                break;
        }

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
            GameManager.instance.MathQuestionAnswered(true);

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
