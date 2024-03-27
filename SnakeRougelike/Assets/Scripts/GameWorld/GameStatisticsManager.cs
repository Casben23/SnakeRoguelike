using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameStats
{
    public int EnemiesKilledThisLevel = 0;
    public int CashGainedThisLevel = 0;
    public int PartsLostThisLevel = 0;

    public int LevelReached = 0;
    public int EnemiesKilled = 0;
    public int DamageDealt = 0;
    public int CurrentLevel = 0;
}

public class GameStatisticsManager : MonoBehaviour
{
    private static GameStatisticsManager instance;

    public static GameStatisticsManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Ensure only one instance of the WaveController exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private GameStats m_GameStats = new GameStats();

    public GameStats GetGameStats()
    {
        return m_GameStats;
    }
}
