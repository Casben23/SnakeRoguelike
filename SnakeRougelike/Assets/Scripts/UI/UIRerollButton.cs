using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRerollButton : UIPurchaseButtonBase
{
    [SerializeField] private UIPurchasePannel m_PurchasePannel;
    [SerializeField] private UIPartInfo m_ToolTip;

    protected override void Purchase()
    {
        base.Purchase();

        m_PurchasePannel.RerollButtons();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        m_ToolTip.ShowCustom("Reroll", "Rerolls all parts", "");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        m_ToolTip.Hide();
    }
}
