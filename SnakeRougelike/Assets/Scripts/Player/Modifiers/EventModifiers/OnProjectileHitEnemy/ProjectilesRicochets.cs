using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectilesRicochets : EventModifierBase
{
    [SerializeField] LayerMask m_EnemyLayer;
    [SerializeField] float m_NearbyEnemyRadius = 4;

    protected override void OnProjectileHitEvent(EventModifierData InData)
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(InData.Enemy.transform.position, m_NearbyEnemyRadius, m_EnemyLayer).ToList<Collider2D>();

        foreach(Collider2D col in colliders)
        {
            if (col.gameObject == InData.Enemy.gameObject)
            {
                colliders.Remove(col);
                break;
            }
        }


        if (colliders.Count <= 0)
            return;

        if (colliders.Count == 1)
        {
            GameObject target = colliders[0].gameObject;

            Vector2 fireDirection = (target.transform.position - InData.Enemy.transform.position).normalized;
            InData.Projectile.SetFireDirection(fireDirection);
            return;
        }

        colliders.Sort((a, b) =>
        {
            float distanceToA = Vector2.Distance(InData.Enemy.transform.position, a.transform.position);
            float distanceToB = Vector2.Distance(InData.Enemy.transform.position, b.transform.position);
            return distanceToA.CompareTo(distanceToB);
        });

        GameObject closestTarget = colliders[0].gameObject;

        Vector2 fireDirectionToCloseTarget = (closestTarget.transform.position - InData.Enemy.transform.position).normalized;
        InData.Projectile.SetFireDirection(fireDirectionToCloseTarget);



    }
}
