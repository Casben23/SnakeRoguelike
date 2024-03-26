using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float m_TargetReachedThreshold = 0.5f;

    [SerializeField]
    private bool m_ShowGizmo = true;

    bool m_ReachedLastTarget = true;

    private Vector2 m_TargetPositionCached;
    private float[] m_InterestTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] InDanger, float[] InInterest, AIData InAiData)
    {
        if(m_ReachedLastTarget)
        {
            if(InAiData.Targets == null || InAiData.Targets.Count <= 0)
            {
                InAiData.CurrentTarget = null;
                return (InDanger, InInterest);
            }
            else
            {
                m_ReachedLastTarget = false;
                InAiData.CurrentTarget = InAiData.Targets.OrderBy
                    (target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
            }
        }

        if (InAiData.CurrentTarget != null && InAiData.Targets != null && InAiData.Targets.Contains(InAiData.CurrentTarget))
            m_TargetPositionCached = InAiData.CurrentTarget.position;

        //First check if we have reached the target
        if (Vector2.Distance(transform.position, m_TargetPositionCached) < m_TargetReachedThreshold)
        {
            m_ReachedLastTarget = true;
            InAiData.CurrentTarget = null;
            return (InDanger, InInterest);
        }

        //If we havent yet reached the target do the main logic of finding the interest directions
        Vector2 directionToTarget = (m_TargetPositionCached - (Vector2)transform.position);
        for (int i = 0; i < InInterest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Direction2D.eightDirectionsNormalizedList[i]);

            //accept only directions at the less than 90 degrees to the target direction
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > InInterest[i])
                {
                    InInterest[i] = valueToPutIn;
                }

            }
        }
        m_InterestTemp = InInterest;
        return (InDanger, InInterest);
    }

    private void OnDrawGizmos()
    {
        if (m_ShowGizmo == false)
            return;
        Gizmos.DrawSphere(m_TargetPositionCached, 0.2f);

        if (Application.isPlaying && m_InterestTemp != null)
        {
            if (m_InterestTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < m_InterestTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Direction2D.eightDirectionsNormalizedList[i] * m_InterestTemp[i] * 2);
                }
                if (m_ReachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(m_TargetPositionCached, 0.1f);
                }
            }
        }
    }
}
