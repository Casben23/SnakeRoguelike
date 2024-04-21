using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image m_FillImage;

    private HealthController m_CurrentHealthController;

    public void SetupHealthBar(HealthController InHealthController)
    {
        m_CurrentHealthController = InHealthController;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentHealthController == null)
        {
            Destroy(gameObject);
            return;
        }

        if (m_CurrentHealthController.IsDead())
        {
            m_FillImage.enabled = false;
            return;
        }

        int maxHealth = m_CurrentHealthController.GetMaxHealth();
        int currentHealth = m_CurrentHealthController.GetCurrentHealth();

        if (currentHealth == maxHealth)
            m_FillImage.enabled = false;
        else
            m_FillImage.enabled = true;

        transform.position = m_CurrentHealthController.gameObject.transform.position + transform.up;

        float healthPercent = (float)currentHealth / maxHealth;
        m_FillImage.fillAmount = healthPercent;
    }
}
