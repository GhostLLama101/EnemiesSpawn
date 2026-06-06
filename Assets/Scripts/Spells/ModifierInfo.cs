using System;
using UnityEngine;

[Serializable]
public class ModifierInfo 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string name;
    public string description;
    public string damage_multiplier;
    public string mana_multiplier;
    public float delay;
    public string cooldown_multiplier;
    public string speed_multiplier;
    public string angle;
    public string projectile_trajectory;
    public string mana_adder;
    public string pierce;
    public int count;

    public ModifierInfo Duplicate()
    {
        ModifierInfo newModifier = new ModifierInfo ();
    
        newModifier.name = this.name;
        newModifier.description = this.description;
        newModifier.damage_multiplier = this.damage_multiplier;
        newModifier.mana_multiplier = this.mana_multiplier;
        newModifier.delay = this.delay;
        newModifier.cooldown_multiplier = this.cooldown_multiplier;
        newModifier.speed_multiplier = this.speed_multiplier;
        newModifier.angle = this.angle;
        newModifier.projectile_trajectory = this.projectile_trajectory;
        newModifier.mana_adder = this.mana_adder;
        newModifier.pierce = this.pierce;
        newModifier.count = this.count;
        return newModifier;
    }
}
