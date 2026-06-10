using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class SkillTree : MonoBehaviour
{
    private SpellCaster SpellCaster;
    
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



}
