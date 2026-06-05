using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SpellInfo
{
    public string name = "Arcane Bolt";
    public string description = "A straight-flying bolt.";
    public int icon = 0;
    public SpellDamage damage;
    public string mana_cost = "10";
    public string cooldown = "2";
    public Projectile projectile;
    public Projectile secondary_projectile;
    public List<ModifierInfo> modifiers = new List<ModifierInfo>();

    public SpellInfo Duplicate()
    {
        SpellInfo newSpell = new SpellInfo();
        
        newSpell.name = this.name;
        newSpell.description = this.description;
        newSpell.icon = this.icon;
        newSpell.damage = this.damage.Duplicate();
        newSpell.mana_cost = this.mana_cost;
        newSpell.cooldown = this.cooldown;
        newSpell.projectile = this.projectile.Duplicate();
        if (this.secondary_projectile != null)
            newSpell.secondary_projectile = this.secondary_projectile.Duplicate();
        newSpell.modifiers = new List<ModifierInfo>();
        for (int i = 0; i < this.modifiers.Count; i++)
        {
            newSpell.modifiers.Add(this.modifiers[i].Duplicate());
        }

        return newSpell;
    }
}