using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UITransitionController : MonoBehaviour
{
    [SerializeField] private GameObject m_TransitionImage;
    [SerializeField] private GameObject m_LevelCompleteText;
    [SerializeField] private TextMeshProUGUI m_CashGainedText;
    [SerializeField] private TextMeshProUGUI m_EnemiesKilledText;
    [SerializeField] private TextMeshProUGUI m_BodyPartsLostText;

    System.Action m_CurrentActionOnComplete;

    private void Start()
    {
        m_LevelCompleteText.gameObject.SetActive(false);
        m_CashGainedText.gameObject.SetActive(false);
        m_EnemiesKilledText.gameObject.SetActive(false);
        m_BodyPartsLostText.gameObject.SetActive(false);

        m_LevelCompleteText.transform.localScale = new Vector3(0,0,1);
        m_CashGainedText.gameObject.transform.localScale = new Vector3(0,0,1);
        m_EnemiesKilledText.gameObject.transform.localScale = new Vector3(0, 0, 1);
        m_BodyPartsLostText.gameObject.transform.localScale = new Vector3(0, 0, 1);
    }

    public void StartSequence(System.Action InOnCompleteAction, float InTransitionTime, bool InShowStats, bool m_StartOpen)
    {
        m_CurrentActionOnComplete = InOnCompleteAction;

        if (m_StartOpen)
        {
            m_TransitionImage.transform.localScale = new Vector3(50, 50, 1);
            HideTransition();
            return;
        }


        if (!InShowStats)
        {
            m_TransitionImage.transform.localScale = new Vector3(0, 0, 1);
            LeanTween.scale(m_TransitionImage, new Vector3(50, 50, 1), InTransitionTime).setIgnoreTimeScale(true).setOnComplete(HideTransition);
            return;
        }

        LeanTween.scale(m_TransitionImage, new Vector3(50, 50, 1), InTransitionTime).setIgnoreTimeScale(true).setOnComplete(ShowStats);
    }

    void ShowStats()
    {
        m_LevelCompleteText.gameObject.SetActive(true);
        m_CashGainedText.gameObject.SetActive(true);
        m_EnemiesKilledText.gameObject.SetActive(true);
        m_BodyPartsLostText.gameObject.SetActive(true);
        
        GameStats gameStats = GameStatisticsManager.Instance.GetGameStats();
        TextMeshProUGUI textMesh = m_LevelCompleteText.GetComponent<TextMeshProUGUI>();
        textMesh.text = "Level " + gameStats.LevelReached + " complete";
        m_CashGainedText.text = "Cash Gained: [<color=#78E08F>" + gameStats.CashGainedThisLevel.ToString() + "</color=>]";
        m_EnemiesKilledText.text = "Enemies Killed: [<color=#78E08F>" + gameStats.EnemiesKilledThisLevel.ToString() + "</color=>]";
        m_BodyPartsLostText.text = "Parts lost: [<color=#78E08F>" + gameStats.PartsLostThisLevel.ToString() + "</color=>]";
        StartCoroutine(StatSequence());
    }

    IEnumerator StatSequence()
    {
        LTSeq openSequence = LeanTween.sequence();

        LeanTween.scale(m_LevelCompleteText, new Vector3(1, 1, 1), 1).setEaseOutElastic().setIgnoreTimeScale(true);
        SoundManager.Instance.PlayGeneralSound(SFXType.LevelUp, false);

        yield return CoroutineUtil.WaitForRealSeconds(1f);

        LeanTween.scale(m_CashGainedText.gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic().setIgnoreTimeScale(true);
        SoundManager.Instance.PlayGeneralSound(SFXType.ShowStatText, false);

        yield return CoroutineUtil.WaitForRealSeconds(0.2f);

        LeanTween.scale(m_EnemiesKilledText.gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic().setIgnoreTimeScale(true);
        SoundManager.Instance.PlayGeneralSound(SFXType.ShowStatText, false);

        yield return CoroutineUtil.WaitForRealSeconds(0.2f);

        LeanTween.scale(m_BodyPartsLostText.gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic().setIgnoreTimeScale(true);
        SoundManager.Instance.PlayGeneralSound(SFXType.ShowStatText, false);

        yield return CoroutineUtil.WaitForRealSeconds(1f);

        LeanTween.scale(m_LevelCompleteText, new Vector3(0, 0, 1), 0.2f).setEaseInCubic().setIgnoreTimeScale(true);
        LeanTween.scale(m_CashGainedText.gameObject, new Vector3(0, 0, 1), 0.2f).setEaseInCubic().setIgnoreTimeScale(true);
        LeanTween.scale(m_EnemiesKilledText.gameObject, new Vector3(0, 0, 1), 0.2f).setEaseInCubic().setIgnoreTimeScale(true);
        LeanTween.scale(m_BodyPartsLostText.gameObject, new Vector3(0, 0, 1), 0.2f).setEaseInCubic().setIgnoreTimeScale(true);

        HideTransition();
    }

    public void HideTransition()
    {
        if(m_CurrentActionOnComplete != null)
            m_CurrentActionOnComplete();

        LeanTween.scale(m_TransitionImage, new Vector3(0, 0, 1), 1.5f).setIgnoreTimeScale(true).setOnComplete(DisableText);
    }

    private void DisableText()
    {
        m_LevelCompleteText.gameObject.SetActive(false);
        m_CashGainedText.gameObject.SetActive(false);
        m_EnemiesKilledText.gameObject.SetActive(false);
        m_BodyPartsLostText.gameObject.SetActive(false);
    }
}
public static class CoroutineUtil
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}
