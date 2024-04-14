using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplodeOnDeath : EventModifierBase
{
    [SerializeField] private LayerMask m_EnemyLayerMask;
    [SerializeField] private GameObject m_ExplosionEffect;
    [SerializeField] private float m_ExplosionRadius = 2;
    [SerializeField] private int m_Damage = 20;

    protected override void OnEnemyDeathEvent(EventModifierData InData)
    {
        Modifiers modifiers = ModifierController.Instance.GetModifiers();

        if (m_ExplosionEffect != null)
        {
            Instantiate(m_ExplosionEffect, InData.Enemy.transform.position, Quaternion.identity);
            SoundManager.Instance.PlayGeneralSound(SFXType.MortarExplode, false);
        }

        ProjectileManager.Instance.TryDoExplosionDamage(InData.Enemy.transform.position, m_ExplosionRadius * modifiers.Mod_AreaOfEffect, Mathf.FloorToInt(m_Damage * modifiers.Mod_Damage), m_EnemyLayerMask);
    }
}
