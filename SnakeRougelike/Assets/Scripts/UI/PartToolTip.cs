using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_DescriptionText;

    public void SetText(string InName, string InDescription)
    {
        if (string.IsNullOrEmpty(InName))
        {
            m_NameText.gameObject.SetActive(false);
        }

        if (string.IsNullOrEmpty(InDescription))
        {
            m_DescriptionText.gameObject.SetActive(false);
        }

        if (m_NameText != null)
        {
            m_NameText.text = InName;
        }

        if(m_DescriptionText != null)
        {
            m_DescriptionText.text = InDescription;
        }
    }

}
