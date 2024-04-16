using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILockButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite m_LockedSprite;
    [SerializeField] private Sprite m_UnlockedSprite;

    private Image m_LockImage;

    private bool m_IsLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        m_LockImage = gameObject.GetComponent<Image>();
    }

    public void ToggleLock()
    {
        m_IsLocked = !m_IsLocked;

        if (m_IsLocked)
        {
            m_LockImage.sprite = m_LockedSprite;
        }
        else
        {
            m_LockImage.sprite = m_UnlockedSprite;
        }

        UINextLevelScreen.Instance.SetPuchasePannelLock(m_IsLocked);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UINextLevelScreen.Instance.GetToolTip().ShowCustom("Lock", "Preserve shop inventory between levels", "");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UINextLevelScreen.Instance.GetToolTip().Hide();
    }
}
