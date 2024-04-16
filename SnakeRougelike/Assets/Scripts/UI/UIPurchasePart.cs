using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPurchasePart : UIPurchaseButtonBase
{
    [SerializeField] private UIPurchasePannel m_PurchasePannel;
    [SerializeField] private Image m_LevelVisualizer;
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_CostText;
    [SerializeField] private UIPartInfo m_PartInfo;

    [SerializeField] private List<Sprite> m_LevelStartSprites;

    private SnakeBodyPartSO m_CurrentSnakePart;

    private int m_PurchaseLevel = 1;
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
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.4f).setEaseOutExpo().setIgnoreTimeScale(true);
        m_ItemBought = true;
        m_PurchasePannel.UpdateStarVisualizers();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (m_ItemBought == true)
            return;

        base.OnPointerEnter(eventData);

        if (m_PartInfo == null)
            return;

        m_PartInfo.ShowPartInfo(m_CurrentSnakePart, m_PurchaseLevel);
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

    public void SetNewPurchaseOption()
    {
        transform.localScale = new Vector3(0, 0, 1);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.7f).setEaseOutElastic().setIgnoreTimeScale(true);

        m_CurrentSnakePart = ItemCollection.Instance.GetRandomSnakeBodyPart();

        m_NameText.text = m_CurrentSnakePart.Name;
        m_CostText.text = m_CurrentSnakePart.Cost.ToString() + "$";

        m_Cost = m_CurrentSnakePart.Cost;
        m_ItemBought = false;

        Sprite starSprite = GetStarToShow();

        m_LevelVisualizer.sprite = starSprite;
    }

    private Sprite GetStarToShow()
    {
        GameObject player = GameManager.Instance.GetPlayer();
        if (player == null)
            return null;

        SnakeBodyController bodyController = player.GetComponent<SnakeBodyController>();
        if (bodyController == null)
            return null;

        Dictionary<EBodyPart, int> partCollection = bodyController.GetPartCollection();
        int amountOfPart = partCollection[m_CurrentSnakePart.Part];

        int level2Count = (amountOfPart / 3) % 3;
        int level1Count = amountOfPart % 3;

        if (level1Count == 2 && level2Count == 2)
        {
            m_PurchaseLevel = 3;
            return m_LevelStartSprites[2];
        }

        if (level1Count == 2)
        {
            m_PurchaseLevel = 2;
            return m_LevelStartSprites[1];
        }

        m_PurchaseLevel = 1;
        return m_LevelStartSprites[0];
    }

    public void UpdateStar()
    {
        Sprite starSprite = GetStarToShow();
        m_LevelVisualizer.sprite = starSprite;
    }
}
