using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float m_TargetDetectionRange = 5;

    [SerializeField]
    private LayerMask m_PlayerLayerMask;

    [SerializeField]
    private bool m_ShowGizmos = false;

    //Gizmos
    private List<Transform> m_Colliders;

    public override void Detect(AIData InAiData)
    {
        //Find out if player is near
        Collider2D playerCollider =
            Physics2D.OverlapCircle(transform.position, m_TargetDetectionRange, m_PlayerLayerMask);

        if (playerCollider != null)
        {
            //Check if you see the player
            m_Colliders = new List<Transform>() { playerCollider.transform };
        }
        else
        {
            m_Colliders = null;
        }
        InAiData.Targets = m_Colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_ShowGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, m_TargetDetectionRange);

        if (m_Colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in m_Colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }

}
