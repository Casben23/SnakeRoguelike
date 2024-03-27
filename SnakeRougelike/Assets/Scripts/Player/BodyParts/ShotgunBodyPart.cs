using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBodyPart : SnakeBodyPartBase
{
    [SerializeField] GameObject m_Projectile;
    protected override void PerformAction()
    {
        base.PerformAction();

        Collider2D closestEnemy = GetClosestEnemyInRange();
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
        SoundManager.Instance.PlayGeneralSound(SFXType.ProjectilFire, true);
    }
}
