using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    // Start is called before the first frame update

    private TextMeshPro m_Text;

    [SerializeField] float m_TimeAlive = 0.5f;

    bool m_ShouldRemove = false;

    void Start()
    {
        
    }

    public void SetupText(int InDamageAmount)
    {
        m_Text = gameObject.GetComponent<TextMeshPro>();
        transform.localScale = new Vector3(0, 0, 1);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setEaseOutElastic();
        LeanTween.moveY(gameObject, transform.position.y + 0.5f, 0.8f).setEaseOutElastic();
        
        m_Text.text = InDamageAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ShouldRemove)
            return;

        if(m_TimeAlive <= 0)
        {
            LeanTween.scale(gameObject, new Vector3(0, 0, 1), 0.3f).setEaseOutCubic().setOnComplete(DestroyText);
            m_ShouldRemove = true;
        }
        else
        {
            m_TimeAlive -= Time.deltaTime;
        }
    }

    void DestroyText()
    {
        Destroy(gameObject);
    }
}
