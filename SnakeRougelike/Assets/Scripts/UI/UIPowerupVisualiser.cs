using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPowerupVisualiser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ModifierBase m_Modifier;
    [SerializeField] private TextMeshProUGUI m_NameText;

    public void SetupVisualiser(ModifierBase InModifier)
    {
        m_Modifier = InModifier;
        m_NameText.text = InModifier.GetModifierName();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UINextLevelScreen.Instance.GetToolTip().ShowCustom(m_Modifier.GetModifierName(), m_Modifier.GetModifierDescription(), "");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UINextLevelScreen.Instance.GetToolTip().Hide();
    }
}
