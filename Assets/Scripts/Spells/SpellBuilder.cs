using System.Buffers;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public static class SpellBuilder 
{
    //building the player's spells?
    public static Spell Build(SpellCaster owner, Spell spell)
    {
        //add the modifiers here or something?
        return new Spell(owner, spell.spellInfo);
    }

   // generic 
   // For making random spells?
   public static Spell Build(Spell coreSpell, List<Modifier> modifiers)
   {
       Spell current = coreSpell;
       foreach (var modifier in modifiers)
       {
           modifier.inner = current;
           current = modifier;
       }
       return current;
   }
    
    // randomSpell() function goes here

}
