using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum EGameState
{
    MainMenu,
    InGame
}

public class GameState : MonoBehaviour
{
    // Singleton instance
    private static GameState instance;

    public EGameState CurrentGameState { get; private set; }
    public UnityEvent<EGameState> OnGameStateChange;

    // Properties to store game stats
    public int EnemiesKilled { get; private set; }
    public int DamageDone { get; private set; }
    public int HighestLevelReached { get; private set; }


    //Settings
    public float SFXMultiplier { get; private set; }
    public float MusicMultiplier { get; private set; }

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                // Find existing instance in the scene
                instance = FindObjectOfType<GameState>();

                // If no instance exists, create a new one
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameState");
                    instance = singletonObject.AddComponent<GameState>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Ensure only one instance of GameState exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadGameState();
        SetGameState(EGameState.MainMenu);
    }

    // Function to set EnemiesKilled
    public void AddEnemiesKilled(int amount)
    {
        EnemiesKilled += amount;
    }

    // Function to set DamageDone
    public void AddDamageDone(int amount)
    {
        DamageDone += amount;
    }

    // Function to set HighestLevelReached
    public void SetHighestLevelReached(int level)
    {
        HighestLevelReached = level;
    }

    public void SetGameState(EGameState InGameState)
    {
        CurrentGameState = InGameState;
        OnGameStateChange?.Invoke(CurrentGameState);
    }

    public void SetMusicMultiplier(float InValue)
    {
        MusicMultiplier = InValue;
    }

    public void SetSFXMultiplier(float InValue)
    {
        SFXMultiplier = InValue;
    }

    // Save game stats to PlayerPrefs
    public void SaveGameState()
    {
        PlayerPrefs.SetInt("EnemiesKilled", EnemiesKilled);
        PlayerPrefs.SetInt("DamageDone", DamageDone);
        PlayerPrefs.SetInt("HighestLevelReached", HighestLevelReached);
        PlayerPrefs.Save();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SFXMutiplier", SFXMultiplier);
        PlayerPrefs.SetFloat("MusicMutiplier", MusicMultiplier);
        PlayerPrefs.Save();
    }

    // Load game stats from PlayerPrefs
    public void LoadGameState()
    {
        //Scores
        EnemiesKilled = PlayerPrefs.GetInt("EnemiesKilled", 0);
        DamageDone = PlayerPrefs.GetInt("DamageDone", 0);
        HighestLevelReached = PlayerPrefs.GetInt("HighestLevelReached", 0);

        //Settings
        SFXMultiplier = PlayerPrefs.GetFloat("SFXMutiplier", 1f);
        MusicMultiplier = PlayerPrefs.GetFloat("MusicMutiplier", 1f);
    }
}
