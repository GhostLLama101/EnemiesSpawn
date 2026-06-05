using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellTreeManager : MonoBehaviour
{
    public GameObject EventSystem;
    public GameObject spellTreeCanvas;
    public TMP_Text spellModifiersText;
    public Image spellIconImage;
    public Button backButton;
    public Button acceptButton;

    public void Start()
    {
        spellTreeCanvas.SetActive(false);
        backButton.onClick.AddListener(CloseSpellTree);
    }
    public void OpenSpellTree(SpellInfo spellInfo, SpellIconManager sp_icon_man, int spellIndex)
    {
        Debug.Log("Opening spell tree for spell " + spellIndex);
        GameManager.Instance.state = GameManager.GameState.PAUSE;
        spellTreeCanvas.SetActive(true);
        //spellModifiersText.text = spellInfo.modifiers;
        sp_icon_man.PlaceSprite(spellInfo.icon, spellIconImage.GetComponent<Image>());
    }

    private void CloseSpellTree()
    {
        spellTreeCanvas.SetActive(false);
        EventSystem.GetComponent<SpellInventoryManager>().OpenInventory();
    }
}