using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MortarBodyPart : SnakeBodyPartBase
{
    [SerializeField] GameObject m_Projectile;

    protected override void PerformAction()
    {
        base.PerformAction();

        Collider2D randomEnemyInRange = GetRandomEnemyInRange();
        if (randomEnemyInRange == null)
            return;

        FireProjectile(randomEnemyInRange.gameObject);
    }

    private void FireProjectile(GameObject InTarget)
    {
        if (m_Projectile == null)
        {
            Debug.LogWarning("No projectile set in [MortarBodyPart] !");
            return;
        }

        GameObject newProjectile = ProjectileManager.Instance.SpawnProjectile(this, m_Projectile, InTarget.transform.position, Quaternion.identity);

        if (newProjectile.TryGetComponent<ProjectileBase>(out ProjectileBase projectile))
        {
            projectile.SetFireDirection(Vector2.zero);
        }

        SoundManager.Instance.PlayGeneralSound(SFXType.ProjectilFire, true);

        Destroy(newProjectile, 10);
    }
}
