using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;

    public static ProjectileManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Search for existing instance
                instance = FindObjectOfType<ProjectileManager>();

                // Create new instance if one doesn't already exist
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("EventManager");
                    instance = singletonObject.AddComponent<ProjectileManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject SpawnProjectile(SnakeBodyPartBase InInstigator, GameObject InProjectileToSpawn, Vector2 InPosition, Quaternion InRotation)
    {
        GameObject newProjectile = Instantiate(InProjectileToSpawn, InPosition, InRotation);

        ProjectileBase projectileBase = newProjectile.GetComponent<ProjectileBase>();
        if (projectileBase == null)
            return null;

        projectileBase.SetInstigator(InInstigator);

        EventModifierData eventData = new EventModifierData();
        eventData.Projectile = projectileBase;
        eventData.BodyPart = InInstigator;

        EventManager.Instance.ProjectileFired(eventData);

        return newProjectile;
    }

    public void TryDoExplosionDamage(Vector2 InPosition, float InRadius, int InDamage, LayerMask InLayerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(InPosition, InRadius, InLayerMask);

        if (colliders.Length <= 0)
        {
            Destroy(gameObject);
            return;
        }

        foreach (Collider2D coll in colliders)
        {
            if (coll.TryGetComponent<EnemyBase>(out EnemyBase enemy))
            {
                enemy.OnTakeDamage(InDamage);
            }
        }
    }
}
