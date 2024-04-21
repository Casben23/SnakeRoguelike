using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EBodyPartClass
{
    Artillery = 0,
    Firearms = 1,
    Support = 2,
}

public enum EBodyPart
{
    // DO NOT FORGET TO ADD INDEX TO NEW ENUM
    // PLACE ENUM WHERE YOU WANT BUT INCREMENT HIGHEST NUMBER

    //Artillery
    Mortar = 0,
    HomingMissleLauncher = 1,
    LandmineDispenser = 2,

    //Firearms
    Pistol = 3,
    SMG = 4,
    Sniper = 5,
    Shotgun = 10, // <--- HIGHEST

    //Supports
    ArtilleryLoader = 6,
    BoxOfTNT = 7,
    BoxOfGuns = 8,
    DoubleBarrels = 9,

    COUNT = 11
}

public class WeaponContainer
{
    EBodyPart BodyPartType = EBodyPart.COUNT;
    int Amount = 0;
}

public class SnakeBodyController : MonoBehaviour
{
    [SerializeField] private GameObject m_SnakeBodyPart;
    [SerializeField] private GameObject m_DestroyEffect;

    private List<GameObject> m_ActiveBodyParts = new List<GameObject>();
    private List<Tuple<GameObject, int>> m_DeadBodyParts = new List<Tuple<GameObject, int>>();
    private Dictionary<EBodyPart, int> m_BodyPartCollection = new Dictionary<EBodyPart, int>();

