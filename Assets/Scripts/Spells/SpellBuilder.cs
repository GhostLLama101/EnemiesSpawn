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

   
    public SpellBuilder()
    {        
    }

}
