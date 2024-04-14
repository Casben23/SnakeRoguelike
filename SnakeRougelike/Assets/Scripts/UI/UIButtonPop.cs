using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonPop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private SFXType OnClickSound;
    [SerializeField] private bool m_CanClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!m_CanClick)
            return;

        LeanTween.cancel(gameObject);

        transform.localScale = new Vector3(1, 1, 1);

        SoundManager.Instance.PlayGeneralSound(OnClickSound, false);
        LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.4f).setEaseOutElastic().setIgnoreTimeScale(true);
    }

    virtual public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayGeneralSound(SFXType.HighlightPart, true);
        LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.4f).setEaseOutElastic().setIgnoreTimeScale(true);
    }

    virtual public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnDisable()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(1, 1, 1);
    }
}
