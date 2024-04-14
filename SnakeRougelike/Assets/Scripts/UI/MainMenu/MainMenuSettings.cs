using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettings : MainMenuLayerBase
{
    [SerializeField] private Slider m_MusicSlider;
    [SerializeField] private Slider m_SFXSlider;

    public override void OnShow()
    {
        base.OnShow();

        if (m_MusicSlider == null || m_SFXSlider == null)
            return;

        m_MusicSlider.value = GameState.Instance.MusicMultiplier;
        m_SFXSlider.value = GameState.Instance.SFXMultiplier;
    }
}
