using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashOnEnemyTakeDamage : EventModifierBase
{
    protected override void OnEnemyTakeDamageEvent(EventModifierData InData)
    {
        int randomChance = Random.Range(1, 101);

        float chanceModifier = ModifierController.Instance.GetModifiers().Mod_ChanceModifier;

        if (randomChance * chanceModifier > 10)
            return;

        GameManager.Instance.AddMoney(1);
    }
}
