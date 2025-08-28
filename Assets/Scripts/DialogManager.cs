using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public TMP_Text feedbackText;
    public Button submitButton;

    private int correctAnswer;
    private NPCQuest activeNPC;
    private bool isActive = false;


void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("DialogManager initialized as singleton");
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"Duplicate DialogManager detected, destroying this instance. Original: {Instance.name}, This: {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        // Verify all references are assigned in the Inspector
        if (dialoguePanel == null) Debug.LogError("dialoguePanel is not assigned in Inspector!");
        if (questionText == null) Debug.LogError("questionText is not assigned in Inspector!");
        if (answerInput == null) Debug.LogError("answerInput is not assigned in Inspector!");
        if (feedbackText == null) Debug.LogError("feedbackText is not assigned in Inspector!");
        if (submitButton == null) Debug.LogError("submitButton is not assigned in Inspector!");

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmitAnswer);
    }


    public void StartDialog(NPCQuest npc, string question, int answer)
    {
        if (dialoguePanel == null || questionText == null || answerInput == null || feedbackText == null)
        {
            Debug.LogError("One or more dialogue components are missing!");
            return;
        }

        activeNPC = npc;
        correctAnswer = answer;
        isActive = true;

        dialoguePanel.SetActive(true);
        feedbackText.text = "";
        answerInput.text = "";
        questionText.text = question;
    }

    void OnSubmitAnswer()
    {
        if (!isActive || activeNPC == null || dialoguePanel == null || feedbackText == null || answerInput == null)
            return;

        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer && !activeNPC.HasRewarded)
            {
                feedbackText.text = "Correct!";
                activeNPC.GiveReward();
                StartCoroutine(CloseAfterDelay());
            }
            else
            {
                feedbackText.text = "Wrong, try again.";
            }
        }
        else
        {
            feedbackText.text = "Please enter a number!";
        }
    }

    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(2);
        CloseDialog();
    }

    public void CloseDialog()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        isActive = false;
        activeNPC = null;
    }
}