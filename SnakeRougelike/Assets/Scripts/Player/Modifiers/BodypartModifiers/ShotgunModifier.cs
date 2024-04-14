using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunModifier : ModifierBase
{
    [SerializeField] private float m_ModifierAmount = 0.1f;

    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_Damage += m_ModifierAmount;
    }

    public override string GetModifierDescription()
    {
        string description = "<color=#78E08F>+" + m_ModifierAmount * 100 + "%</color=> damage to all weapons";
        return description;
    }
}
