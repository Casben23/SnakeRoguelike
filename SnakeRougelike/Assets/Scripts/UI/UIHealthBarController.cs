using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarController : MonoBehaviour
{
    private static UIHealthBarController instance;

    public static UIHealthBarController Instance
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

    [SerializeField] private GameObject m_HealthBarPrefab;

    public void CreateNewHealthBar(HealthController m_HealthController)
    {
        GameObject newHealthBarObj = Instantiate(m_HealthBarPrefab, transform);
        if(newHealthBarObj.TryGetComponent<UIHealthBar>(out UIHealthBar healthBar))
        {
            healthBar.SetupHealthBar(m_HealthController);
        }
    }
}
