using System;
using UnityEngine;

[Serializable]
public class RelicInfo
{
    public string name;
    public int sprite;
    public RelicTrigger trigger = new RelicTrigger();
    public RelicEffect effect = new RelicEffect();

    public RelicInfo Duplicate()
    {
        RelicInfo newRelicInfo = new RelicInfo();
        newRelicInfo.name = this.name;
        newRelicInfo.sprite = this.sprite;
        newRelicInfo.trigger = this.trigger.Duplicate();
        newRelicInfo.effect = this.effect.Duplicate();

        return newRelicInfo;
    }
}
[Serializable] 
public class RelicTrigger
{
    public string description;
    public string type;
    public string amount;

    public RelicTrigger Duplicate()
    {
        RelicTrigger newRelicTrigger = new RelicTrigger();
        
        newRelicTrigger.description = this.description;
        newRelicTrigger.type = this.type;
        newRelicTrigger.amount = this.amount;

        return newRelicTrigger;
    }
}

[Serializable]
public class RelicEffect
{
    public string description;
    public string type;
    public string amount;
    public string until;

    public RelicEffect Duplicate()
    {
        RelicEffect newRelicEffect = new RelicEffect();
        
        newRelicEffect.description = this.description;
        newRelicEffect.type = this.type;
        newRelicEffect.amount = this.amount;
        newRelicEffect.until = this.until;

        return newRelicEffect;
    }
}
