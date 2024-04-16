using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUpScreen : MonoBehaviour
{
    [SerializeField] private Image m_BackgroundImage;
    [SerializeField] private GameObject m_ChosePowerUpText;

    [SerializeField] private UIPowerUpButton m_PowerUpButton1;
    [SerializeField] private UIPowerUpButton m_PowerUpButton2;

    private void Start()
    {
        Vector3 scale = new Vector3(0, 0, 0);
        m_PowerUpButton1.transform.localScale = scale;
        m_PowerUpButton2.transform.localScale = scale;

        m_ChosePowerUpText.transform.localScale = scale;
    }

    public void ShowPowerUpScreenSequence()
    {
        List<ModifierBase> randomModifiers = ItemCollection.Instance.GetRandomItemModifiers(2);

        Vector3 scale = new Vector3(0, 0, 0);
        m_PowerUpButton1.transform.localScale = scale;
        m_PowerUpButton2.transform.localScale = scale;
        m_ChosePowerUpText.transform.localScale = scale;

        m_PowerUpButton1.GetComponent<Button>().interactable = true;
        m_PowerUpButton2.GetComponent<Button>().interactable = true;

        m_PowerUpButton1.SetUpButton(randomModifiers[0]);
        m_PowerUpButton2.SetUpButton(randomModifiers[1]);

        StartCoroutine(ShowSequence());
    }

    public void HideButtons()
    {
        m_PowerUpButton1.GetComponent<Button>().interactable = false;
        m_PowerUpButton2.GetComponent<Button>().interactable = false;

        LeanTween.scale(m_PowerUpButton1.gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
        LeanTween.scale(m_PowerUpButton2.gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
    }

    IEnumerator ShowSequence()
    {
        // Get the original color of the background image
        Color originalColor = m_BackgroundImage.color;

        // Set the alpha of the background image to 0
        Color transparentColor = originalColor;
        transparentColor.a = 0f;
        m_BackgroundImage.color = transparentColor;

        // Fade in the background image
        float duration = 1f;
        LeanTween.value(gameObject, 0f, 0.5f, duration).setOnUpdate((float alpha) =>
        {
            Color newColor = m_BackgroundImage.color;
            newColor.a = alpha;
            m_BackgroundImage.color = newColor;
        }).setIgnoreTimeScale(true);

        // Wait until the background image is fully faded in
        yield return CoroutineUtil.WaitForRealSeconds(duration);

        // Pop out the buttons
        float popDuration = 0.5f;
        Vector3 scale = new Vector3(0, 0, 0);
        m_PowerUpButton1.transform.localScale = scale;
        m_PowerUpButton2.transform.localScale = scale;
        m_ChosePowerUpText.transform.localScale = scale;

        LeanTween.scale(m_ChosePowerUpText, Vector3.one, popDuration).setEaseOutElastic().setIgnoreTimeScale(true);

        yield return CoroutineUtil.WaitForRealSeconds(popDuration);

        LeanTween.scale(m_PowerUpButton1.gameObject, Vector3.one, popDuration).setEaseOutElastic().setIgnoreTimeScale(true);

        yield return CoroutineUtil.WaitForRealSeconds(popDuration);

        LeanTween.scale(m_PowerUpButton2.gameObject, Vector3.one, popDuration).setEaseOutElastic().setIgnoreTimeScale(true);
    }
}
