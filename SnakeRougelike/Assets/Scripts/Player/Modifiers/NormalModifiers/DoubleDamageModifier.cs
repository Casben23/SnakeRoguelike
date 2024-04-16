using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamageModifier : ModifierBase
{
    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_Damage *= 2;
        InModifiers.Mod_MaxHealth *= 0.5f;
    }
}
