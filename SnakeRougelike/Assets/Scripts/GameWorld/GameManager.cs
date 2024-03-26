using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField] private GameObject m_BackgroundMusicObject;
    [SerializeField] private GameObject m_MoneyText;

    private GameObject m_Player;
    private int m_CurrentMoney = 0;
    private int m_CurrentLevel = 1;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_CurrentMoney = 0;
        m_MoneyText.GetComponent<TextMeshProUGUI>().text = m_CurrentMoney.ToString() + "$";
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

    public int GetCurrentLevel()
    {
        return m_CurrentLevel;
    }

    public void OnLevelComplete()
    {
        Time.timeScale = 0;
        m_BackgroundMusicObject.GetComponent<AudioLowPassFilter>().enabled = true;
        m_NextLevelScreen.OpenNextLevelScreen();
    }

    public bool IsUpgradeScreenOpen()
    {
        return m_NextLevelScreen.IsUpgradeScreenOpen();
    }

    public void StartNextLevel()
    {
        Time.timeScale = 1;
        m_CurrentLevel += 1;

        m_BackgroundMusicObject.GetComponent<AudioLowPassFilter>().enabled = false;
        m_NextLevelScreen.CloseNextLevelScreen();
        WaveController.Instance.OnLevelStart();

        m_Player.GetComponent<SnakeHeadController>().SetEatCoolDown(2);
    }

    public UINextLevelScreen GetUpgradeScreen()
    {
        return m_NextLevelScreen;
    }
}
