using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySpawn
{
    public GameObject Enemy;
    public int Cost;
    public int SpawnAfterLevel;
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
        m_CurrentSpawnValue = 10 * Mathf.RoundToInt(currentLevel * 0.6f);

        int waveAmount = Mathf.RoundToInt(currentLevel * 0.4f);
        m_WaveAmount = Mathf.Clamp(waveAmount, 1, 7);
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
            GameManager gameManager = GameManager.Instance;

            int cashToGain = Mathf.RoundToInt(10 * (gameManager.GetCurrentLevel() * 0.3f));
            if (m_CurrentWave >= m_WaveAmount)
            {
                if (!GameManager.Instance.IsWaitingForNewLevel())
                {
                    gameManager.AddMoney(cashToGain);
                    GameManager.Instance.OnLevelComplete();
                }
                return;
            }

           
            gameManager.AddMoney(cashToGain);
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
        List<EnemySpawn> tempList = m_EnemySpawns.Where(spawn => spawn.SpawnAfterLevel <= GameManager.Instance.GetCurrentLevel()).ToList();

        int currentSpawnValue = m_CurrentSpawnValue;
        bool creatingList = true;
        while (creatingList)
        {
            int availableEnemies = 0;
            foreach (EnemySpawn enemy in tempList)
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
                resultEnemyList.Add(tempList[0]);
                currentSpawnValue -= tempList[0].Cost;
            }
            else
            {
                int randomEnemyIndex = Random.Range(0, availableEnemies);
                resultEnemyList.Add(tempList[randomEnemyIndex]);
                currentSpawnValue -= tempList[randomEnemyIndex].Cost;
            }
        }

        return resultEnemyList;
    }

    public void OnEnemyDeath()
    {
        m_SpawnManager.OnEnemyDeath();
    }
}
