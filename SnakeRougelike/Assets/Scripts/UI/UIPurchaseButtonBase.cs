using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPurchaseButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected int m_Cost;
    [SerializeField] private bool m_Highlighted = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_Highlighted)
        {
            SoundManager.Instance.PlayGeneralSound(SFXType.HighlightPart, true);
            LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.4f).setEaseOutElastic().setIgnoreTimeScale(true);
            m_Highlighted = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(1, 1, 1);
        m_Highlighted = false;
    }

    public void TryPurchase()
    {
        int currentMoney = GameManager.Instance.GetCurrentMoney();
        if (currentMoney < m_Cost)
        {
            SoundManager.Instance.PlayGeneralSound(SFXType.InsuficientFunds, false);
            return;
        }

        GameManager.Instance.RemoveMoney(m_Cost);

        Purchase();
    }

    protected virtual void Purchase()
    {
        SoundManager.Instance.PlayGeneralSound(SFXType.PurchasePart, false);
    }
}
