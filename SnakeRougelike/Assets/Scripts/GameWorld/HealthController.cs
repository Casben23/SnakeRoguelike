using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private int m_MaxHealth = 1;
    private int m_CurrentHealth = 1;

    public void SetupHealthController(int InMaxHealth)
    {
        m_MaxHealth = InMaxHealth;
        m_CurrentHealth = m_MaxHealth;
    }

    public void Heal(int InHealthAmount)
    {
        m_CurrentHealth += InHealthAmount;
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
    }

    public void FullyHeal()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void TakeDamage(int InDamage)
    {
        m_CurrentHealth -= InDamage;
    }

    public int GetCurrentHealth()
    {
        return m_CurrentHealth;
    }

    public int GetMaxHealth()
    {
        return m_MaxHealth;
    }

    public bool IsDead()
    {
        return m_CurrentHealth <= 0;
    }
}
