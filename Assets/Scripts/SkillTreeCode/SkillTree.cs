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
        string jsonString = File.ReadAllText(Application.dataPath + "/Resources/SkillTree.json");
        spellSlots = JsonConvert.DeserializeObject<List<SpellSlot>>(jsonString);
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
