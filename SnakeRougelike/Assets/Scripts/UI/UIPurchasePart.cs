using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPurchasePart : UIPurchaseButtonBase
{
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_CostText;

    private SnakeBodyPartSO m_CurrentSnakePart;

    protected override void Purchase()
    {
        base.Purchase();

        GameObject player = GameManager.Instance.GetPlayer();
        if (player == null)
            return;

        SnakeBodyController bodyController = player?.GetComponent<SnakeBodyController>();

        bodyController.AddNewBodyPart(m_CurrentSnakePart.Lvl1BodyPartPrefab);
        GameManager.Instance.GetUpgradeScreen().UpdateSnakeVisualiser();
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.4f).setEaseOutExpo().setIgnoreTimeScale(true).setOnComplete(SetNewPurchaseOption);
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

    void SetNewPurchaseOption()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.8f).setEaseOutElastic().setIgnoreTimeScale(true);

        m_CurrentSnakePart = ItemCollection.Instance.GetRandomSnakeBodyPart();

        m_NameText.text = m_CurrentSnakePart.Name;
        m_CostText.text = m_CurrentSnakePart.Cost.ToString();

        m_Cost = m_CurrentSnakePart.Cost;
    }
}
