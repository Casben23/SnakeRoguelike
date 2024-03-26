using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float m_Radius = 2f, m_AgentColliderSize = 0.6f;

    [SerializeField]
    private bool m_ShowGizmos = true;

    //Gizmos Parameters
    float[] m_DangerResultTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] InDanger, float[] InInterest, AIData InAiData)
    {
        foreach (Collider2D obstacleCollider in InAiData.Obstacles)
        {
            if (obstacleCollider == null)
                continue;

            Vector2 directionToObstacle = obstacleCollider.transform.position - transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            float weight = distanceToObstacle <= m_AgentColliderSize ? 1 : (m_Radius - distanceToObstacle) / m_Radius;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;
            for (int i = 0; i < Direction2D.eightDirectionsNormalizedList.Count; i++)
            {
                float result = Vector2.Dot(directionToObstacleNormalized, Direction2D.eightDirectionsNormalizedList[i]);
                float valueToPutIn = result * weight;

                if(valueToPutIn > InDanger[i])
                {
                    InDanger[i] = valueToPutIn;
                }
            }
        }
        m_DangerResultTemp = InDanger;
        return (InDanger, InInterest);
    }

    private void OnDrawGizmos()
    {
        if (m_ShowGizmos == false)
            return;

        if (Application.isPlaying && m_DangerResultTemp != null)
        {
            if (m_DangerResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < m_DangerResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(
                        transform.position,
                        Direction2D.eightDirectionsNormalizedList[i] * m_DangerResultTemp[i] * 2
                        );
                }
            }
        }

    }
}
