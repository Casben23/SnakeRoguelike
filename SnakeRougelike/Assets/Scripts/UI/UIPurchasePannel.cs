using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPurchasePannel : MonoBehaviour
{
    [SerializeField] List<UIPurchasePart> m_PurchasePartButtons;

    public void RerollButtons()
    {
        foreach(UIPurchasePart purchaseButton in m_PurchasePartButtons)
        {
            purchaseButton.SetNewPurchaseOption();
        }
    }

    public void UpdateStarVisualizers()
    {
        foreach(UIPurchasePart purchaseButton in m_PurchasePartButtons)
        {
            purchaseButton.UpdateStar();
        }
    }
}
