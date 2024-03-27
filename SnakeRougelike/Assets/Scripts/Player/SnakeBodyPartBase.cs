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
    public int AreaOfEffect = 3;
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

[RequireComponent(typeof(Animator))]
public class SnakeBodyPartBase : MonoBehaviour
{
    [SerializeField] private EBodyPart m_Part;
    [SerializeField] private List<EBodyPartClass> m_Classes;

    [SerializeField] protected WeaponPartStats m_Stats;

    [Header("Misc")]
    [SerializeField] private LayerMask m_EnemyLayerMask;
    [SerializeField] private float m_FlashDuration = 0.2f;
    [SerializeField] private Color m_FlashColor = Color.white;

    private Animator m_Animator;

    private int m_CurrentHealth = 1;
    private SpriteRenderer m_SpriteRenderer;
    private Color m_StartColor;
    private bool m_IsFlashing = false;

    private float m_CurrentActionCooldown = 0f;

    SnakeBodyController m_BodyController;

    public WeaponPartStats GetWeaponPartStats()
    {
        return m_Stats;
    }

    public EBodyPart GetPart()
    {
        return m_Part;
    }

    public List<EBodyPartClass> GetClasses()
    {
        return m_Classes;
    }

    public void SetController(SnakeBodyController InController)
    {
        m_BodyController = InController;
    }
    public void TakeDamage()
    {
        m_CurrentHealth -= 1;

        if (m_CurrentHealth <= 0)
        {
            m_BodyController?.RemoveBodyPart(gameObject);
        }
    }

    protected List<Collider2D> GetEnemiesInRange()
    {
        return Physics2D.OverlapCircleAll(transform.position, m_Stats.Range, m_EnemyLayerMask).ToList();
    }

    protected Collider2D GetClosestEnemyInRange()
    {
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, m_Stats.Range, m_EnemyLayerMask).ToList();

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
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, m_Stats.Range, m_EnemyLayerMask).ToList();

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
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, m_Stats.Range, m_EnemyLayerMask).ToList();

        if (colliders.Count <= 0)
            return null;

        int randomIndex = Random.Range(0, colliders.Count);

        return colliders[randomIndex];
    }

    protected virtual void PerformAction()
    {
        m_CurrentActionCooldown = m_Stats.ActionCooldown;
        m_Animator.SetTrigger("OnAction");

        Flash();
    }

    protected GameObject SpawnProjectile(GameObject InProjectileToSpawn, Vector2 InPosition, Quaternion InRotation)
    {
        GameObject newProjectile = Instantiate(InProjectileToSpawn, InPosition, InRotation);

        ProjectileBase projectileBase = newProjectile.GetComponent<ProjectileBase>();
        if (projectileBase == null)
            return null;

        projectileBase.SetInstigator(this);

        return newProjectile;
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
        m_CurrentActionCooldown = m_Stats.ActionCooldown;

        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_Animator = gameObject.GetComponent<Animator>();

        m_StartColor = m_SpriteRenderer.color;
        m_CurrentHealth = m_Stats.Health;
    }

    private void Update()
    {
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
