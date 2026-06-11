using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // we only have one spell (right now)
        foreach (GameObject spellUI in spellUIs)
        {
            spellUI.SetActive(true);
        }
    }
    public void AddSpell(int index, Spell spell) // this sets the spell UI
    {
        Debug.Log($"Adding Spell: {spell.GetName()}, Index: {index}");
        if (index < 0 || index >= spellUIs.Length) return;

        SpellUI ui = spellUIs[index].GetComponent<SpellUI>();
        if (ui != null)
            ui.SetSpell(spell);
    }
    
    public void SetActiveSlot(int index)
    {
        for (int i = 0; i < spellUIs.Length; i++)
        {
            SpellUI ui = spellUIs[i].GetComponent<SpellUI>();
            if (ui != null)
                ui.highlight.SetActive(i == index);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
