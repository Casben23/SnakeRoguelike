using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float m_MoveSpeed = 10f;
    [SerializeField] private float m_RotationSpeed = 10f;
    [SerializeField] private LayerMask m_BodyLayerMask;
    [SerializeField] private float m_TimeUntilCanEat = 2;

    private Vector2 m_CurrentMousePosition;
    private SnakeBodyController m_BodyController;
    private bool m_HasEaten = false;

    private float m_CurrentTimeUntilCanEat = 0;

    void Start()
    {
        m_BodyController = gameObject.GetComponent<SnakeBodyController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMousePosition();
        UpdateRotation();
        UpdateMovement();

        TryEatBody();

        m_BodyController.UpdateMovement(m_MoveSpeed);
    }

    public void SetEatCoolDown(float InTime)
    {
        m_CurrentTimeUntilCanEat = InTime;
    }

    void TryEatBody()
    {
        if (m_HasEaten)
        {
            if(m_CurrentTimeUntilCanEat <= 0)
            {
                m_HasEaten = false;
            }
            else
            {
                m_CurrentTimeUntilCanEat -= Time.deltaTime;
                return;
            }
        }

        Collider2D collider = Physics2D.OverlapCircle(transform.position + transform.up, 0.2f, m_BodyLayerMask);

        if (collider == null)
            return;

        m_BodyController.RemoveBodyPart(collider.gameObject);
        m_HasEaten = true;
        m_CurrentTimeUntilCanEat = m_TimeUntilCanEat;
    }

    void UpdateMousePosition()
    {
        if(Camera.main == null)
        {
            Debug.LogWarning("Main Camera Not Found [SnakeHeadController]");
            return;
        }

        m_CurrentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void UpdateMovement()
    {
        Vector2 newPosition = gameObject.transform.up * m_MoveSpeed * Time.deltaTime;

        gameObject.transform.position += (Vector3)newPosition;
    }

    void UpdateRotation()
    {
        Vector2 direction = m_CurrentMousePosition - (Vector2)transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
    }
}
