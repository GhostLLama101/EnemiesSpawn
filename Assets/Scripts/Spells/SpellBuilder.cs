using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class SpellBuilder 
{
    //building the player's spells?
    public static Modifier CreateModifier(SpellCaster owner,ModifierInfo info, Spell inner)
    {
        return info.name switch
        {
            "doubled" => new doubler(owner, info, inner),
            "split" => new splitter(owner, info, inner),
            "damage-amplified" => new damageAmp(owner, info, inner),
            "speed-amplified" => new speedAmp(owner, info, inner),
            "swifty" => new Budget(owner, info, inner),
            "lance" => new lance(owner, info, inner),
            "chaotic" => new chaotic(owner, info, inner),
            "homing" => new homing(owner, info, inner),
            "crawlingMine" => new crawlingMine(owner, info, inner),
            _ => null
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
    public static Spell RandomSpell(SpellCaster placeholder = null)
    {
        if (placeholder == null)
        {
            placeholder = new SpellCaster(-1, -1, Hittable.Team.PLAYER);
        }
        Random rng = new Random();

        int index = rng.Next(0, GameManager.Instance.spellKeys.Count);
        string spellName = GameManager.Instance.spellKeys[index];
        SpellInfo spinf = GameManager.Instance.SpellsDict[spellName].Duplicate();
        if (spinf == null)
        {
            Debug.LogError($"SpellInfo is null for key: {spellName}");
            return null;
        }

        Dictionary<string, ModifierInfo> availableModifiers = GameManager.Instance.ModDict;
        
        if (availableModifiers == null || availableModifiers.Count == 0)
            throw new ArgumentException("availableModifiers must not be null or empty.");
        
        int numberOfModifiers = 3;
        
        var keys = new List<string>(availableModifiers.Keys); // copy, don't mutate original
        List<Modifier> spellModifiers = new List<Modifier>();
        
        Spell current = new Spell(placeholder, spinf);
        SpellInfo global = current.spellInfo;
        for (int i = 0; i < numberOfModifiers; i++)
        {
            ModifierInfo info = availableModifiers[keys[rng.Next(0, keys.Count)]];
            Modifier mod = CreateModifier(placeholder, info, current);
            if (mod == null) continue; 
            mod.spellInfo = global;
            spellModifiers.Add(mod);
            current = mod;
        }
        foreach (Modifier mod in spellModifiers)
        {
            mod.ApplyModStats();
        }

        return current;
    }
}
