using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPartInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_ClassText;
    [SerializeField] private TextMeshProUGUI m_DescriptionText;

    private void Start()
    {
        Hide();
    }

    public void Show(Vector2 InPostion, string InNameText, string InClassText, string InDescriptionText)
    {
        m_NameText.text = InNameText;
        m_ClassText.text = InClassText;
        m_DescriptionText.text = InDescriptionText;

        transform.localScale = new Vector3(0, 0, 1);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setEaseOutElastic().setIgnoreTimeScale(true);
    }

    public void Hide()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(0, 0, 1);
    }
}
