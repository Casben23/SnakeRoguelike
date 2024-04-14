using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private GameObject m_HitEffect;

    protected SnakeBodyPartBase m_Instigator;
    protected WeaponPartStats m_InstigatorStats;

    private Vector2 m_StartPosition;
    private Vector2 m_FireDirection;

    // Start is called before the first frame update
    void Start()
    {
        m_StartPosition = transform.position;
    }

    public void SetFireDirection(Vector2 InFireDirection)
    {
        float randomAngleOffset = Random.Range(-45f, 45f); // Adjust the range as needed
        randomAngleOffset *= m_InstigatorStats.Spread;
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngleOffset);
        Vector2 fireDirectionWithSpread = randomRotation * InFireDirection;

        m_FireDirection = fireDirectionWithSpread;

        Quaternion fireDirectionRotation = Quaternion.LookRotation(Vector3.forward, m_FireDirection);

        transform.rotation = fireDirectionRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)m_FireDirection * m_InstigatorStats.ProjectileSpeed * Time.deltaTime;

        if ((transform.position - (Vector3)m_StartPosition).magnitude >= m_InstigatorStats.Range)
        {
            RemoveProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            EventModifierData eventData = new EventModifierData();
            eventData.Projectile = this;
            eventData.Enemy = enemy;
            eventData.BodyPart = m_Instigator;

            EventManager.Instance.ProjectileHitEnemy(eventData);

            enemy.OnTakeDamage(m_InstigatorStats.Damage);

            if (m_InstigatorStats.PierceAmount <= 0)
            {
                RemoveProjectile();
                return;
            }


            m_InstigatorStats.PierceAmount -= 1;
        }
    }

    private void RemoveProjectile()
    {
        if(m_HitEffect != null)
        {
            SpawnHitEffect();
        }

        Destroy(gameObject);
    }

    private void SpawnHitEffect()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.up);

        Instantiate(m_HitEffect, transform.position, rotation);
    }

    public void SetInstigator(SnakeBodyPartBase InInstigator)
    {
        m_Instigator = InInstigator;

        m_InstigatorStats = (WeaponPartStats)InInstigator.GetWeaponPartModifiedStats().Clone();
    }
}
