using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PistolBodyPart : SnakeBodyPartBase
{
    [SerializeField] LayerMask m_EnemyLayerMask;
    [SerializeField] GameObject m_Projectile;

    protected override void PerformAction()
    {
        base.PerformAction();

        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, 10000, m_EnemyLayerMask).ToList();

        if (colliders.Count <= 0)
            return;

        colliders.Sort((a, b) =>
        {
            float distanceToA = Vector2.Distance(transform.position, a.transform.position);
            float distanceToB = Vector2.Distance(transform.position, b.transform.position);
            return distanceToA.CompareTo(distanceToB);
        });

        FireProjectile(colliders[0].gameObject);
    }

    private void FireProjectile(GameObject InTarget)
    {
        if(m_Projectile == null)
        {
            Debug.LogWarning("No projectile set in [CanonBodyPart] !");
            return;
        }

        GameObject newProjectile = SpawnProjectile(m_Projectile, transform.position, Quaternion.identity);

        if (newProjectile.TryGetComponent<ProjectileBase>(out ProjectileBase projectile))
        {
            Vector2 fireDirection = (InTarget.transform.position - transform.position).normalized;

            projectile.SetFireDirection(fireDirection);
        }

        SoundManager.Instance.PlayGeneralSound(SFXType.ProjectilFire, true);

        Destroy(newProjectile, 10);
    }
}
