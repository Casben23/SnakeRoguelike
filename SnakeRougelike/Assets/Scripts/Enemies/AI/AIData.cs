using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> Targets = null;
    public List<Collider2D> Obstacles = null;

    public Transform CurrentTarget;

    public int GetTargetsCount() => Targets == null ? 0 : Targets.Count;

    public Vector2 GetDirectionToTarget()
    {
        if (CurrentTarget == null)
            return Vector2.zero;

        return (CurrentTarget.position - transform.position).normalized;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),    //UP
        new Vector2Int(1,0),    //RIGHT
        new Vector2Int(0,-1),   //DOWN
        new Vector2Int(-1,0)    //LEFT
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1),    //UP-RIGHT
        new Vector2Int(1,-1),   //RIGHT-DOWN
        new Vector2Int(-1,-1),  //DOWN-LEFT
        new Vector2Int(-1,1)    //LEFT-UP
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),    //UP
        new Vector2Int(1,1),    //UP-RIGHT
        new Vector2Int(1,0),    //RIGHT
        new Vector2Int(1,-1),   //RIGHT-DOWN
        new Vector2Int(0,-1),   //DOWN
        new Vector2Int(-1,-1),  //DOWN-LEFT
        new Vector2Int(-1,0),   //LEFT
        new Vector2Int(-1,1)    //LEFT-UP
    };

    public static List<Vector2> eightDirectionsNormalizedList = new List<Vector2>
    {
        new Vector2(0,1).normalized,    //UP
        new Vector2(1,1).normalized,    //UP-RIGHT
        new Vector2(1,0).normalized,    //RIGHT
        new Vector2(1,-1).normalized,   //RIGHT-DOWN
        new Vector2(0,-1).normalized,   //DOWN
        new Vector2(-1,-1).normalized,  //DOWN-LEFT
        new Vector2(-1,0).normalized,   //LEFT
        new Vector2(-1,1).normalized    //LEFT-UP
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
