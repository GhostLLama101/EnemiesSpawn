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
        this.trigger.description = "When you take a spell";
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
    }
    void OnScaledPlayer() //This should give extra spellpower each time we scale the player
    {
        PlayerController playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        Effects.AddSpellPower(additionalSpellpower, playerController);
    }

}
