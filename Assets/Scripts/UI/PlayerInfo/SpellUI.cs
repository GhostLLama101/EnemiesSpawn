using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUI : MonoBehaviour
{
    public GameObject icon;
    public RectTransform cooldown;
    public TextMeshProUGUI manacost;
    public TextMeshProUGUI damage;
    public GameObject highlight;
    public Spell spell;
    float last_text_update;
    const float UPDATE_DELAY = 1;
    //public GameObject dropbutton;
    public TextMeshProUGUI spellName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        last_text_update = 0;
        
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        Debug.Log($"icon index: {spell.GetIcon()}, iconManager: {GameManager.Instance.spellIconManager}, icon: {icon}");
        GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
    
        manacost.text = spell.GetManaCost().ToString();
        damage.text = spell.GetDamage().ToString();
        if (spellName != null) spellName.text = spell.GetName(); 
        last_text_update = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   
        if (GameManager.Instance.state == GameManager.GameState.INWAVE) 
        {
            if (spell == null) return;
            if (Time.time > last_text_update + UPDATE_DELAY)
            {
                manacost.text = spell.GetManaCost().ToString();
                damage.text = spell.GetDamage().ToString();
                last_text_update = Time.time;
            }
            
            float since_last = Time.time - spell.last_cast;
            float perc;
            if (since_last > spell.GetCooldown())
            {
                perc = 0;
            }
            else
            {
                perc = 1-since_last / spell.GetCooldown();
            }
            cooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48 * perc);
        }
    }
    public void SetClickable(bool on, Action onClick)
    {
        Button btn = GetComponent<Button>();
        if (btn == null) btn = gameObject.AddComponent<Button>();
    
        // Button needs a raycast target to receive clicks
        Image img = GetComponent<Image>();
        if (img == null)
        {
            img = gameObject.AddComponent<Image>();
            img.color = new Color(1, 1, 1, 0); // invisible but clickable
        }
        img.raycastTarget = on;

        btn.onClick.RemoveAllListeners();
        if (on && onClick != null)
            btn.onClick.AddListener(() => onClick());
        btn.enabled = on;
    }
}
