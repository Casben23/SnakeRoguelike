using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject m_UIObjects;

    private bool m_PauseMenuOpen = false;

    void Start()
    {
        m_UIObjects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseScreen(m_PauseMenuOpen);
        }
    }

    public void SetPauseScreen(bool InShouldClose)
    {
        if (InShouldClose)
        {
            Time.timeScale = 1;
            m_UIObjects.SetActive(false);
            m_PauseMenuOpen = !m_PauseMenuOpen;
        }
        else
        {
            Time.timeScale = 0;
            m_UIObjects.SetActive(true);
            m_PauseMenuOpen = !m_PauseMenuOpen;
        }
    }
}
