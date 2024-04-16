using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleProjectileModifier : ModifierBase
{
    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_ProjectileAmount *= 2;
        InModifiers.Mod_Spread *= 1.3f;
    }
}
