using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    [SerializeField] float m_Speed = 700;
    [SerializeField] private GameObject m_HitEffect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * m_Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<SnakeBodyPartBase>(out SnakeBodyPartBase bodyPart))
        {
            bodyPart.TakeDamage();
            Instantiate(m_HitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
