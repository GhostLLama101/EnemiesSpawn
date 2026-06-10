using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadSkillTree : MonoBehaviour
{
    public List<SpellSlot> spellSlots;
    public SpellCaster SpellCaster;
    public Modifier Modifiers;
    void Start()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/SkillTree.json");
        spellSlots = JsonConvert.DeserializeObject<List<SpellSlot>>(jsonString);
    }

    void PurchaseSpell(int cost, Spell spellName)
    {
        if (GameManager.Instance.SkillPoints < cost)
        {
            return;
        }

        GameManager.Instance.SkillPoints -= cost;

        GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.AddSpell(spellName);
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
}
