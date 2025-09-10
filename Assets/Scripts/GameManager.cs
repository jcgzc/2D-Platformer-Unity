using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Math Level Settings")]
    public int mathLevel = 1;               // Startnivå
    public int mathQuestionsCorrect = 0;    // Räknar antal rätt
    public int mathQuestionsToLevelUp = 5;  // Rätt svar för att gå upp i nivå
    public int mathMaxLevel = 5;            // Maxnivå


    [SerializeField] private TMP_Text coinText;

    [SerializeField] private PlayerController playerController;

    private int coinCount = 0;
    private int gemCount = 0;
    private bool isGameOver = false;
    private Vector3 playerPosition;


    [SerializeField] private TMP_Text timerText;  // Dra in din Timer Text (TMP) från UI i Inspector
    private float elapsedTime = 0f;
    private bool isTimerRunning = true;



    //Level Complete

    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] TMP_Text leveCompletePanelTitle;
    [SerializeField] TMP_Text levelCompleteCoins;
    public GameObject gameOverPanel;  // Assign this in Inspector




    private int totalCoins = 0;

    public void PlayAgain()
    {
        Time.timeScale = 1f; // In case it was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (timerText != null)
            timerText.text = "00:00";



        coinText = GameObject.Find("CoinPickupText").GetComponent<TMP_Text>();
        UpdateGUI();
        UIManager.instance.fadeFromBlack = true;
        playerPosition = playerController.transform.position;

        FindTotalPickups();
    }

    public void IncrementCoinCount()
    {
        coinCount++;
        Debug.Log("Coin collected! Total: " + coinCount);
        UpdateGUI();
    }
    public void IncrementGemCount()
    {
        gemCount++;
        UpdateGUI();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }


    private void UpdateGUI()
    {
        coinText.text = coinCount.ToString();
  
    }

    public void Death()
    {
        Debug.Log("GameManager: Death() called");
        Debug.Log("[GameManager] Death() called");

        Debug.Log("Death() kallad! Player active: " + playerController.gameObject.activeSelf);



        isTimerRunning = false;

        if (!isGameOver)
        {
            // Disable Mobile Controls
            UIManager.instance.DisableMobileControls();
            // Initiate screen fade
            UIManager.instance.fadeToBlack = true;

            // Disable the player object
            playerController.gameObject.SetActive(false);

            // Show Game Over panel
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            // Pause the game
            Time.timeScale = 0f;

            // Start death coroutine to wait and then respawn the player (optional)
            StartCoroutine(DeathCoroutine());

            // Update game state
            isGameOver = true;

            // Log death message
            Debug.Log("Died");
        }
    }

    public void FindTotalPickups()
    {
        // Vanliga pickups
        pickup[] pickups = GameObject.FindObjectsOfType<pickup>();
        foreach (pickup pickupObject in pickups)
        {
            if (pickupObject.pt == pickup.pickupType.coin)
            {
                totalCoins += 1;
            }
        }

        // Coins från NPC:er
        CoinGiver[] npcCoinGivers = GameObject.FindObjectsOfType<CoinGiver>();
        foreach (CoinGiver giver in npcCoinGivers)
        {
            totalCoins += giver.coinsGiven;
        }
    }

    public void LevelComplete()
    {
        isTimerRunning = false;

        levelCompletePanel.SetActive(true);
        leveCompletePanelTitle.text = "LEVEL COMPLETE";

        levelCompleteCoins.text = "COINS COLLECTED: " + coinCount.ToString() + " / " + totalCoins.ToString();

        // Starta coroutine för att byta scen efter en liten delay
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2f); // tid att se panelen
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }


    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1f);
        playerController.transform.position = playerPosition;

        // Wait for 2 seconds
        yield return new WaitForSeconds(1f);

        // Check if the game is still over (in case player respawns earlier)
        if (isGameOver)
        {
            SceneManager.LoadScene(1);

            
        }
    }

    public void MathQuestionAnswered(bool isCorrect)
    {
        if (!isCorrect) return;

        mathQuestionsCorrect++;

        if (mathQuestionsCorrect >= mathQuestionsToLevelUp && mathLevel < mathMaxLevel)
        {
            mathLevel++;
            mathQuestionsCorrect = 0;
            Debug.Log($"Grattis! Du har nått Math Level {mathLevel}!");
            // Här kan du t.ex. visa ett UI-feedback på skärmen
        }
    }



}
