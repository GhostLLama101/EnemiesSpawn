using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadSkillTree : MonoBehaviour
{
    public List<SpellSlot> spellSlots;
    public SpellCaster spellCaster;
    public Modifier Modifiers;
    void Start()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/SkillTree.json");
        spellSlots = JsonConvert.DeserializeObject<List<SpellSlot>>(jsonString);
    }

    void PurchaseSpell(int cost, string spellName)
    {
        if (GameManager.Instance.SkillPoints < cost)
        {
            return;
        }

        GameManager.Instance.SkillPoints -= cost;

        // SpellCaster.AddSpell(Spell spellName); 
        // this needs to add the spell to a list of unlocked & accessible spells. swapping spells with keybinds needs an additional condition of being unlocked.
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
