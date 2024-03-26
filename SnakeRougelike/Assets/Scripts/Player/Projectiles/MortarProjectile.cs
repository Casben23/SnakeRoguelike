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

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_InstigatorStats.AreaOfEffect, m_EnemyLayerMask);

        if (colliders.Length <= 0)
        {
            Destroy(gameObject);
            return;
        }

        foreach(Collider2D coll in colliders)
        {
            if(coll.TryGetComponent<EnemyBase>(out EnemyBase enemy))
            {
                enemy.OnTakeDamage(m_InstigatorStats.Damage);
            }
        }

        Destroy(gameObject);
    }
}
