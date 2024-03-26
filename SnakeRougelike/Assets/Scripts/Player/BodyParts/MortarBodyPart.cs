using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MortarBodyPart : SnakeBodyPartBase
{
    [SerializeField] LayerMask m_EnemyLayerMask;
    [SerializeField] GameObject m_Projectile;

    protected override void PerformAction()
    {
        base.PerformAction();

        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, 10000, m_EnemyLayerMask).ToList();

        if (colliders.Count <= 0)
            return;

        int randomIndex = Random.Range(0, colliders.Count);

        FireProjectile(colliders[randomIndex].gameObject);
    }

    private void FireProjectile(GameObject InTarget)
    {
        if (m_Projectile == null)
        {
            Debug.LogWarning("No projectile set in [MortarBodyPart] !");
            return;
        }

        GameObject newProjectile = SpawnProjectile(m_Projectile, InTarget.transform.position, Quaternion.identity);

        if (newProjectile.TryGetComponent<ProjectileBase>(out ProjectileBase projectile))
        {
            projectile.SetFireDirection(Vector2.zero);
        }

        SoundManager.Instance.PlayGeneralSound(SFXType.ProjectilFire, true);

        Destroy(newProjectile, 10);
    }
}
