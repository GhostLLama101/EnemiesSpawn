using System.Buffers;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{

    public Spell Build(SpellCaster owner, SpellInfo spell)
    {
        
        return new Spell(owner, spell);
    }

   // generic 
   public Spell Build(Spell coreSpell, List<Modifier> modifiers)
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
