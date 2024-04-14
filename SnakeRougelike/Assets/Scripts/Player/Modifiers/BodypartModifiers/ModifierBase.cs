using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBase : MonoBehaviour
{
    [SerializeField] protected string m_ModifierName = "";

    [TextArea]
    [SerializeField] protected string m_ModifierDescription = "";

    public virtual void ApplyModifier(Modifiers InModifiers) { }
    public virtual string GetModifierName() { return m_ModifierName; }
    public virtual string GetModifierDescription() { return m_ModifierDescription; }
}
