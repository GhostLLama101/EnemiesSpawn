using Newtonsoft.Json.Linq;
using static RPNEvaluator.RPNEvaluator;
public class damageAmp : Modifier
{
    public damageAmp(SpellCaster owner, ModifierInfo spell, Spell inner) : base(owner, spell, inner)
    {
        
    }
    public override void SetAttributes(JObject mod)
    {
        base.SetAttributes(mod);
    
        this.ModifierInfo.damage_multiplier = mod["damage_multiplier"].ToString();
        this.ModifierInfo.mana_multiplier = mod["mana_multiplier"].ToString();
    }
    
    public override int GetDamage()
    {
        return (int)(inner.GetDamage() * EvaluateStat(ModifierInfo.damage_multiplier));
        
    }

    public override int GetManaCost()
    {
        return (int)(inner.GetManaCost() * EvaluateStat(ModifierInfo.mana_multiplier));
    }
    
    
}
