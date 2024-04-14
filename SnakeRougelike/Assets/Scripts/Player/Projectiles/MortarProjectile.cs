using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectile : ProjectileBase
{
    [SerializeField] private GameObject m_ExplosionEffect;
    [SerializeField] private LayerMask m_EnemyLayerMask;

    private void Start()
    {
        SoundManager.Instance.PlayGeneralSound(SFXType.MortarFly, true);
    }

    public void Explode()
    {
        if(m_ExplosionEffect != null)
        {
            Instantiate(m_ExplosionEffect, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayGeneralSound(SFXType.MortarExplode, false);
        }

        ProjectileManager.Instance.TryDoExplosionDamage(transform.position, m_InstigatorStats.AreaOfEffect, m_InstigatorStats.Damage, m_EnemyLayerMask);

        Destroy(gameObject);
    }
}
