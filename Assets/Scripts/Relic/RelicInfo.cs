using System;
using UnityEngine;

[Serializable]
public class RelicInfo
{
    public string name;
    public int sprite;
    public RelicTrigger trigger;
    public RelicEffect effect;
    
    // TODO this needs to be fixed.  
    public int getAmount(RelicTrigger trigger)
    {
        return effect.amount;
    }
}
[Serializable] 
public class RelicTrigger
{
    public string description;
    public string type;
    public string amount;
}

[Serializable]
public class RelicEffect
{
    public string description;
    public string type;
    public int amount;
}
