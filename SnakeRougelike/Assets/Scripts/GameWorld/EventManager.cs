using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;

    public UnityEvent<EventModifierData> OnProjectileHitEnemy;
    public UnityEvent<EventModifierData> OnEnemyTakeDamage;
    public UnityEvent<EventModifierData> OnEnemyDeath;
    public UnityEvent<EventModifierData> OnProjectileFired;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Search for existing instance
                instance = FindObjectOfType<EventManager>();

                // Create new instance if one doesn't already exist
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("EventManager");
                    instance = singletonObject.AddComponent<EventManager>();
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


    public void EnemyTakeDamage(EventModifierData InData)
    {
        OnEnemyTakeDamage?.Invoke(InData);
    }
    public void EnemyDeath(EventModifierData InData)
    {
        OnEnemyDeath?.Invoke(InData);
    }
    public void ProjectileHitEnemy(EventModifierData InData)
    {
        OnProjectileHitEnemy?.Invoke(InData);
    }
    public void ProjectileFired(EventModifierData InData)
    {
        OnProjectileFired?.Invoke(InData);
    }
}
