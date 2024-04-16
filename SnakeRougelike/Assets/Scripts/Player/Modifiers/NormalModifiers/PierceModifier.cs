using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceModifier : ModifierBase
{
    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_PierceAmount += 2;
    }
}
