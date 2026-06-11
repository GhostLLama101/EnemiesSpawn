using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellTreeManager : MonoBehaviour
{
    public GameObject EventSystem;
    public GameObject spellTreeCanvas;
    public TMP_Text[] spellModifiersText;
    public GameObject[] modifierButtons;
    public Image spellIconImage;
    public Button backButton;
    public Button acceptButton;

    public int[][] modifiersPurchased = new int[4][] 
    {
        new int[] { 0, 0, 0 },
        new int[] { 0, 0, 0 },
        new int[] { 0, 0, 0 },
        new int[] { 0, 0, 0 }
    };

    public void Start()
    {
        spellTreeCanvas.SetActive(false);
        backButton.onClick.AddListener(CloseSpellTree);

    }
    public void OpenSpellTree(SpellInfo spellInfo, SpellIconManager sp_icon_man, int spellIndex)
    {
        GameManager.Instance.state = GameManager.GameState.PAUSE;
        spellTreeCanvas.SetActive(true);
        UpdateModifierText();
        UpdatePurchasedModifiers();
        //spellModifiersText.text = spellInfo.modifiers;
        sp_icon_man.PlaceSprite(spellInfo.icon, spellIconImage.GetComponent<Image>());
    }

    private void CloseSpellTree()
    {
        spellTreeCanvas.SetActive(false);
        EventSystem.GetComponent<SpellInventoryManager>().OpenInventory();
    }

    private void UpdateModifierText() 
    {
        if (GameManager.Instance.SkillTreeMods == null)
        {
            return;
        }

        for (int i = 0; i < spellModifiersText.Length; i++) {

            foreach (var kvp in GameManager.Instance.SkillTreeMods)
            {
                SpellSlot skill_slot_info = kvp.Value;
                if (skill_slot_info.spell_slot == GameManager.Instance.currentSpellSelected + 1)
                {
                    spellModifiersText[i].text = "Modifier: " + skill_slot_info.available_modifiers[i].name + 
                    "\nCost = " + skill_slot_info.available_modifiers[i].cost.ToString() + " Skill Points";
                }
            }

        }
    }

    private void PurchaseModifier(int cost, string modifierName)
    {
        GameManager.Instance.SkillPoints -= cost;

        int index = GameManager.Instance.currentSpellSelected;
        SpellCaster spellcaster = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster;

        GameManager.Instance.spells[index] = Modifier.AddModifier(
            spellcaster,
            GameManager.Instance.spells[index],
            modifierName
        );
        
    }

    public void ModiferClicked(int modifierIndex)
    {
        
        if (GameManager.Instance.SkillTreeMods == null)
        {
            return;
        }

        foreach (var kvp in GameManager.Instance.SkillTreeMods)
        {
            SpellSlot skill_slot_info = kvp.Value;
            if (skill_slot_info.spell_slot == GameManager.Instance.currentSpellSelected + 1)
            {
                if (GameManager.Instance.SkillPoints < skill_slot_info.available_modifiers[modifierIndex].cost)
                {
                    return;
                }
                Debug.Log("cost: " + skill_slot_info.available_modifiers[modifierIndex].cost + "name: " + skill_slot_info.available_modifiers[modifierIndex].name);
                PurchaseModifier(skill_slot_info.available_modifiers[modifierIndex].cost, skill_slot_info.available_modifiers[modifierIndex].name);
            }
        }

        modifiersPurchased[GameManager.Instance.currentSpellSelected][modifierIndex] = 1;

        modifierButtons[modifierIndex].SetActive(false);

        SoundManager.instance.PlaySoundClip(SoundManager.instance.selectSound, transform);
    }

    private void UpdatePurchasedModifiers()
    {
        for (int i = 0; i < 3; i++)
        {
            modifierButtons[i].SetActive(true);

            if (modifiersPurchased[GameManager.Instance.currentSpellSelected][i] == 1)
            {
                modifierButtons[i].SetActive(false);
            }
        }
    }
}