using System;
using System.Collections.Generic;
using UnityEngine;
using static RPNEvaluator.RPNEvaluator;

public class MoveGainSpellpower : RelicInfo
{
    public int addedSpellpower = 0;
    
    public MoveGainSpellpower()
    {
        Debug.Log("Added MoveGainSpellpower");
        EventBus.Instance.OnMoved10 += OnMoved10;
        EventBus.Instance.OnScaledPlayer += OnScaledPlayer;
        this.name = "Red Pendant";
        this.sprite = 7;
        this.trigger.description = "Every 10 units you travel";
        this.trigger.type = "move";
        this.trigger.amount = "10";
        this.effect.description = "you gain 1 spellpower";
        this.effect.type = "gain-spellpower";
        this.effect.amount = "1";
    }

    void OnMoved10() //Increases total in the relic 
    //Also handles mid-round increases
    {
        addedSpellpower++;
        PlayerController playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        Effects.AddSpellPower(1, playerController);
    }

    void OnScaledPlayer() //Adds spellpower when power is refreshed
    {
        PlayerController playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        Effects.AddSpellPower(addedSpellpower, playerController);
    }

}
