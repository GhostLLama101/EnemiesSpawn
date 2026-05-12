using UnityEngine;
using static RPNEvaluator.RPNEvaluator;
using Newtonsoft.Json.Linq;

public class speedAmp : Modifier
{
    public speedAmp(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }
    
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
        this.ModifierInfo.speed_multiplier= mod["speed_multiplier"].ToString();
    }
    
    public override float GetSpeed()
    {
        return inner.GetSpeed() * EvaluateStat(ModifierInfo.speed_multiplier);
    }
}
