using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class which manages the game
/// </summary>
public class GameManager : MonoBehaviour
{
    // The script that manages all others
    public static GameManager instance = null;

    [Tooltip("The UIManager component which manages the current scene's UI")]
    public UIManager uiManager = null;

    [Tooltip("The player gameobject")]
    public GameObject player = null;

    // Getter for the player's health (private, because there shouldn't be a reason for other scripts to access)
    private Health playerHealth
    {
        get
        {
            if (player != null)
            {
                return player.GetComponent<Health>();
            }
            return null;
        }
    }

    // Current lives associated with the player's health script
    public int playerCurrentLives
    {
        get
        {
            if (playerHealth != null)
            {
                return playerHealth.currentLives;
            }
            return 0;
        }
    }

    [Header("Scores")]

    [Tooltip("The player's score")]
    [SerializeField] private int gameManagerScore = 0;

    // Static getter/setter for player score (for convenience)
    public static int score
    {
        get
        {
            return instance.gameManagerScore;
        }
        set
        {
            instance.gameManagerScore = value;
        }
    }

    // The highest score obtained by this player
    [Tooltip("The highest score acheived on this device")]
    public int highScore = 0;

    [Header("Game Progress / Victory Settings")]
    [Tooltip("Whether the game is winnable or not \nDefault: true")]
    public bool gameIsWinnable = true;

    [Tooltip("Page index in the UIManager to go to on winning the game")]
    public int gameVictoryPageIndex = 0;

    [Tooltip("The effect to create upon winning the game")]
    public GameObject victoryEffect;

    /// <summary>
    /// Description:
    /// When this component is first added or activated, setup the global reference
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        if (player == null)
        {
            player = FindObjectOfType<ThirdPersonCharacterController>().gameObject;
        }

    }

    /// <summary>
    /// Description:
    /// Less urgent startup behaviors, like loading highscores
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    private void Start()
    {
        
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScore = PlayerPrefs.GetInt("highscore");
        }
        if (PlayerPrefs.HasKey("score"))
        {
            score = PlayerPrefs.GetInt("score");
        }
        InitilizeGamePlayerPrefs();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once per frame
    /// Inputs:
    /// none
    /// Retuns:
    /// void
    /// </summary>
    private void Update()
    {
        UpdateUIElements();
    }


    /// <summary>
    /// Description:
    /// Sets up the game player prefs of the player's health and lives
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void InitilizeGamePlayerPrefs()
    {
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();

            // Set lives accordingly
            if (PlayerPrefs.GetInt("lives") == 0)
            {
                PlayerPrefs.SetInt("lives", playerHealth.currentLives);
            }

            playerHealth.currentLives = PlayerPrefs.GetInt("lives");

            // Set health accordingly
            if (PlayerPrefs.GetInt("health") == 0)
            {
                PlayerPrefs.SetInt("health", playerHealth.currentHealth);
            }

            playerHealth.currentHealth = PlayerPrefs.GetInt("health");
        }
        KeyRing.ClearKeyRing();
    }
    /// <summary>
    /// Description:
    /// Standard Unity function that gets called when the application (or playmode) ends
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveHighScore();
        ResetScore();
    }

    /// <summary>
    /// Description:
    /// Sends out a message to UI elements to update
    /// Input: N/A
    /// Returns: N/A
    /// </summary>
    public static void UpdateUIElements()
    {
        if (instance != null && instance.uiManager != null)
        {
            instance.uiManager.UpdateUI();
        }
    }

    /// <summary>
    /// Description:
    /// Ends the level, meant to be called when the level is complete (End of level reached)
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    public void LevelCleared()
    {
        if (uiManager != null)
        {
            player.gameObject.SetActive(false);
            player.GetComponent<ThirdPersonCharacterController>().playerRepresentation.gameObject.SetActive(false);
            player.GetComponent<ThirdPersonCharacterController>().playerCamera.GetComponent<ThirdPersonCamera>().enabled = false;
            uiManager.allowPause = false;
            uiManager.GoToPage(gameVictoryPageIndex);
            CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
            if (victoryEffect != null)
            {
                Instantiate(victoryEffect, transform.position, transform.rotation, null);
            }
        }     
    }

    [Header("Game Over Settings:")]
    [Tooltip("The index in the UI manager of the game over page")]
    public int gameOverPageIndex = 0;
    [Tooltip("The game over effect to create when the game is lost")]
    public GameObject gameOverEffect;

    // Whether or not the game is over
    [HideInInspector]
    public bool gameIsOver = false;

    /// <summary>
    /// Description:
    /// Displays game over screen
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public void GameOver()
    {
        gameIsOver = true;
        if (gameOverEffect != null)
        {
            Instantiate(gameOverEffect, transform.position, transform.rotation, null);
        }
        if (uiManager != null)
        {
            uiManager.allowPause = false;
            CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
            player.GetComponent<ThirdPersonCharacterController>().playerCamera.GetComponent<ThirdPersonCamera>().enabled = false;
            uiManager.GoToPage(gameOverPageIndex);
        }
    }

    /// <summary>
    /// Description:
    /// Adds a number to the player's score stored in the gameManager
    /// Input: int scoreAmount - the amount to add to the score
    /// Returns: N/A
    /// </summary>
    /// <param name="scoreAmount"></param>
    public static void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        if (score > instance.highScore)
        {
            SaveHighScore();
        }
        UpdateUIElements();
    }

    /// <summary>
    /// Description:
    /// Saves the highscore and then resets the current player score
    /// Inputs: N/A
    /// Returns: N/A
    /// </summary>
    public static void ResetScore()
    {
        PlayerPrefs.SetInt("score", 0);
        score = 0;
    }

    /// <summary>
    /// Description:
    /// Saves the player's highscore
    /// Input: N/A
    /// Returns: N/A
    /// </summary>
    public static void SaveHighScore()
    {
        if (score > instance.highScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            instance.highScore = score;
        }
        UpdateUIElements();
    }

    /// <summary>
    /// Description:
    /// Resets the high score in player preferences
    /// Inputs: N/A
    /// Returns: N/A
    /// </summary>
    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        if (instance != null)
        {
            instance.highScore = 0;
        }
        UpdateUIElements();
    }
}
