using System;
using System.Collections.Generic;
using UnityEngine;
using static RPNEvaluator.RPNEvaluator;

public class SpellsGivePower : RelicInfo
{
    public int additionalSpellpower = 0;
    public int spellsTaken = 0;
    public SpellsGivePower()
    {
        Debug.Log("Added SpellsGivePower");
        EventBus.Instance.OnReceiveSpell += OnReceiveSpell;
        EventBus.Instance.OnScaledPlayer += OnScaledPlayer;
        this.name = "Ancient Spellbook";
        this.sprite = 10;
        this.trigger.description = "For every spell you take";
        this.trigger.type = "take-spell";
        this.trigger.amount = "1";
        this.effect.description = "you gain 5 spellpower";
        this.effect.type = "gain-spellpower";
        this.effect.amount = "5";
    }

    void OnReceiveSpell()
    {
        spellsTaken ++;
        additionalSpellpower = spellsTaken*Evaluate(this.effect.amount, new Dictionary<string, int>());
        //Debug.Log("WE TOOK A SPELL; ADDED SPELLPOWER IS "+additionalSpellpower);
    }
    void OnScaledPlayer() //This should give extra spellpower each time we scale the player
    {
        PlayerController playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        //Debug.Log("Old power " + playerController.spellcaster.power);
        Effects.AddSpellPower(additionalSpellpower, playerController);
        //Debug.Log("New power " + playerController.spellcaster.power);
    }

}
