using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLayerBase : MonoBehaviour
{
    [SerializeField] private EMainMenuLayer m_Layer;

    public EMainMenuLayer GetLayer()
    {
        return m_Layer;
    }

    public virtual void OnShow()
    {
        
    }
}
