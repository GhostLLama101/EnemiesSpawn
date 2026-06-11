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
    private Modifier Modifiers;
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
        for (int i = 0; i < spellModifiersText.Length; i++)
        {
            foreach (var kvp in GameManager.Instance.ModDict)
            {
                ModifierInfo modifier_text = kvp.Value;
                spellModifiersText[i].text = "Modifier " + (i + 1) + modifier_text.description;
                Debug.Log($"modifier text: " + modifier_text.description);
            }
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

    public void ModiferClicked(int modifierIndex)
    {
        modifiersPurchased[GameManager.Instance.currentSpellSelected][modifierIndex]++;

        modifierButtons[modifierIndex].SetActive(false);

        SoundManager.instance.PlaySoundClip(SoundManager.instance.selectSound, transform);
    }

    private void UpdatePurchasedModifiers()
    {
        for (int i = 0; i < 3; i++)
        {
            modifierButtons[i].SetActive(false);

            if (modifiersPurchased[GameManager.Instance.currentSpellSelected][i] == 1)
            {
                modifierButtons[i].SetActive(true);
            }
        }
    }
}