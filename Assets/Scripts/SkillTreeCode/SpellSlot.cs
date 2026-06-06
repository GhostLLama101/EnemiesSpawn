using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
public class SpellSlot
{
    public int spell_slot;
    public int spell_cost;
    public int sprite;
    public List<Available_Modifiers> available_modifiers;
}

[Serializable]
public class Available_Modifiers
{
    public string name;
    public int cost;
}


