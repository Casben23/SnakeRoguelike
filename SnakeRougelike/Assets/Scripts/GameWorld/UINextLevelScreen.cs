using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINextLevelScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_Background;
    [SerializeField] private UISnakeVisualiserController m_SnakeVisualiser;

    private bool m_UpgradeScreenOpen = false;

    public bool IsUpgradeScreenOpen()
    {
        return m_UpgradeScreenOpen;
    }

    public void UpdateSnakeVisualiser()
    {
        m_SnakeVisualiser.UpdateSnakeVisualiser();
    }

    public void OpenNextLevelScreen()
    {
        gameObject.transform.position = new Vector3(0, 0, gameObject.transform.position.z);
        m_UpgradeScreenOpen = true;
        UpdateSnakeVisualiser();
        OpenUpgradeScreenSequence();
    }

    void OpenUpgradeScreenSequence()
    {
        LeanTween.scale(m_Background, new Vector3(100, 100, 1), 1.5f).setIgnoreTimeScale(true);
    }

    public void CloseNextLevelScreen()
    {
        LeanTween.scale(m_Background, new Vector3(0, 0, 1), 1.5f).setOnComplete(() => { gameObject.transform.position = new Vector3(100000, 0, gameObject.transform.position.z); }).setIgnoreTimeScale(true);
        m_UpgradeScreenOpen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseNextLevelScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
