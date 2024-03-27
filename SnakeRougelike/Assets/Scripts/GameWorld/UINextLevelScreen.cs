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
        m_Background.SetActive(true);
        gameObject.transform.position = new Vector3(0, 0, gameObject.transform.position.z);
        m_UpgradeScreenOpen = true;
        UpdateSnakeVisualiser();
    }
    
    public void CloseNextLevelScreen()
    {
        m_Background.SetActive(false);
        gameObject.transform.position = new Vector3(10000, 10000, gameObject.transform.position.z);
        m_UpgradeScreenOpen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseNextLevelScreen();
    }
}
