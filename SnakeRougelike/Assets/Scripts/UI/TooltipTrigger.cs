using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string m_Name;
    [SerializeField] private string m_Description;

    public void SetText(string InName, string InDescription)
    {
        m_Name = InName;
        m_Description = InDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipController.Show(m_Name, m_Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Hide();
    }
}
