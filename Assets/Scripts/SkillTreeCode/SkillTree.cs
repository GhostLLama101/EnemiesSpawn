using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public List<SpellSlot> spellSlots;
    public SpellCaster SpellCaster;
    public Modifier Modifiers;
    void Start()
    {
        // for debuging
        Debug.Log("checking for loaded stuff in skill tree");
        if (GameManager.Instance.SkillTreeMods != null)
        {
            foreach (var kvp in GameManager.Instance.SkillTreeMods)
            {
                SpellSlot skill_slot_info = kvp.Value;
                Debug.Log("slot: " + skill_slot_info.spell_slot + " sprite: " + skill_slot_info.sprite + " cost: " + skill_slot_info.spell_cost);
                foreach (var modifier in skill_slot_info.available_modifiers)
                {
                    Debug.Log("modifier: " + modifier.name + " cost: " + modifier.cost);
                }
            }
        }
        else
        {
            Debug.Log("not loaded skill tree");
        }
    }

    void PurchaseModifier(int cost, string modifierName)
    {
        if (GameManager.Instance.SkillPoints < cost)
        {
            return;
        }

        GameManager.Instance.SkillPoints -= cost;

        Modifiers.AddModifier(GameManager.Instance.player.GetComponent<PlayerController>().spellcaster, GameManager.Instance.spells[GameManager.Instance.currentSpellSelected], modifierName);
    }

    public void Modifier1Clicked()
    {
        Debug.Log("Modifier1Clicked");
    }

    public void Modifier2Clicked()
    {
        Debug.Log("Modifier2Clicked");
    }

    public void Modifier3Clicked()
    {
        Debug.Log("Modifier3Clicked");
    }
}
