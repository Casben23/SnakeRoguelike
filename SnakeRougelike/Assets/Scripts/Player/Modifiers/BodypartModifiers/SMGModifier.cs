using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGModifier : ModifierBase
{
    [SerializeField] private float m_ModifierAmount = 0.05f;

    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_ActionCooldown += m_ModifierAmount;
    }

    public override string GetModifierDescription()
    {
        string description = "<color=#78E08F>-" + m_ModifierAmount * 100 + "%</color=> action cooldown to all weapons";
        return description;
    }
}
