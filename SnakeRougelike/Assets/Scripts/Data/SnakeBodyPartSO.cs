using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBodyPart", menuName = "ScriptableObjects/NewSnakeBodyPart")]
public class SnakeBodyPartSO : ScriptableObject
{
    public EBodyPart Part;

    public GameObject Lvl1BodyPartPrefab;
    public GameObject Lvl2BodyPartPrefab;
    public GameObject Lvl3BodyPartPrefab;

    public int Cost;
    public string Name;
}
