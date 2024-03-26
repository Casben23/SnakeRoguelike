using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySpawn
{
    public GameObject Enemy;
    public int Cost;
}

enum EWaveState
{
    Idle,
    Battle
}

public class WaveController : MonoBehaviour
{
    private static WaveController instance;

    public static WaveController Instance
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

    [SerializeField] private List<EnemySpawn> m_EnemySpawns;
    [SerializeField] private TextMeshProUGUI m_UIWaveText;
    [SerializeField] private float m_TimeBtwWaves = 2;
    [SerializeField] private int m_WaveAmount = 5;

    private EWaveState m_CurrentWaveState = EWaveState.Idle;
    private float m_CurrentTimeBtwWaves = 0;
    private float m_CurrentWave = 0;

    private int m_CurrentSpawnValue = 0;

    private WaveSpawnManager m_SpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        m_EnemySpawns.Sort((x, y) => x.Cost.CompareTo(y.Cost));

        m_SpawnManager = gameObject.GetComponent<WaveSpawnManager>();
        StartNewWave();
    }

    public void OnLevelStart()
    {
        m_CurrentWave = 0;
        StartNewWave();
    }

    private void ScaleWaveStats()
    {
        int currentLevel = GameManager.Instance.GetCurrentLevel();

        m_CurrentSpawnValue = 1;
        m_CurrentSpawnValue *= 10 * currentLevel;

        int waveAmount = Mathf.FloorToInt(currentLevel);
        m_WaveAmount = Mathf.Clamp(waveAmount, 1, 10);
    }

    private void StartNewWave()
    {
        m_CurrentWave += 1;
        ScaleWaveStats();

        m_CurrentWaveState = EWaveState.Idle;
        m_CurrentTimeBtwWaves = m_TimeBtwWaves;
        m_UIWaveText.text = "WAVE [" + m_CurrentWave.ToString() + "/" + m_WaveAmount.ToString() + "]";
        m_UIWaveText.gameObject?.GetComponent<Animator>().SetTrigger("NewWave");

        SoundManager.Instance.PlayGeneralSound(SFXType.NewWave, false);
        //UI MANAGER CALL WAVE TEXT UPDATE!
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaveState();
    }

    private void UpdateWaveState()
    {
        switch (m_CurrentWaveState)
        {
            case EWaveState.Idle:
                UpdateIdleState();
                break;
            case EWaveState.Battle:
                UpdateBattleState();
                break;
            default:
                break;
        }
    }

    private void UpdateIdleState()
    {
        if (m_CurrentTimeBtwWaves <= 0)
        {
            StartWave();
            return;
        }

        m_CurrentTimeBtwWaves -= Time.deltaTime;
    }

    private void UpdateBattleState()
    {
        if (m_SpawnManager.IsWaveCompleted())
        {
            if(m_CurrentWave >= m_WaveAmount)
            {
                if (!GameManager.Instance.IsUpgradeScreenOpen())
                {
                    GameManager.Instance.OnLevelComplete();
                }
                return;
            }

            StartNewWave();
        }
    }

    private void StartWave()
    {
        m_CurrentWaveState = EWaveState.Battle;
        m_SpawnManager.InitiateNewWave(GetEnemyEncounterList());
    }

    private List<EnemySpawn> GetEnemyEncounterList()
    {
        List<EnemySpawn> resultEnemyList = new List<EnemySpawn>();

        int currentSpawnValue = m_CurrentSpawnValue;
        bool creatingList = true;
        while (creatingList)
        {
            int availableEnemies = 0;
            foreach (EnemySpawn enemy in m_EnemySpawns)
            {
                if (enemy.Cost <= currentSpawnValue)
                {
                    availableEnemies++;
                }
            }

            if (availableEnemies == 0)
            {
                creatingList = false;
            }
            else if (availableEnemies == 1)
            {
                resultEnemyList.Add(m_EnemySpawns[0]);
                currentSpawnValue -= m_EnemySpawns[0].Cost;
            }
            else
            {
                int randomEnemyIndex = Random.Range(0, availableEnemies);
                resultEnemyList.Add(m_EnemySpawns[randomEnemyIndex]);
                currentSpawnValue -= m_EnemySpawns[randomEnemyIndex].Cost;
            }
        }

        return resultEnemyList;
    }

    public void OnEnemyDeath()
    {
        m_SpawnManager.OnEnemyDeath();
    }
}
