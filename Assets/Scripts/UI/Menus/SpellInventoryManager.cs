using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Random = System.Random;

public class SpellInventoryManager : MonoBehaviour
{
    public GameObject screen;
    public GameObject spell1;
    public GameObject spell2;
    public GameObject spell3;
    public GameObject spell4;
    private List<SpellInfo> playerSpells = new List<SpellInfo>();
    public Button nextButton;
    

    void Start()
    {
        //TODO
        //collect the player's spells
        //disable the canvas
    }

    public void OnEnable()
    {
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(Next);

        SpellIconManager sp_ico_man = GameManager.Instance.spellIconManager;
        if (sp_ico_man != null)
        {
            GameObject[] spellObjects = { spell1, spell2, spell3, spell4 };
            for (int i = 0; i < 3; i++)
            {
                int j = i;
                sp_ico_man.PlaceSprite(playerSpells[j].icon, spellObjects[j].GetComponent<Image>());
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
    private void GoToSpellTree(SpellInfo spellInfo)
    {
        //TODO
        //disable this canvas and its buttons
        //enable the spell tree canvas and populate it with the selected spell's info
    }

    public void Next()
    {
        nextButton.onClick.RemoveListener(Next);
        GameManager.Instance.enemySpawner.NextWave();
        screen.SetActive(false);
    }
    
}
