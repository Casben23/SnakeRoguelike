using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedEnemy : EnemyBase
{
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private float m_MaxTimeBtwShoots = 5;
    [SerializeField] private float m_MinTimeBtwShoots = 3;

    private float m_TimeBtwShoots = 4f;

    void Update()
    {
        if(m_TimeBtwShoots <= 0)
        {
            FireProjectile();
            m_TimeBtwShoots = Random.Range(m_MinTimeBtwShoots, m_MaxTimeBtwShoots);
            return;
        }

        m_TimeBtwShoots -= Time.deltaTime;
    }

    private void FireProjectile()
    {
        GameObject player = GameManager.Instance.GetPlayer();

        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);

            Instantiate(m_Projectile, transform.position, rotation);
        }
    }
}
