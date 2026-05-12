using UnityEngine;
using Newtonsoft.Json.Linq;
public class homing : Modifier
{
    public homing(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }
    //TODO: Actually make this set the attributes
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        //multiply mana cost by x1.25
        //make tragectory of projectile homing
        //this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.mana_multiplier = mod["mana_multiplier"].ToString();
    }
    
    // need to add an edit values for when the player gets stronger
}
