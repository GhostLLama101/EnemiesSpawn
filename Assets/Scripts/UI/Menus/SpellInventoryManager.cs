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
    public GameObject spell1;
    public GameObject spell2;
    public GameObject spell3;
    public GameObject spell4;
    private List<SpellInfo> playerSpells = new List<SpellInfo>();
    public Button nextButton;
    public Key triggerKey = Key.Escape;

    void Start()
    {
        screen.SetActive(false);
    }
    
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[triggerKey].wasPressedThisFrame)
        {
            screen.SetActive(true);
        }
    }

    public void OnEnable()
    {
        GameManager.Instance.state = GameManager.GameState.PAUSE;
        nextButton.onClick.AddListener(Next);

        if (GameManager.Instance.player == null) return;

        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();

        for (int i = 0; i < 4; i++)
        {
            playerSpells.Add(player.spellcaster.spells[i].spellInfo);
        }
        

        SpellIconManager sp_icon_man = GameManager.Instance.spellIconManager;
        if (sp_icon_man != null)
        {
            GameObject[] spellObjects = { spell1, spell2, spell3, spell4 };
            for (int i = 0; i < 4; i++)
            {
                int j = i;
                sp_icon_man.PlaceSprite(playerSpells[j].icon, spellObjects[j].GetComponent<Image>());
                //TODO: I think add the other things like mana and damage too

                Button button = spellObjects[j].GetComponent<Button>();
                button.onClick.RemoveAllListeners(); // clear before adding
                button.onClick.AddListener(() => {
                    foreach (GameObject o in spellObjects)
                    {
                        o.GetComponent<Button>().onClick.RemoveAllListeners(); //get rid of the listeners for the spells
                    }
                    GoToSpellTree(playerSpells[j]);
                });
            }
        }
    }
    public void OnDisable()
    {
        nextButton.onClick.RemoveListener(Next);
        GameManager.Instance.state = GameManager.GameState.INWAVE;
    }
    private void GoToSpellTree(SpellInfo spellInfo)
    {
        //TODO
        screen.SetActive(false);
        //enable the spell tree canvas and populate it with the selected spell's info
    }
    
    public void Next()
    {
        GameManager.Instance.enemySpawner.NextWave();
        screen.SetActive(false);
    }
    
}