using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerupVisualierController : MonoBehaviour
{
    [SerializeField] private GameObject m_Visualiser;

    private Image m_GridImage;
    

    private void Start()
    {
        m_GridImage = gameObject.GetComponent<Image>();
        m_GridImage.enabled = false;
    }

    public void AddNewVisualiser(ModifierBase InModifier)
    {
        m_GridImage.enabled = true;

        GameObject newVisualiser = Instantiate(m_Visualiser, this.transform);

        if (newVisualiser.TryGetComponent<UIPowerupVisualiser>(out UIPowerupVisualiser visualiser))
        {
            visualiser.SetupVisualiser(InModifier);
        }
    }
}
