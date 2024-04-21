using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyStats
{
    [Header("Defence")]
    public float Health = 100f;
    public float Weight = 1f;

    [Header("Movement")]
    public float MovementSpeed = 5;

    [Header("Misc")]
    public int XpDrop = 5;
}

public enum EnemyState
{
    Moveing,
    Attack,
    Damaged
}

[RequireComponent(typeof(HealthController))]
public class EnemyBase : MonoBehaviour
{
    //AI
    [Header("AI Settings")]
    [SerializeField]
    protected AIData m_AIData;

    [SerializeField]
    protected List<SteeringBehaviour> m_SteeringBehaviours;

    [SerializeField]
    private List<Detector> m_Detectors;

    [SerializeField]
    private float m_DetectionDelay = 0.05f;

    [SerializeField]
    private ContextSolver m_ContextSolver;

    protected Vector2 m_CurrentMovementDirection;
    //AI

    [SerializeField] private int m_MaxHealth = 5;
    [SerializeField] private float m_RotationSpeed = 3;
    [SerializeField] private float m_MoveSpeed = 5;

    private SpriteRenderer m_SpriteRenderer;
    protected float m_DamagedTime = 0f;

    private HealthController m_HealthController;
    [SerializeField] private float m_FlashDuration = 0.2f;
    [SerializeField] private Color m_FlashColor = Color.white;

    [SerializeField] private GameObject m_DeathEffect;

    private Animator m_Animator;
    private Collider2D m_Collider;

    private Color m_StartColor;
    private bool m_IsFlashing = false;

    private bool m_IsDead = false;

    private void Start()
    {
        m_SpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        m_ContextSolver = gameObject.GetComponent<ContextSolver>();
        m_AIData = gameObject.GetComponent<AIData>();
        m_SteeringBehaviours = gameObject.GetComponents<SteeringBehaviour>().ToList();
        m_Detectors = gameObject.GetComponentsInChildren<Detector>().ToList();

        m_Animator = gameObject?.GetComponent<Animator>();
        m_Collider = gameObject.GetComponent<Collider2D>();

        m_StartColor = m_SpriteRenderer.color;

        m_HealthController = gameObject.GetComponent<HealthController>();
        m_HealthController.SetupHealthController(m_MaxHealth + (10 * GameManager.Instance.GetCurrentLevel()));

        UIHealthBarController.Instance.CreateNewHealthBar(m_HealthController);

        SoundManager.Instance.PlayGeneralSound(SFXType.EnemySpawn, true);

        InvokeRepeating("PerformDetection", 0, m_DetectionDelay);
    }

    private void Update()
    {
        if (m_IsDead == true)
            return;

        m_CurrentMovementDirection = m_ContextSolver.GetDirectionToMove(m_SteeringBehaviours, m_AIData);

        UpdateMovement();
    }

    void UpdateMovement()
    {
        Vector2 direction = (transform.position + (Vector3)m_CurrentMovementDirection) - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);

        transform.position += transform.up * m_MoveSpeed * Time.deltaTime;
    }

    private void PerformDetection()
    {
        if (m_IsDead == true)
            return;

        foreach (Detector detector in m_Detectors)
        {
            detector.Detect(m_AIData);
        }
    }

    public void Die()
    {
        if (m_IsDead == true)
            return;

        if (m_Collider != null)
            m_Collider.enabled = false;

        m_IsDead = true;

        GameStats gameStats = GameStatisticsManager.Instance.GetGameStats();
        gameStats.EnemiesKilled += 1;
        gameStats.EnemiesKilledThisLevel += 1;

        SoundManager.Instance.PlayGeneralSound(SFXType.EnemyDie, true);

        Instantiate(m_DeathEffect, transform.position, Quaternion.identity);

        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.3f).setEaseOutCubic().setOnComplete(DestroyEnemy);

        GameState.Instance.AddEnemiesKilled(1);
    }

    void DestroyEnemy()
    {
        EventModifierData eventData = new EventModifierData();
        eventData.Enemy = this;

        EventManager.Instance.EnemyDeath(eventData);

        WaveController.Instance.OnEnemyDeath();

        Destroy(gameObject);
    }

    public void OnTakeDamage(int InDamage)
    {
        if (m_HealthController == null)
        {
            Die();
            return;
        }

        m_HealthController.TakeDamage(InDamage);
        GameState.Instance.AddDamageDone(InDamage);

        GameStats gameStats = GameStatisticsManager.Instance.GetGameStats();
        gameStats.DamageDealt += InDamage;

        SoundManager.Instance.PlayGeneralSound(SFXType.EnemyHit, true);
        GameManager.Instance.SpawnDamageText(transform.position, InDamage);

        EventModifierData eventData = new EventModifierData();
        eventData.Enemy = this;

        EventManager.Instance.EnemyTakeDamage(eventData);

        if (m_HealthController.IsDead())
        {
            Die();
            return;
        }

        Flash();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SnakeBodyPartBase>(out SnakeBodyPartBase bodyPart))
        {
            bodyPart.TakeDamage();
            OnTakeDamage(m_HealthController.GetCurrentHealth());
        }
    }

    private void Flash()
    {
        if (!m_IsFlashing)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        m_IsFlashing = true;

        if (m_SpriteRenderer == null)
            yield return null;

        m_SpriteRenderer.color = new Color(m_FlashColor.r, m_FlashColor.g, m_FlashColor.b);

        float elapsedTime = 0f;
        while (elapsedTime < m_FlashDuration)
        {
            m_SpriteRenderer.color = Color.Lerp(m_FlashColor, m_StartColor, elapsedTime / m_FlashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_SpriteRenderer.color = m_StartColor;

        m_IsFlashing = false;
    }
}
