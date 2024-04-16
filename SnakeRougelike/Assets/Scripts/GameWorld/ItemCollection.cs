using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<ModifierBase> m_ModifierItems = new List<ModifierBase>();

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

    public List<ModifierBase> GetRandomItemModifiers(int InAmount)
    {
        List<ModifierBase> resultList = new List<ModifierBase>();
        List<ModifierBase> tempModifiers = new List<ModifierBase>(m_ModifierItems);
       
        for(int i = 0; i < InAmount; i++)
        {
            int randomIndex = Random.Range(0, tempModifiers.Count);

            resultList.Add(tempModifiers[randomIndex]);
            tempModifiers.RemoveAt(randomIndex);
            m_ModifierItems.Remove(resultList[i]);
        }

        return resultList;
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

    public SnakeBodyPartSO GetSnakePartSO(EBodyPart InBodyPart)
    {
        foreach(SnakeBodyPartSO bodyPartSO in m_SnakeBodyParts)
        {
            if (bodyPartSO.Part == InBodyPart)
                return bodyPartSO;
        }

        return null;
    }

    public void RemoveItemFromCollection(ModifierBase InModifier)
    {
        m_ModifierItems.Remove(InModifier);
    }
}
