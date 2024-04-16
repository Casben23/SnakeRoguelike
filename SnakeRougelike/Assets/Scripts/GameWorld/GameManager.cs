using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
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

    [SerializeField] private UINextLevelScreen m_NextLevelScreen;
    [SerializeField] private UITransitionController m_TransitionController;
    [SerializeField] private UIDeathScreen m_DeathScreen;
    [SerializeField] private UIPowerUpScreen m_PowerUpScreen;

    [SerializeField] private GameObject m_BackgroundMusicObject;
    [SerializeField] private GameObject m_MoneyText;
    [SerializeField] private GameObject m_DamageText;

    [SerializeField] private int m_ModifierEveryLevel = 3;

    private GameObject m_Player;
    private int m_CurrentMoney = 0;
    private int m_CurrentLevel = 0;

    private bool m_IsWaitingForNewLevel = false;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_CurrentMoney = 0;
        m_MoneyText.GetComponent<TextMeshProUGUI>().text = m_CurrentMoney.ToString() + "$";

        m_PowerUpScreen.gameObject.SetActive(false);
        m_TransitionController.StartSequence(CloseUpgradeScreen, 1f, false, true);
    }

    public GameObject GetPlayer()
    {
        if (m_Player == null)
            return null;

        return m_Player;
    }

    public void AddMoney(int InAmount)
    {
        m_CurrentMoney += InAmount;
        GameStatisticsManager.Instance.GetGameStats().CashGainedThisLevel += InAmount;
        m_MoneyText.GetComponent<TextMeshProUGUI>().text = m_CurrentMoney.ToString() + "$";
    }

    public void RemoveMoney(int InAmount)
    {
        m_CurrentMoney -= InAmount;
        m_MoneyText.GetComponent<TextMeshProUGUI>().text = m_CurrentMoney.ToString() + "$";
    }

    public int GetCurrentMoney()
    {
        return m_CurrentMoney;
    }

    public void SpawnDamageText(Vector2 InPosition, int InDamage)
    {
        if (m_DamageText == null)
            return;

        GameObject newDamageText = Instantiate(m_DamageText, InPosition, Quaternion.identity);
        if(newDamageText.TryGetComponent<DamageTextController>(out DamageTextController damageTextController))
        {
            damageTextController.SetupText(InDamage);
        }
    }

    public int GetCurrentLevel()
    {
        return m_CurrentLevel;
    }

    public void OnLevelComplete()
    {
        if (m_Player == null)
            return;

        if (m_Player.GetComponent<SnakeBodyController>().IsDead())
            return;

        Time.timeScale = 0;

        if (m_CurrentLevel % m_ModifierEveryLevel == 0)
        {
            m_PowerUpScreen.gameObject.SetActive(true);
            m_PowerUpScreen.ShowPowerUpScreenSequence();
        }
        else
        {
            m_TransitionController.StartSequence(ShowUpgradeScreen, 0.7f, true, false);
        }
        m_IsWaitingForNewLevel = true;
    }

    public void OnPowerUpChosen()
    {
        m_PowerUpScreen.HideButtons();
        m_TransitionController.StartSequence(ShowUpgradeScreen, 0.7f, true, false);
    }

    public void StartTransition(System.Action InOnCompleteAction, bool InShowStats, bool m_StartOpen)
    {
        m_TransitionController.StartSequence(InOnCompleteAction, 0.7f, InShowStats, m_StartOpen);
    }

    public void GameOver()
    {
        GameState.Instance.SaveGameState();
        m_DeathScreen.ShowDeathScreen();
    }

    public void MuffleSound()
    {
        m_BackgroundMusicObject.GetComponent<AudioLowPassFilter>().enabled = true;
    }

    private void ShowUpgradeScreen()
    {
        m_PowerUpScreen.gameObject.SetActive(false);
        m_BackgroundMusicObject.GetComponent<AudioLowPassFilter>().enabled = true;
        m_NextLevelScreen.OpenNextLevelScreen();
    }

    public bool IsUpgradeScreenOpen()
    {
        return m_NextLevelScreen.IsUpgradeScreenOpen();
    }

    public void StartNextLevel()
    {
        m_TransitionController.StartSequence(CloseUpgradeScreen, 1f, false, false);
    }

    private void CloseUpgradeScreen()
    {
        Time.timeScale = 1;
        m_CurrentLevel += 1;

        if(GameState.Instance.HighestLevelReached < m_CurrentLevel)
        {
            GameState.Instance.SetHighestLevelReached(m_CurrentLevel);
        }

        GameStats gameStats = GameStatisticsManager.Instance.GetGameStats();
        gameStats.EnemiesKilledThisLevel = 0;
        gameStats.CashGainedThisLevel = 0;
        gameStats.PartsLostThisLevel = 0;
        gameStats.LevelReached = m_CurrentLevel;

        m_BackgroundMusicObject.GetComponent<AudioLowPassFilter>().enabled = false;
        m_NextLevelScreen.CloseNextLevelScreen();
        WaveController.Instance.OnLevelStart();

        m_IsWaitingForNewLevel = false;
    }

    public bool IsWaitingForNewLevel()
    {
        return m_IsWaitingForNewLevel;
    }

    public UINextLevelScreen GetUpgradeScreen()
    {
        return m_NextLevelScreen;
    }

    public void QuitPlayMode()
    {
        GameManager.Instance.StartTransition(LoadMainMenu, false, false);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        GameState.Instance.SetGameState(EGameState.MainMenu);
        Time.timeScale = 1;
    }

}
