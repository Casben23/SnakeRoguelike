using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeIncreaseProjectileOnHit : EventModifierBase
{
    [SerializeField] float m_SizeMultiplier = 3;

    protected override void OnProjectileHitEvent(EventModifierData InData)
    {
        if (InData.Projectile == null)
            return;

        Vector3 currentScale = InData.Projectile.transform.localScale;

        LeanTween.scale(InData.Projectile.gameObject, new Vector3(currentScale.x * m_SizeMultiplier, currentScale.y * m_SizeMultiplier, 1), 0.4f).setEaseOutElastic();
    }
}
