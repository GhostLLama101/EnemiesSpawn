using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;
using static RPNEvaluator.RPNEvaluator;
public class homing : Modifier
{

    
    

    public homing(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        ModifierInfo.name = "homing";
        ModifierInfo.description = 
        "The spell's projectile now homes in on the nearest target. Damage is decreased. Mana cost is increased.";
        ModifierInfo.damage_multiplier = "0.9";
        ModifierInfo.mana_adder = "5";
        ModifierInfo.projectile_trajectory = "homing";
    }
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        //add 5 to mana cost
        //decrease mana cost by 10%
        //make tragectory of projectile homing
        //this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.mana_multiplier = mod["mana_multiplier"].ToString();
    }
    
    // need to add an edit values for when the player gets stronger
}
