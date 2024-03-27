using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPurchasePart : UIPurchaseButtonBase
{
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_CostText;
    [SerializeField] private UIPartInfo m_PartInfo;

    private SnakeBodyPartSO m_CurrentSnakePart;

    private bool m_ItemBought = false;

    protected override void Purchase()
    {
        if (m_ItemBought == true)
            return;

        base.Purchase();

        GameObject player = GameManager.Instance.GetPlayer();
        if (player == null)
            return;

        SnakeBodyController bodyController = player?.GetComponent<SnakeBodyController>();

        bodyController.AddNewBodyPart(m_CurrentSnakePart.Lvl1BodyPartPrefab);
        GameManager.Instance.GetUpgradeScreen().UpdateSnakeVisualiser();
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.4f).setEaseOutExpo().setIgnoreTimeScale(true);
        m_ItemBought = true;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (m_ItemBought == true)
            return;

        base.OnPointerEnter(eventData);

        if (m_PartInfo == null)
            return;

        SnakeBodyPartBase bodyPartbase = m_CurrentSnakePart.Lvl1BodyPartPrefab.GetComponent<SnakeBodyPartBase>();
        if (bodyPartbase == null)
            return;

        List<EBodyPartClass> classes = bodyPartbase.GetClasses();
        string classString = "";
        foreach(EBodyPartClass classEnum in classes)
        {
            classString += classEnum.ToString();
        }

        string fixedDescription = m_CurrentSnakePart.Description.Replace("@", "\n");

        m_PartInfo.Show(transform.localPosition, m_CurrentSnakePart.Name, classString, fixedDescription);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (m_ItemBought == true)
        {
            m_PartInfo.Hide();
            return;
        }

        base.OnPointerExit(eventData);
        if (m_PartInfo == null)
            return;

        m_PartInfo.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNewPurchaseOption();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewPurchaseOption()
    {
        transform.localScale = new Vector3(0, 0, 1);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 1f).setEaseOutElastic().setIgnoreTimeScale(true);

        m_CurrentSnakePart = ItemCollection.Instance.GetRandomSnakeBodyPart();

        m_NameText.text = m_CurrentSnakePart.Name;
        m_CostText.text = m_CurrentSnakePart.Cost.ToString();

        m_Cost = m_CurrentSnakePart.Cost;
        m_ItemBought = false;
    }
}
