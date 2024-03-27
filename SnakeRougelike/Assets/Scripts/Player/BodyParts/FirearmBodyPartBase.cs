using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETargetType
{
    Random,
    Furthest,
    Closest
}

public class FirearmBodyPartBase : SnakeBodyPartBase
{
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private SFXType m_ActionSound;
    [SerializeField] private ETargetType m_TargetType;

    protected override void PerformAction()
    {
        base.PerformAction();

        Collider2D closestEnemy = null;

        switch (m_TargetType)
        {
            case ETargetType.Random:
                closestEnemy = GetRandomEnemyInRange();
                break;
            case ETargetType.Furthest:
                closestEnemy = GetFurthestEnemyInRange();
                break;
            case ETargetType.Closest:
                closestEnemy = GetClosestEnemyInRange();
                break;
            default:
                break;
        }

        if (closestEnemy == null)
            return;

        FireProjectile(closestEnemy.gameObject);
    }

    private void FireProjectile(GameObject InTarget)
    {
        if (m_Projectile == null)
        {
            Debug.LogWarning("No projectile set in " + this.GetType().ToString() + "!");
            return;
        }

        for (int i = 0; i < m_Stats.ProjectileAmount; i++)
        {
            GameObject newProjectile = SpawnProjectile(m_Projectile, transform.position, Quaternion.identity);

            if (newProjectile.TryGetComponent<ProjectileBase>(out ProjectileBase projectile))
            {
                Vector2 fireDirection = (InTarget.transform.position - transform.position).normalized;

                projectile.SetFireDirection(fireDirection);
            }
        }
        SoundManager.Instance.PlayGeneralSound(m_ActionSound, true);
    }
}
