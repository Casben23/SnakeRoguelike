 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPowerUpButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_DescriptionText;

    private ModifierBase m_CurrentModifier;

    public void SetUpButton(ModifierBase InModifier)
    {
        m_NameText.text = InModifier.GetModifierName();
        m_DescriptionText.text = InModifier.GetModifierDescription();

        m_CurrentModifier = InModifier;
    }

    public void ChosePowerUp()
    {
        ModifierController.Instance.GiveItemModifier(m_CurrentModifier);
        GameManager.Instance.OnPowerUpChosen();
        ItemCollection.Instance.RemoveItemFromCollection(m_CurrentModifier);

        GameObject player = GameManager.Instance.GetPlayer();
        if (player == null)
            return;

        SnakeBodyController bodyController = player.GetComponent<SnakeBodyController>();
        if (bodyController == null)
            return;

        bodyController.UpdatePartStats();

        UINextLevelScreen.Instance.AddNewPowerupVisualiser(m_CurrentModifier);
    }
}
