using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    private static ItemCollection instance;

    public static ItemCollection Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Ensure only one instance of the WaveController exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [SerializeField] private List<SnakeBodyPartSO> m_SnakeBodyParts = new List<SnakeBodyPartSO>();


    public SnakeBodyPartSO GetRandomSnakeBodyPart()
    {
        int randomIndex = Random.Range(0, m_SnakeBodyParts.Count);
        return m_SnakeBodyParts[randomIndex];
    }

    public GameObject GetBodyPartWithLevel(EBodyPart InPart, int InLevel)
    {
        foreach (SnakeBodyPartSO partSO in m_SnakeBodyParts)
        {
            if (partSO.Part != InPart)
                continue;

            if (InLevel == 1)
                return partSO.Lvl1BodyPartPrefab;
            else if (InLevel == 2)
                return partSO.Lvl2BodyPartPrefab;
            else if (InLevel == 3)
                return partSO.Lvl3BodyPartPrefab;
        }

        Debug.LogError("Failed to get part: [" + InPart.ToString() + "] Level: " + InLevel.ToString());

        return null;
    }

    public Sprite GetPartSprite(EBodyPart InBodyPart)
    {
        foreach(SnakeBodyPartSO partSO in m_SnakeBodyParts)
        {
            if(partSO.Part == InBodyPart)
            {
                GameObject partObject = partSO.Lvl1BodyPartPrefab;
                return partObject.GetComponent<SpriteRenderer>().sprite;
            }
        }

        Debug.LogError("No Sprite found for [" + InBodyPart.ToString() + "]");
        return null;
    }
}
