using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected SnakeBodyPartBase m_Instigator;
    protected WeaponPartStats m_InstigatorStats;
    private Vector2 m_FireDirection;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetFireDirection(Vector2 InFireDirection)
    {
        m_FireDirection = InFireDirection;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)m_FireDirection * m_InstigatorStats.ProjectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            enemy.OnTakeDamage(m_InstigatorStats.Damage);

            if (m_InstigatorStats.PierceAmount <= 0)
            {
                Destroy(gameObject);
                return;
            }

            m_InstigatorStats.PierceAmount -= 1;
        }
    }

    public void SetInstigator(SnakeBodyPartBase InInstigator)
    {
        m_Instigator = InInstigator;

        m_InstigatorStats = (WeaponPartStats)InInstigator.GetWeaponPartStats().Clone();
    }
}
