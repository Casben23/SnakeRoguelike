using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISnakeVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject m_PartVisualserPrefab;

    private EBodyPart m_CurrentPart;
    private int m_CurrentAmount = 0;

    private List<GameObject> m_PartVisualisers = new List<GameObject>();

    public EBodyPart GetCurrentPart()
    {
        return m_CurrentPart;
    }

    public int GetCurrentAmount()
    {
        return m_CurrentAmount;
    }

    public void SetVisualiser(EBodyPart InBodyPart, int InAmount)
    {
        m_CurrentPart = InBodyPart;

        UpdateAmount(InAmount);
    }

    public void UpdateAmount(int InAmount)
    {
        if (InAmount == m_CurrentAmount)
            return;

        foreach(GameObject visualiser in m_PartVisualisers)
        {
            Destroy(visualiser);
        }

        m_PartVisualisers.Clear();

        int level3Count = (InAmount / 9) % 3;
        int level2Count = (InAmount / 3) % 3;
        int level1Count = InAmount % 3;

        Vector2 currentLocalPosition = Vector2.zero;
        for (int i = 0; i < level3Count; i++)
        {
            GameObject newPartVisualiser = Instantiate(m_PartVisualserPrefab, this.transform);
            newPartVisualiser.transform.localPosition = currentLocalPosition;
            newPartVisualiser.transform.localScale = new Vector3(0, 0, 1);
            LeanTween.scale(newPartVisualiser, new Vector3(1, 1, 1), 0.6f).setEaseOutElastic().setIgnoreTimeScale(true);
            currentLocalPosition -= new Vector2(0, 105);

            UpdatePartGFX(newPartVisualiser, 3);

            m_PartVisualisers.Add(newPartVisualiser);
        }

        for (int i = 0; i < level2Count; i++)
        {
            GameObject newPartVisualiser = Instantiate(m_PartVisualserPrefab, this.transform);
            newPartVisualiser.transform.localPosition = currentLocalPosition;
            newPartVisualiser.transform.localScale = new Vector3(0, 0, 1);
            LeanTween.scale(newPartVisualiser, new Vector3(1, 1, 1), 0.4f).setEaseOutElastic().setIgnoreTimeScale(true);
            currentLocalPosition -= new Vector2(0, 105);

            UpdatePartGFX(newPartVisualiser, 2);

            m_PartVisualisers.Add(newPartVisualiser);
        }

        for (int i = 0; i < level1Count; i++)
        {
            GameObject newPartVisualiser = Instantiate(m_PartVisualserPrefab, this.transform);
            newPartVisualiser.transform.localPosition = currentLocalPosition;
            newPartVisualiser.transform.localScale = new Vector3(0, 0, 1);
            LeanTween.scale(newPartVisualiser, new Vector3(1, 1, 1), 0.2f).setEaseOutElastic().setIgnoreTimeScale(true);
            currentLocalPosition -= new Vector2(0, 105);

            UpdatePartGFX(newPartVisualiser, 1);

            m_PartVisualisers.Add(newPartVisualiser);
        }

        m_CurrentAmount = InAmount;
    }

    private void UpdatePartGFX(GameObject InPartVisualiser, int InLevel)
    {
        if (InPartVisualiser.TryGetComponent<Image>(out Image imageComp))
        {
            Sprite partSprite = ItemCollection.Instance.GetPartSprite(m_CurrentPart);
            imageComp.sprite = partSprite;
        }


        TextMeshProUGUI textComp = InPartVisualiser.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp == null)
            return;

        textComp.text = InLevel.ToString();
    }

    private void Start()
    {
    }
}
