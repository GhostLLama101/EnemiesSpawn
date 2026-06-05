using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

[System.Serializable] 
public class SpellDamage
{
    public string amount = "25 power 5 / +"; //just leaving these as base values
    public string type = "arcane";

    public SpellDamage Duplicate()
    {
        SpellDamage newDamage = new SpellDamage();
        newDamage.amount = this.amount;
        newDamage.type = this.type;
        return newDamage;
    }
}