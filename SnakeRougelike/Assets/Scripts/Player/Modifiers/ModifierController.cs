using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : ICloneable
{
    public float Mod_ActionCooldown = 1f;
    public float Mod_MaxHealth = 1f;

    public float Mod_ProjectileSpeed = 1f;
    public float Mod_Damage = 1f;
    public float Mod_AreaOfEffect = 1f;

    public int Mod_PierceAmount = 0;
    public float Mod_Range = 1;
    public int Mod_ProjectileAmount = 1;

    public float Mod_Spread = 1;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class ModifierController : MonoBehaviour
{
    private static ModifierController instance;

    public static ModifierController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Ensure only one instance of the WaveController exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Modifiers m_BaseModifiers = new Modifiers();
    private Modifiers m_CurrentModifiers = new Modifiers();

    private List<ModifierBase> m_ItemModifiers = new List<ModifierBase>();

    public void ReapplyModifiers()
    {
        GameObject player = GameManager.Instance.GetPlayer();
        if (player == null)
            return;

        SnakeBodyController bodyController = player.GetComponent<SnakeBodyController>();
        if (bodyController == null)
            return;

        List<GameObject> bodyParts = bodyController.GetActiveParts();
        if (bodyParts.Count <= 0)
            return;

        m_CurrentModifiers = (Modifiers)m_BaseModifiers.Clone();

        foreach (GameObject bodyPartObj in bodyParts)
        {
            if (bodyPartObj.TryGetComponent<ModifierBase>(out ModifierBase bodyPartBase))
            {
                bodyPartBase.ApplyModifier(m_CurrentModifiers);
            }
        }

        foreach (ModifierBase modifier in m_ItemModifiers)
        {
            modifier.ApplyModifier(m_CurrentModifiers);
        }
    }

    public void GiveItemModifier(ModifierBase InModifier)
    {
        GameObject newModifierObject = Instantiate(InModifier.gameObject, new Vector3(100000, 100000, 1), Quaternion.identity);
        m_ItemModifiers.Add(newModifierObject.GetComponent<ModifierBase>());
    }

    public Modifiers GetModifiers()
    {
        return m_CurrentModifiers;
    }
}
