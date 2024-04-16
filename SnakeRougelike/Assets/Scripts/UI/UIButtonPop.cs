using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonPop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private SFXType OnClickSound;
    [SerializeField] private bool m_CanClick;

    Vector3 m_DesiredScale;
    Vector3 m_HoverScale;

    private void Awake()
    {
        m_DesiredScale = gameObject.transform.localScale;
        m_HoverScale = m_DesiredScale * 1.1f;
    }

    private void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!m_CanClick)
            return;

        LeanTween.cancel(gameObject);

        transform.localScale = m_DesiredScale;

        SoundManager.Instance.PlayGeneralSound(OnClickSound, false);
        LeanTween.scale(gameObject, m_HoverScale, 0.4f).setEaseOutElastic().setIgnoreTimeScale(true);
    }

    virtual public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayGeneralSound(SFXType.HighlightPart, true);
        LeanTween.scale(gameObject, m_HoverScale, 0.4f).setEaseOutElastic().setIgnoreTimeScale(true);
    }

    virtual public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        transform.localScale = m_DesiredScale;
    }

    public void OnDisable()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = m_DesiredScale;
    }
}
