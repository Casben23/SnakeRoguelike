using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    private static TooltipController Instance;

    public PartToolTip m_Tooltip;

    public void Awake()
    {
        Instance = this;
    }

    public static void Show(string InName, string InDescription)
    {
        Instance.m_Tooltip.SetText(InName, InDescription);
        Instance.m_Tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        Instance.m_Tooltip.gameObject.SetActive(false);
    }
}