    private bool m_IsDead = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < (int)EBodyPart.COUNT; i++)
        {
            m_BodyPartCollection.Add((EBodyPart)i, 0);
        }

        m_ActiveBodyParts.Add(gameObject);

        AddNewBodyPart(m_SnakeBodyPart);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddNewBodyPart();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (m_ActiveBodyParts.Count >= 1)
                RemoveBodyPart(m_ActiveBodyParts[m_ActiveBodyParts.Count - 1]);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            RemoveRandomPart();
        }
    }

    public Dictionary<EBodyPart, int> GetPartCollection()
    {
        return m_BodyPartCollection;
    }

    public List<GameObject> GetActiveParts()
    {
        return m_ActiveBodyParts;
    }

    public void UpdateMovement(float InCurrentMoveSpeed)
    {
        UpdateBodyPartMovement(InCurrentMoveSpeed);
    }

    void UpdateBodyPartMovement(float InCurrentMoveSpeed)
    {
        for (int i = m_ActiveBodyParts.Count - 1; i >= 1; i--)
        {
            // Calculate direction towards the previous body part
            Vector2 direction = m_ActiveBodyParts[i - 1].transform.position - m_ActiveBodyParts[i].transform.position;
            direction.Normalize();

            // Calculate rotation towards the previous body part
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            m_ActiveBodyParts[i].transform.rotation = targetRotation;

            // Calculate the desired position for the current body part
            Vector2 targetPosition = m_ActiveBodyParts[i - 1].transform.position - (Vector3)direction * 0.7f;

            // Smoothly move the current body part towards the desired position using Lerp
            m_ActiveBodyParts[i].transform.position = Vector2.Lerp(m_ActiveBodyParts[i].transform.position, targetPosition, Time.deltaTime * InCurrentMoveSpeed);
        }
    }

    public void AddNewBodyPart()
    {
        GameObject lastBodyPart = m_ActiveBodyParts[m_ActiveBodyParts.Count - 1];

        GameObject newPart = Instantiate(m_SnakeBodyPart, lastBodyPart.transform.position - lastBodyPart.transform.up * 0.5f, Quaternion.identity);
        newPart?.GetComponent<SnakeBodyPartBase>().SetController(this);
        m_ActiveBodyParts.Add(newPart);
    }

    public void AddNewBodyPart(GameObject InNewBodyPart)
    {
        SnakeBodyPartBase partBase = InNewBodyPart.GetComponent<SnakeBodyPartBase>();
        if (partBase == null)
            return;

        EBodyPart newPartType = partBase.GetPart();

        if (HasPart(newPartType))
        {
            if (ShouldLevelUp(newPartType, out int newLevel))
            {
                GameObject newPartObject = ItemCollection.Instance.GetBodyPartWithLevel(newPartType, newLevel);

                if (newLevel == 2)
                    SoundManager.Instance.PlayGeneralSound(SFXType.PartLevelUp2, false);
                else if (newLevel == 3)
                    SoundManager.Instance.PlayGeneralSound(SFXType.PartLevelUp3, false);

                UpgradePart(newPartObject);
                m_BodyPartCollection[newPartType] += 1;
            }
            else
            {
                m_BodyPartCollection[newPartType] += 1;
            }

            UpdatePartStats();
            return;
        }

        GameObject lastBodyPart = m_ActiveBodyParts[m_ActiveBodyParts.Count - 1];

        GameObject newPart = Instantiate(InNewBodyPart, lastBodyPart.transform.position - lastBodyPart.transform.up * 0.5f, Quaternion.identity);
        newPart?.GetComponent<SnakeBodyPartBase>().SetController(this);
        m_ActiveBodyParts.Add(newPart);
        m_BodyPartCollection[newPartType] += 1;

        // Fix modifiers since new modifiers could have been added
        UpdatePartStats();
    }

    public void RemoveBodyPart(GameObject InPartToRemove)
    {
        if (m_ActiveBodyParts.Count > 1)
        {
            for (int i = 0; i < m_ActiveBodyParts.Count; i++)
            {
                GameObject part = m_ActiveBodyParts[i];
                if (part == InPartToRemove)
                {
                    SoundManager.Instance.PlayGeneralSound(SFXType.BodyPartDestroyed, false);
                    Instantiate(m_DestroyEffect, part.transform.position, Quaternion.identity);

                    m_ActiveBodyParts.Remove(part);
                    m_DeadBodyParts.Add(new Tuple<GameObject, int>(part, i));

                    part.SetActive(false);

                    if (m_ActiveBodyParts.Count <= 1)
                    {
                        Die();
                        return;
                    }

                    break;
                }
            }

            // Fix modifiers since new modifiers could have been removed
            UpdatePartStats();
        }
    }

    private void Die()
    {
        m_IsDead = true;
    }

    public bool IsDead()
    {
        return m_IsDead;
    }

    public void ResurectDeadBodyParts()
    {
        m_DeadBodyParts.Sort((x, y) => x.Item2.CompareTo(y.Item2));

        foreach (Tuple<GameObject, int> deadPart in m_DeadBodyParts)
        {
            m_ActiveBodyParts.Insert(deadPart.Item2, deadPart.Item1);

            deadPart.Item1.SetActive(true);

            HealthController healthController = deadPart.Item1.GetComponent<HealthController>();
        }

        foreach(GameObject part in m_ActiveBodyParts)
        {
            HealthController healthController = part.GetComponent<HealthController>();
            if (healthController == null)
                continue;

            healthController.FullyHeal();
        }

        m_DeadBodyParts.Clear();
    }

    void RemoveRandomPart()
    {
        if (m_ActiveBodyParts.Count > 1)
        {
            int randomIndex = Random.Range(1, m_ActiveBodyParts.Count);

            GameObject bodyPart = m_ActiveBodyParts[randomIndex];
            m_ActiveBodyParts.RemoveAt(randomIndex);
            Destroy(bodyPart);
        }
    }

    private void UpgradePart(GameObject InNewPart)
    {
        SnakeBodyPartBase newPartBase = InNewPart.GetComponent<SnakeBodyPartBase>();
        if (newPartBase == null)
        {
            Debug.LogError("Failed to get part: " + newPartBase.GetType().ToString());
            return;
        }

        for (int i = 0; i < m_ActiveBodyParts.Count; i++)
        {
            if (m_ActiveBodyParts[i].TryGetComponent<SnakeBodyPartBase>(out SnakeBodyPartBase activeBodyBase))
            {
                if (activeBodyBase.GetPart() == newPartBase.GetPart())
                {
                    GameObject newPart = Instantiate(InNewPart, m_ActiveBodyParts[i].transform.position, Quaternion.identity);
                    newPart?.GetComponent<SnakeBodyPartBase>().SetController(this);

                    GameObject bodyPartToRemove = m_ActiveBodyParts[i];
                    m_ActiveBodyParts.RemoveAt(i);
                    Destroy(bodyPartToRemove);

                    m_ActiveBodyParts.Insert(i, newPart);
                }
            }
        }
    }

    public void UpdatePartStats()
    {
        ModifierController.Instance.ReapplyModifiers();

        foreach (GameObject bodyPartObj in m_ActiveBodyParts)
        {
            if (bodyPartObj.TryGetComponent<SnakeBodyPartBase>(out SnakeBodyPartBase bodyPartBase))
            {
                bodyPartBase.UpdateStats();
            }
        }
    }

    private bool HasPart(EBodyPart InPartToCheck)
    {
        if (m_BodyPartCollection.TryGetValue(InPartToCheck, out int amount))
        {
            if (amount <= 0)
                return false;

            return true;
        }

        return false;
    }

    private bool ShouldLevelUp(EBodyPart InPartToCheck, out int OutLevel)
    {
        if (m_BodyPartCollection.TryGetValue(InPartToCheck, out int amount))
        {
            if (amount == 2)
            {
                OutLevel = 2;
                return true;
            }
            else if (amount == 8)
            {
                OutLevel = 3;
                return true;
            }
        }

        OutLevel = 1;
        return false;
    }
}
