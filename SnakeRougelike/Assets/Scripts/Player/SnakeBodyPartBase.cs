using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WeaponPartStats : ICloneable
{
    [Header("Common")]
    public float ActionCooldown = 1f;
    public int Health = 1;

    [Header("Combat")]
    public float ProjectileSpeed = 15;
    public int Damage = 1;
    public float AreaOfEffect = 3;
    public int PierceAmount = 0;
    public float Range = 5;
    public int ProjectileAmount = 1;

    [Range(0, 1)]
    public float Spread = 0;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

[RequireComponent(typeof(Animator), typeof(HealthController))]
public class SnakeBodyPartBase : MonoBehaviour
{
    [SerializeField] private EBodyPart m_Part;

    [SerializeField] private WeaponPartStats m_BaseStats = new WeaponPartStats();
    protected WeaponPartStats m_ModifiedStats = new WeaponPartStats();

    [Header("Misc")]
    [SerializeField] private bool m_IsPassive = false;
    [SerializeField] private LayerMask m_EnemyLayerMask;
    [SerializeField] private float m_FlashDuration = 0.2f;
    [SerializeField] private Color m_FlashColor = Color.white;

    private Animator m_Animator;

    private SpriteRenderer m_SpriteRenderer;
    private Color m_StartColor;
    private bool m_IsFlashing = false;

    private float m_CurrentActionCooldown = 0f;
    private HealthController m_HealthController;

    SnakeBodyController m_BodyController;

    public WeaponPartStats GetWeaponPartBaseStats()
    {
        return m_BaseStats;
    }

    public WeaponPartStats GetWeaponPartModifiedStats()
    {
        return m_ModifiedStats;
    }

    public EBodyPart GetPart()
    {
        return m_Part;
    }

    public void SetController(SnakeBodyController InController)
    {
        m_BodyController = InController;
    }

    public void TakeDamage()
    {
        m_HealthController.TakeDamage(1);

        if (m_HealthController.IsDead())
        {
            m_BodyController?.RemoveBodyPart(gameObject);
        }
    }

    public void UpdateStats()
    {
        m_ModifiedStats = (WeaponPartStats)m_BaseStats.Clone();

        Modifiers currentMods = ModifierController.Instance.GetModifiers();

        m_ModifiedStats.Health = Mathf.FloorToInt(m_ModifiedStats.Health * currentMods.Mod_MaxHealth);
        m_ModifiedStats.Damage = Mathf.FloorToInt(m_ModifiedStats.Damage * currentMods.Mod_Damage);
        m_ModifiedStats.Spread *= currentMods.Mod_Spread;
        m_ModifiedStats.ActionCooldown *= currentMods.Mod_ActionCooldown;

        m_ModifiedStats.Range *= currentMods.Mod_Range;
        m_ModifiedStats.ProjectileAmount *= currentMods.Mod_ProjectileAmount;
        m_ModifiedStats.ProjectileSpeed *= currentMods.Mod_ProjectileSpeed;
        m_ModifiedStats.PierceAmount += currentMods.Mod_PierceAmount;
        m_ModifiedStats.AreaOfEffect *= currentMods.Mod_AreaOfEffect;
    }

    public WeaponPartStats GetModifiedStatClone()
    {
        WeaponPartStats resultStats = (WeaponPartStats)m_BaseStats.Clone();

        Modifiers currentMods = ModifierController.Instance.GetModifiers();

        resultStats.Health = Mathf.FloorToInt(resultStats.Health * currentMods.Mod_MaxHealth);
        resultStats.Damage = Mathf.FloorToInt(resultStats.Damage * currentMods.Mod_Damage);
        resultStats.Spread *= currentMods.Mod_Spread;
        resultStats.ActionCooldown *= currentMods.Mod_ActionCooldown;

        resultStats.Range *= currentMods.Mod_Range;
        resultStats.ProjectileAmount *= currentMods.Mod_ProjectileAmount;
        resultStats.ProjectileSpeed *= currentMods.Mod_ProjectileSpeed;
        resultStats.PierceAmount += currentMods.Mod_PierceAmount;
        resultStats.AreaOfEffect *= currentMods.Mod_AreaOfEffect;

        return resultStats;
    }

    protected List<Collider2D> GetEnemiesInRange()
    {
        return Physics2D.OverlapCircleAll(transform.position, m_BaseStats.Range, m_EnemyLayerMask).ToList();
    }

    protected Collider2D GetClosestEnemyInRange()
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, m_BaseStats.Range, m_EnemyLayerMask).ToList();

        if (colliders.Count <= 0)
            return null;

        colliders.Sort((a, b) =>
        {
            float distanceToA = Vector2.Distance(transform.position, a.transform.position);
            float distanceToB = Vector2.Distance(transform.position, b.transform.position);
            return distanceToA.CompareTo(distanceToB);
        });

        return colliders[0];
    }

    protected Collider2D GetFurthestEnemyInRange()
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, m_BaseStats.Range, m_EnemyLayerMask).ToList();

        if (colliders.Count <= 0)
            return null;

        colliders.Sort((a, b) =>
        {
            float distanceToA = Vector2.Distance(transform.position, a.transform.position);
            float distanceToB = Vector2.Distance(transform.position, b.transform.position);
            return distanceToA.CompareTo(distanceToB);
        });

        return colliders[colliders.Count - 1];
    }

    protected Collider2D GetRandomEnemyInRange()
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, m_BaseStats.Range, m_EnemyLayerMask).ToList();

        if (colliders.Count <= 0)
            return null;

        int randomIndex = Random.Range(0, colliders.Count);

        return colliders[randomIndex];
    }

    protected virtual void PerformAction()
    {
        if (m_IsPassive)
            return;

        m_CurrentActionCooldown = m_BaseStats.ActionCooldown;
        m_Animator.SetTrigger("OnAction");

        Flash();
    }

    private void Flash()
    {
        if (!m_IsFlashing)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private void Start()
    {
        m_CurrentActionCooldown = m_BaseStats.ActionCooldown;

        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_Animator = gameObject.GetComponent<Animator>();

        m_StartColor = m_SpriteRenderer.color;

        m_HealthController = gameObject.GetComponent<HealthController>();
        m_HealthController.SetupHealthController(m_BaseStats.Health);

        UIHealthBarController.Instance.CreateNewHealthBar(m_HealthController);
    }

    private void Update()
    {
        if (m_IsPassive)
            return;

        if (m_CurrentActionCooldown <= 0)
        {
            PerformAction();
        }
        else
        {
            m_CurrentActionCooldown -= Time.deltaTime;
        }
    }

    private IEnumerator FlashCoroutine()
    {
        m_IsFlashing = true;

        m_SpriteRenderer.material.SetColor("_FlashColor", new Color(m_FlashColor.r, m_FlashColor.g, m_FlashColor.b));

        float elapsedTime = 0f;
        while (elapsedTime < m_FlashDuration)
        {
            m_SpriteRenderer.material.SetFloat("_FlashAmount", Mathf.Lerp(1, 0, elapsedTime / m_FlashDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_SpriteRenderer.material.SetFloat("_FlashAmount", 0);

        m_IsFlashing = false;
    }

}
