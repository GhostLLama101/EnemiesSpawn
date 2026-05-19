using System;
using UnityEngine;

[Serializable]
public class RelicInfo
{
    public string name;
    public int sprite;
    public RelicTrigger trigger;
    public RelicEffect effect;

}
[Serializable] 
public class RelicTrigger
{
    public string description;
    public string type;
}

[Serializable]
public class RelicEffect
{
    public string description;
    public string type;
    public int amount;
}
