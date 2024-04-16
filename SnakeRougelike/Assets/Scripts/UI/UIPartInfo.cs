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

    public void ShowPartInfo(SnakeBodyPartSO InSnakePartSO, int InLevel)
    {
        string modifierDescription = "";
        if (InSnakePartSO.Lvl3BodyPartPrefab.TryGetComponent<ModifierBase>(out ModifierBase modBase))
        {
            modifierDescription = "Level 3 Modifier: " + modBase.GetModifierDescription();
        }

        WeaponPartStats level1PartStats = InSnakePartSO.Lvl1BodyPartPrefab.GetComponent<SnakeBodyPartBase>().GetWeaponPartBaseStats();
        WeaponPartStats level2PartStats = InSnakePartSO.Lvl2BodyPartPrefab.GetComponent<SnakeBodyPartBase>().GetWeaponPartBaseStats();
        WeaponPartStats level3PartStats = InSnakePartSO.Lvl3BodyPartPrefab.GetComponent<SnakeBodyPartBase>().GetWeaponPartBaseStats();

        WeaponPartStats level1PartStatsModified = InSnakePartSO.Lvl1BodyPartPrefab.GetComponent<SnakeBodyPartBase>().GetModifiedStatClone();
        WeaponPartStats level2PartStatsModified = InSnakePartSO.Lvl2BodyPartPrefab.GetComponent<SnakeBodyPartBase>().GetModifiedStatClone();
        WeaponPartStats level3PartStatsModified = InSnakePartSO.Lvl3BodyPartPrefab.GetComponent<SnakeBodyPartBase>().GetModifiedStatClone();

        string baseDamageDescription = "";
        string baseProjectileAmountDescription = "";

        string damageDescription = "";
        string projectileAmountDescription = "";

        switch (InLevel)
        {
            case 1:
                baseDamageDescription = "<style=BD> (base: " + level1PartStats.Damage.ToString() + "/" + level2PartStats.Damage.ToString() + "/" + level3PartStats.Damage.ToString() + ")</style>";
                baseProjectileAmountDescription = "<style=BD> (base: " + level1PartStats.ProjectileAmount.ToString() + "/" + level2PartStats.ProjectileAmount.ToString() + "/" + level3PartStats.ProjectileAmount.ToString() + ")</style>";
                damageDescription = "<style=SD>" + level1PartStatsModified.Damage.ToString() + "</style>/" + level2PartStatsModified.Damage.ToString() + "/" + level3PartStatsModified.Damage.ToString();
                projectileAmountDescription = "<style=SD>" + level1PartStatsModified.ProjectileAmount.ToString() + "</style>/" + level2PartStatsModified.ProjectileAmount.ToString() + "/" + level3PartStatsModified.ProjectileAmount.ToString();
                break;
            case 2:
                baseDamageDescription = "<style=BD> (base: " + level1PartStats.Damage.ToString() + "/" + level2PartStats.Damage.ToString() + "/" + level3PartStats.Damage.ToString() + ")</style>";
                baseProjectileAmountDescription = "<style=BD> (base: " + level1PartStats.ProjectileAmount.ToString() + "/" + level2PartStats.ProjectileAmount.ToString() + "/" + level3PartStats.ProjectileAmount.ToString() + ")</style>";
                damageDescription = level1PartStatsModified.Damage.ToString() + "/<style=SD>" + level2PartStatsModified.Damage.ToString() + "</style>/" + level3PartStatsModified.Damage.ToString();
                projectileAmountDescription = level1PartStatsModified.ProjectileAmount.ToString() + "/<style=SD>" + level2PartStatsModified.ProjectileAmount.ToString() + "</style>/" + level3PartStatsModified.ProjectileAmount.ToString();
                break;
            case 3:
                baseDamageDescription = "<style=BD> (base: " + level1PartStats.Damage.ToString() + "/" + level2PartStats.Damage.ToString() + "/" + level3PartStats.Damage.ToString() + ")</style>";
                baseProjectileAmountDescription = "<style=BD> (base: " + level1PartStats.ProjectileAmount.ToString() + "/" + level2PartStats.ProjectileAmount.ToString() + "/" + level3PartStats.ProjectileAmount.ToString() + ")</style>";
                damageDescription = level1PartStatsModified.Damage.ToString() + "/" + level2PartStatsModified.Damage.ToString() + "/<style=SD>" + level3PartStatsModified.Damage.ToString() + "</style>";
                projectileAmountDescription = level1PartStatsModified.ProjectileAmount.ToString() + "/" + level2PartStatsModified.ProjectileAmount.ToString() + "/<style=SD>" + level3PartStatsModified.ProjectileAmount.ToString() + "</style>";
                break;
        }

        damageDescription += baseDamageDescription;
        projectileAmountDescription += baseProjectileAmountDescription;

        string fixedDescriptionDamage = InSnakePartSO.Description.Replace("@D", damageDescription);
        string finalfixedDescription = fixedDescriptionDamage.Replace("@PA", projectileAmountDescription);

        m_NameText.text = InSnakePartSO.Name;
        m_DescriptionText.text = finalfixedDescription;
        m_ModifierDescription.text = modifierDescription;

        transform.localScale = new Vector3(0, 1, 1);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.2f).setEaseOutCubic().setIgnoreTimeScale(true);
    }

    public void ShowCustom(string InName, string InDescription, string InModifierDescription)
    {
        m_NameText.text = InName;
        m_DescriptionText.text = InDescription;
        m_ModifierDescription.text = InModifierDescription;

        transform.localScale = new Vector3(0, 1, 1);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.2f).setEaseOutCubic().setIgnoreTimeScale(true);
    }

    public void Hide()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(0, 0, 1);
    }
}
