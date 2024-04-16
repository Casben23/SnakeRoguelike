using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFireRate : ModifierBase
{
    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_ActionCooldown *= 0.7f;
    }
}
