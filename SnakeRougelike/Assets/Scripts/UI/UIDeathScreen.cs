using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_Background;

    [SerializeField] private GameObject m_GameoverText;
    [SerializeField] private GameObject m_TryAgainButton;
    [SerializeField] private GameObject m_QuitButton;

    [SerializeField] private TextMeshProUGUI m_StatText;

    // Start is called before the first frame update
    void Start()
    {
        Image imageComp = m_Background.GetComponent<Image>();

        imageComp.color = new Color(imageComp.color.r, imageComp.color.g, imageComp.color.b, 0);
        m_Background.SetActive(false);

        m_GameoverText.SetActive(false);
        m_TryAgainButton.SetActive(false);
        m_QuitButton.SetActive(false);

        m_StatText.gameObject.SetActive(false);

        m_GameoverText.gameObject.transform.localScale = new Vector3(0,0,1);
        m_TryAgainButton.gameObject.transform.localScale = new Vector3(0, 0, 1);
        m_QuitButton.gameObject.transform.localScale = new Vector3(0, 0, 1);

        m_StatText.gameObject.transform.localScale = new Vector3(0, 0, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowDeathScreen();
        }
    }

    public void ShowDeathScreen()
    {
        m_Background.SetActive(true);

        m_GameoverText.SetActive(true);
        m_TryAgainButton.SetActive(true);
        m_QuitButton.SetActive(true);

        m_StatText.gameObject.SetActive(true);

        GameStats gameStats = GameStatisticsManager.Instance.GetGameStats();

        m_StatText.text = "level reached: [" + gameStats.LevelReached + "/20]\nCritters Killed: [" + gameStats.EnemiesKilled.ToString() + "]\n" + "Damage Dealt: [" + gameStats.DamageDealt + "]";

        StartCoroutine(StatSequence());
    }

    IEnumerator StatSequence()
    {
        LeanTween.value(gameObject, 0, 0.8f, 1f).setOnUpdate(UpdateAlpha);

        yield return new WaitForSeconds(1.5f);

        LeanTween.scale(m_GameoverText.gameObject, new Vector3(1, 1, 1), 1.5f).setEaseOutElastic();
        SoundManager.Instance.PlayGeneralSound(SFXType.ShowStatText, true);

        yield return new WaitForSeconds(1f);

        LeanTween.scale(m_TryAgainButton.gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic().setIgnoreTimeScale(true);
        SoundManager.Instance.PlayGeneralSound(SFXType.ShowStatText, true);

        yield return new WaitForSeconds(0.5f);

        LeanTween.scale(m_QuitButton.gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic().setIgnoreTimeScale(true);
        SoundManager.Instance.PlayGeneralSound(SFXType.ShowStatText, true);

        yield return new WaitForSeconds(1f);

        LeanTween.scale(m_StatText.gameObject, new Vector3(1, 1, 1), 1f).setEaseOutElastic().setIgnoreTimeScale(true);
        SoundManager.Instance.PlayGeneralSound(SFXType.ShowStatText, true);

    }

    private void UpdateAlpha(float InValue)
    {
        Image imageComp = m_Background.GetComponent<Image>();
        imageComp.color = new Color(imageComp.color.r, imageComp.color.g, imageComp.color.b, InValue);
    }

    public void OnTryAgainButton()
    {
        GameManager.Instance.StartTransition(ReloadScene, false, false);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitButton()
    {
        
    }
}
