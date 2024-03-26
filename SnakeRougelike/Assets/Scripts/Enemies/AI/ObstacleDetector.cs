using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleDetector : Detector
{
    [SerializeField]
    private float m_DetectionRadius = 2;

    [SerializeField]
    private LayerMask m_LayerMask;

    [SerializeField]
    private bool m_ShowGizmos = true;

    List<Collider2D> m_Colliders = new List<Collider2D>();

    public override void Detect(AIData InAiData)
    {
        m_Colliders = Physics2D.OverlapCircleAll(transform.position, m_DetectionRadius, m_LayerMask).ToList();

        InAiData.Obstacles = m_Colliders;
    }

    private void OnDrawGizmos()
    {
        if (m_ShowGizmos == false)
            return;

        if(Application.isPlaying && m_Colliders != null)
        {
            Gizmos.color = Color.red;
            foreach(Collider2D obstacle in m_Colliders)
            {
                Gizmos.DrawSphere(obstacle.transform.position, 0.2f);
            }
        }
    }
}
