using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuStatScreen : MainMenuLayerBase
{
    [SerializeField] private TextMeshProUGUI m_StatValueText;

    public override void OnShow()
    {
        base.OnShow();

        if (m_StatValueText == null)
            return;

        //In Order
        int enemiesKilled = GameState.Instance.EnemiesKilled;
        int damageDone = GameState.Instance.DamageDone;
        int levelReached = GameState.Instance.HighestLevelReached;

        m_StatValueText.text = enemiesKilled.ToString() + "\n" + damageDone.ToString() + "\n" + levelReached.ToString();
    }
}
