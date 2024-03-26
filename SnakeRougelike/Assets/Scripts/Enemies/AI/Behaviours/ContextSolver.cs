using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    [SerializeField]
    private bool m_ShowGizmos = true;

    //gozmo parameters
    float[] m_InterestGizmo = new float[0];
    Vector2 m_ResultDirection = Vector2.zero;
    private float m_RayLength = 2;

    private void Start()
    {
        m_InterestGizmo = new float[8];
    }

    public Vector2 GetDirectionToMove(List<SteeringBehaviour> InBehaviours, AIData InAiData)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        //Loop through each behaviour
        foreach (SteeringBehaviour behaviour in InBehaviours)
        {
            (danger, interest) = behaviour.GetSteering(danger, interest, InAiData);
        }

        //subtract danger values from interest array
        for (int i = 0; i < 8; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        m_InterestGizmo = interest;

        //get the average direction
        Vector2 outputDirection = Vector2.zero;
        for (int i = 0; i < 8; i++)
        {
            outputDirection += Direction2D.eightDirectionsNormalizedList[i] * interest[i];
        }

        outputDirection.Normalize();

        m_ResultDirection = outputDirection;

        //return the selected movement direction
        return m_ResultDirection;
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying && m_ShowGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, m_ResultDirection * m_RayLength);
        }
    }
}
