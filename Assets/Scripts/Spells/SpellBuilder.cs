using System;
using System.Collections.Generic;

public static class SpellBuilder 
{
    //building the player's spells?
    private static Modifier CreateModifier(SpellCaster owner,ModifierInfo info, Spell inner)
    {
        return info.name switch
        {
            "doubler" => new doubler(owner, info, inner),
            "split" => new splitter(owner, info, inner),
            "damage-amplified" => new damageAmp(owner, info, inner),
            "speed-amplified" => new speedAmp(owner, info, inner),
            //"chaotic" => new chaotic(owner, info, inner),
            // need homing
        };
    }
    public static Spell Build(SpellCaster owner, Spell spell)
    {
        Spell freshCore = new Spell(owner, spell.spellInfo);
        return Build(freshCore, spell.GetModifiers());
    }
    
   public static Spell Build(Spell coreSpell, List<Modifier> spellModifiers)
   {
       Spell current = coreSpell;
       foreach (var modifier in spellModifiers)
       {
           modifier.inner = current;
           current = modifier;
       }
       return current;
   }
   // dont need to take in dictionary because in gamemanager
    public static Spell RandomSpell(SpellCaster owner, Spell coreSpell, Dictionary<string, ModifierInfo> availableModifiers) // takes in the game manager modifiers list
    {
        SpellCaster placeholder = new SpellCaster(-1, -1, Hittable.Team.PLAYER);
        Random rng = new Random();
        
        if (availableModifiers == null || availableModifiers.Count == 0)
            throw new ArgumentException("availableModifiers must not be null or empty.");
        
        int numberOfModifiers = 3;
        
        var keys = new List<string>(availableModifiers.Keys); // copy, don't mutate original
        List<Modifier> spellModifiers = new List<Modifier>();
        
        Spell current = coreSpell;
        for (int i = 0; i < numberOfModifiers; i++)
        {
            ModifierInfo info = availableModifiers[keys[rng.Next(0, keys.Count)]];
            Modifier mod = CreateModifier(owner, info, current);
            spellModifiers.Add(mod);
            current = mod;
        }
        return current;
    }
}
