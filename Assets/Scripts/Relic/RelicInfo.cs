using System;
using UnityEngine;

[Serializable]
public class RelicInfo
{
    public string name;
    public int sprite;
    public RelicTrigger trigger = new RelicTrigger();
    public RelicEffect effect = new RelicEffect();

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
    public string amount;
}
