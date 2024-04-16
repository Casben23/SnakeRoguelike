using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperModifer : ModifierBase
{
    [SerializeField] private int m_ModifierAmount = 1;

    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_PierceAmount += m_ModifierAmount;
    }

    public override string GetModifierDescription()
    {
        string description = "<color=#78E08F>+" + m_ModifierAmount + "</color=> pierce to all weapons";
        return description;
    }
}
