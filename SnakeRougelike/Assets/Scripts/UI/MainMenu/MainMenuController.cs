using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EMainMenuLayer
{
    TitleScreen = 0,
    MainMenu = 1,
    StatScreen = 2,
    Settings = 3
}

public class MainMenuController : MonoBehaviour
{
    private EMainMenuLayer m_CurrentLayer = EMainMenuLayer.TitleScreen;
    [SerializeField] private List<MainMenuLayerBase> m_Layers = new List<MainMenuLayerBase>();
    [SerializeField] private UITransitionController m_TransitionController;

    private void Start()
    {
        SetCurrentLayer((int)EMainMenuLayer.TitleScreen);
        m_TransitionController.StartSequence(null, false, true);
    }

    public void SetCurrentLayer(int InLayer)
    {
        m_CurrentLayer = (EMainMenuLayer)InLayer;

        foreach (MainMenuLayerBase layer in m_Layers)
        {
            if (layer.GetLayer() == (EMainMenuLayer)InLayer)
            {
                layer.gameObject.SetActive(true);
                layer.OnShow();
            }
            else
            {
                layer.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCurrentLayer((int)EMainMenuLayer.MainMenu);
        }
    }

    public void OnPlay()
    {
        m_TransitionController.StartSequence(LoadNextLevel, false, false);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameState.Instance.SetGameState(EGameState.InGame);
    }
}
