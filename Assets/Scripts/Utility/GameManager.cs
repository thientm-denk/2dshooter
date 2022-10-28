using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    [Header("Scores")]
    // The current player score in the game
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
    [Tooltip("The number of enemies that must be defeated to win the game")]
    public int enemiesToDefeat = 10;
    
    // The number of enemies defeated in game
    private int enemiesDefeated = 0;

    [Tooltip("Whether or not to print debug statements about whether the game can be won or not according to the game manager's" +
        " search at start up")]
    public bool printDebugOfWinnableStatus = true;
    [Tooltip("Page index in the UIManager to go to on winning the game")]
    public int gameVictoryPageIndex = 0;
    [Tooltip("The effect to create upon winning the game")]
    public GameObject victoryEffect;

    //The number of enemies observed by the game manager in this scene at start up"
    private int numberOfEnemiesFoundAtStart;

    /// <summary>
    /// Description:
    /// Standard Unity function called when the script is loaded, called before start
    /// 
    /// When this component is first added or activated, setup the global reference
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first Update
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void Start()
    {
        HandleStartUp();
    }

    /// <summary>
    /// Description:
    /// Handles necessary activities on start up such as getting the highscore and score, updating UI elements, 
    /// and checking the number of enemies
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void HandleStartUp()
    {
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScore = PlayerPrefs.GetInt("highscore");
        }
        if (PlayerPrefs.HasKey("score"))
        {
            score = PlayerPrefs.GetInt("score");
        }
        UpdateUIElements();
        if (printDebugOfWinnableStatus)
        {
            FigureOutHowManyEnemiesExist();
        }
    }

    /// <summary>
    /// Description:
    /// Searches the level for all spawners and static enemies.
    /// Only produces debug messages / warnings if the game is set to be winnable
    /// If there are any infinite spawners a debug message will say so,
    /// If there are more enemies than the number of enemies to defeat to win
    /// then a debug message will say so
    /// If there are too few enemies to defeat to win then a debug warning will say so
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    private void FigureOutHowManyEnemiesExist()
    {
        List<EnemySpawner> enemySpawners = FindObjectsOfType<EnemySpawner>().ToList();
        List<Enemy> staticEnemies = FindObjectsOfType<Enemy>().ToList();

        int numberOfInfiniteSpawners = 0;
        int enemiesFromSpawners = 0;
        int enemiesFromStatic = staticEnemies.Count;
        foreach(EnemySpawner enemySpawner in enemySpawners)
        {
            if (enemySpawner.spawnInfinite)
            {
                numberOfInfiniteSpawners += 1;
            }
            else
            {
                enemiesFromSpawners += enemySpawner.maxSpawn;
            }
        }
        numberOfEnemiesFoundAtStart = enemiesFromSpawners + enemiesFromStatic;

        if (gameIsWinnable)
        {
            if (numberOfInfiniteSpawners > 0)
            {
                Debug.Log("There are " + numberOfInfiniteSpawners + " infinite spawners " + " so the level will always be winnable, "
                    + "\nhowever you sshould still playtest for timely completion");
            }
            else if (enemiesToDefeat > numberOfEnemiesFoundAtStart)
            {
                Debug.LogWarning("There are " + enemiesToDefeat + " enemies to defeat but only " + numberOfEnemiesFoundAtStart + 
                    " enemies found at start \nThe level can not be completed!");
            }
            else
            {
                Debug.Log("There are " + enemiesToDefeat + " enemies to defeat and " + numberOfEnemiesFoundAtStart +
                    " enemies found at start \nThe level can completed");
            }
        }
    }

    public static int CountEnemies()
    {
        List<EnemySpawner> enemySpawners = FindObjectsOfType<EnemySpawner>().ToList();
        List<Enemy> staticEnemies = FindObjectsOfType<Enemy>().ToList();

        int numberOfInfiniteSpawners = 0;
        int enemiesFromSpawners = 0;
        int enemiesFromStatic = staticEnemies.Count;
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            if (enemySpawner.spawnInfinite)
            {
                numberOfInfiniteSpawners += 1;
            }
            else
            {
                enemiesFromSpawners += enemySpawner.maxSpawn;
            }
        }
        
        return enemiesFromSpawners + enemiesFromStatic;

    }
    /// <summary>
    /// Description:
    /// Increments the number of enemies defeated by 1
    /// Input:
    /// none
    /// Return:
    /// void (no returned value)
    /// </summary>
    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated++;
        if (enemiesDefeated >= enemiesToDefeat && gameIsWinnable)
        {
            LevelCleared();
        }
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
    /// Adds a number to the player's score stored in the gameManager
    /// Input: 
    /// int scoreAmount
    /// Returns: 
    /// void (no return)
    /// </summary>
    /// <param name="scoreAmount">The amount to add to the score</param>
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
    /// Resets the current player score
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    public static void ResetScore()
    {
        PlayerPrefs.SetInt("score", 0);
        score = 0;
    }

    /// <summary>
    /// Description:
    /// Saves the player's highscore
    /// Input: 
    /// none
    /// Returns: 
    /// void (no return)
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
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
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

    /// <summary>
    /// Description:
    /// Sends out a message to UI elements to update
    /// Input: 
    /// none
    /// Returns: 
    /// void (no return)
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
    /// Ends the level, meant to be called when the level is complete (enough enemies have been defeated)
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    public void LevelCleared()
    {
        PlayerPrefs.SetInt("score", score);
        if (uiManager != null)
        {
            player.SetActive(false);
            uiManager.allowPause = false;
            uiManager.GoToPage(gameVictoryPageIndex);
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
            uiManager.GoToPage(gameOverPageIndex);
        }
    }
}
