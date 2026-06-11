using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine.InputSystem;

public class SpellInventoryManager : MonoBehaviour
{
    public GameObject screen;
    public GameObject EventSystem;
    public TextMeshProUGUI skillPointsText;
    public GameObject[] unlockButtons;
    public GameObject[] spellObjects;
    private List<SpellInfo> playerSpells = new List<SpellInfo>();
    public Button nextButton;
    public Key triggerKey = Key.Escape;
    GameManager.GameState lastState;
    public SpellUIContainer spellUIContainer;

    void Start()
    {
        screen.SetActive(false);
        nextButton.onClick.AddListener(Continue); 
    }
    
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.INWAVE || GameManager.Instance.state == GameManager.GameState.WAVEEND){
            if (Keyboard.current != null && Keyboard.current[triggerKey].wasPressedThisFrame && !screen.activeSelf)
            {
                lastState = GameManager.Instance.state;
                GameManager.Instance.state = GameManager.GameState.PAUSE;
                OpenInventory();
            }
        }
    }

    public void OpenInventory()
    {
        screen.SetActive(true);
        GameManager.Instance.state = GameManager.GameState.PAUSE;
        UpdateSkillPointsText();

        playerSpells.Clear();

        if (GameManager.Instance.player == null) return;

        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();

        for (int i = 0; i < 4; i++)
        {
            playerSpells.Add(GameManager.Instance.spells[i].spellInfo);
        }
        

        SpellIconManager sp_icon_man = GameManager.Instance.spellIconManager;
        if (sp_icon_man != null)
        {
            //spellObjects = new GameObject[] { spell1, spell2, spell3, spell4 };
            for (int i = 0; i < 4; i++)
            {
                SpellInfo spellData = playerSpells[i];
                int j = i;
                sp_icon_man.PlaceSprite(spellData.icon, spellObjects[j].transform.Find("spellicon").GetComponent<Image>());
                //TODO: I think update the other things like mana and damage too

                /* SpellUI manaUpdate = playerSpells[i];
                manaUpdate.SetSpell(manaUpdate);

                SpellUI dmgUpdate = playerSpells[i];
                dmgUpdate.SetSpell(dmgUpdate); */

                Button button = spellObjects[j].GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    GameManager.Instance.currentSpellSelected = j;
                    Debug.Log($"Selected spell: {spellData.name}");

                    GoToSpellTree(spellData, sp_icon_man, j);
                });
            }
        }
    }
    public void CloseInventory()
    {
        screen.SetActive(false);

        foreach (GameObject o in spellObjects)
        {
            o.GetComponent<Button>().onClick.RemoveAllListeners(); //get rid of the listeners for the spells
        }
        playerSpells.Clear();
        
    }
    private void GoToSpellTree(SpellInfo spellInfo, SpellIconManager spellIconManager, int spellIndex)
    {
        CloseInventory();
        EventSystem.GetComponent<SpellTreeManager>().OpenSpellTree(spellInfo, spellIconManager, spellIndex);
    }
    
    public void Continue()
    {
        GameManager.Instance.state = lastState;
        CloseInventory();
    }

    void PurchaseSpell(int cost, Spell spellName, int index)
    {
        if (GameManager.Instance.SkillPoints < cost)
        {
            return;
        }

        GameManager.Instance.RemoveSkillPoint(cost);

        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.spellcaster.AddSpell(spellName);
        Debug.Log($"Purchased Spell: {spellName}");
        // Update the spell UI slot for the purchased index
        if (spellUIContainer != null)
            spellUIContainer.AddSpell(index, spellName);
        else
            Debug.LogError("SpellUIContainer reference is missing on SpellInventoryManager!");
    }

    public void UnlockSpell(int index)
    {
        if (index < 0 || index > GameManager.Instance.spells.Count) return;

        int[] spellCosts = { 5, 10, 20 };

        int cost = spellCosts[index];

        if (GameManager.Instance.SkillPoints >= cost)
        {
            //PurchaseSpell(cost, GameManager.Instance.spells[index + 1]);
            PurchaseSpell(cost, GameManager.Instance.spells[index + 1], index + 1); // pass index + 1
            unlockButtons[index].SetActive(false);
            spellObjects[index + 1].SetActive(true);
        }

        UpdateSkillPointsText();
    }

    void UpdateSkillPointsText()
    {
        skillPointsText.text = $"Number of Skill Points: {GameManager.Instance.SkillPoints}";
    }
}