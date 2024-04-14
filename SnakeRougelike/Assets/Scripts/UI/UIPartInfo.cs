using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPartInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_DescriptionText;
    [SerializeField] private TextMeshProUGUI m_ModifierDescription;

    private void Start()
    {
        Hide();
    }

    public void Show(Vector2 InPostion, string InNameText, string InDescriptionText, string InModifierDescription)
    {
        m_NameText.text = InNameText;
        m_DescriptionText.text = InDescriptionText;
        m_ModifierDescription.text = InModifierDescription;

        transform.localScale = new Vector3(0, 0, 1);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setEaseOutElastic().setIgnoreTimeScale(true);
    }

    public void Hide()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(0, 0, 1);
    }
}
