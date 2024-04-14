using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EModifierEvent
{
    OnProjectileHitEnemy,
    OnEnemyTakeDamage,
    OnEnemyDeath,
    OnProjectileFire
}

public class EventModifierData
{
    public EnemyBase Enemy = null;
    public ProjectileBase Projectile = null;
    public SnakeBodyPartBase BodyPart = null;
}

public class EventModifierBase : ModifierBase
{
    [SerializeField] private EModifierEvent EventModifier;

    // Start is called before the first frame update
    void Start()
    {
        switch (EventModifier)
        {
            case EModifierEvent.OnProjectileHitEnemy:
                EventManager.Instance.OnProjectileHitEnemy.AddListener(OnProjectileHitEvent);
                break;
            case EModifierEvent.OnEnemyTakeDamage:
                EventManager.Instance.OnEnemyTakeDamage.AddListener(OnEnemyTakeDamageEvent);
                break;
            case EModifierEvent.OnEnemyDeath:
                EventManager.Instance.OnEnemyDeath.AddListener(OnEnemyDeathEvent);
                break;
            case EModifierEvent.OnProjectileFire:
                EventManager.Instance.OnProjectileFired.AddListener(OnProjectileFireEvent);
                break;
        }
    }

    private void OnDisable()
    {
        switch (EventModifier)
        {
            case EModifierEvent.OnProjectileHitEnemy:
                EventManager.Instance.OnProjectileHitEnemy.RemoveListener(OnProjectileHitEvent);
                break;
            case EModifierEvent.OnEnemyTakeDamage:
                EventManager.Instance.OnEnemyTakeDamage.RemoveListener(OnEnemyTakeDamageEvent);
                break;
            case EModifierEvent.OnEnemyDeath:
                EventManager.Instance.OnEnemyDeath.RemoveListener(OnEnemyDeathEvent);
                break;
            case EModifierEvent.OnProjectileFire:
                EventManager.Instance.OnProjectileFired.RemoveListener(OnProjectileFireEvent);
                break;
        }
    }

    protected virtual void OnEnemyTakeDamageEvent(EventModifierData InData) { }
    protected virtual void OnProjectileHitEvent(EventModifierData InData) { }
    protected virtual void OnEnemyDeathEvent(EventModifierData InData) { }
    protected virtual void OnProjectileFireEvent(EventModifierData InData) { }
}
