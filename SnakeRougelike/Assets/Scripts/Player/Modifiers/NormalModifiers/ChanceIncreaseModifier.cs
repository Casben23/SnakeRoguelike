using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceIncreaseModifier : ModifierBase
{
    public override void ApplyModifier(Modifiers InModifiers)
    {
        InModifiers.Mod_ChanceModifier *= 2;
    }
}
