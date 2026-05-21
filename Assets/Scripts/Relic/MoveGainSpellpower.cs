using System;
using System.Collections.Generic;
using UnityEngine;
using static RPNEvaluator.RPNEvaluator;

public class MoveGainSpellpower : RelicInfo
{
    public int addedSpellpower = 0;
    public float totalDistance = 0f;
    public MoveGainSpellpower()
    {
        Debug.Log("Added MoveGainSpellpower");
        EventBus.Instance.OnMoved50 += OnMove;
        this.name = "Red Pendant";
        this.sprite = 7;
        this.trigger.description = "Every 10 units you travel";
        this.trigger.type = "move";
        this.trigger.amount = "10";
        this.effect.description = "you gain 1 spellpower";
        this.effect.type = "gain-spellpower";
        this.effect.amount = "1";
    }

    void OnMove()
    {
        
        int new_sp = (int)GameManager.Instance.totalDistance/
                Evaluate(trigger.amount, new Dictionary<string, int>());
        //Debug.Log("Distance Moved "+totalDistance);
        if (new_sp > addedSpellpower)
        {
            addedSpellpower = new_sp;
        }
        
    }

}
