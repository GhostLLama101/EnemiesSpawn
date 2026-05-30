using System;
using System.Collections.Generic;
using UnityEngine;
using static RPNEvaluator.RPNEvaluator;

public  class RelicBaseClass 
{
    private PlayerController player;
    public RelicInfo relicInfo;
    public Dictionary<string, int> RPNDict = new Dictionary<string, int>();
    public int permanent_effects = 0;

    public RelicBaseClass(PlayerController player, RelicInfo relicInfo)
    {
        this.player = player;
        this.RPNDict = new Dictionary<string, int>();
        this.relicInfo = relicInfo.Duplicate();
        AddToEventBus(this.relicInfo, this.relicInfo.trigger.type);
        //ReadRelics(GameManager.Instance.Relics);
        
        // need to find where the event fires called?
        EventBus.Instance.OnScaledPlayer += OnScaledPlayer;
        
    }
    
    private void ReadRelics(Dictionary<string, RelicInfo> Relics) 
    {
        // call add relic for event bus
        foreach (var relic in Relics)
        {
            AddToEventBus(relic.Value, relic.Value.trigger.type);
        }
    }
    private void AddToEventBus(RelicInfo Relic, string type)
    {
        switch (type)
        {
            case "take-damage":
                Debug.Log("take-damage");
                EventBus.Instance.OnTakeDamage  += () => OnTakeDamage(Relic);
                break;
            case "stand-still":
                Debug.Log("Added OnNotMove to bus");
                EventBus.Instance.OnNotMove += ()=> OnNotMove(Relic);
                break;
            case "on-kill":
                Debug.Log("on-kill");
                EventBus.Instance.OnEnemyKilled += ()=> OnEnemyKilled(Relic);
                break;
            case "move-x-units":
                Debug.Log("move-x-units");
                EventBus.Instance.OnMoved10 += ()=> OnMoved10(Relic);
                break;
            case "take-spell":
                Debug.Log("take-spell");
                EventBus.Instance.OnReceiveSpell += () => OnReceiveSpell(Relic);
                break;
        }
    }
    private void RegisterUntil(RelicInfo Relic, int amount)
    {
        switch (Relic.effect.until)
        {
            case "move":
                Action onMove = null;
                onMove = () =>
                {
                    DoRemoveEffectTypes(Relic.effect.type, amount);
                    EventBus.Instance.OnMove -= onMove; // unsubscribe after firing once
                };
                EventBus.Instance.OnMove += onMove;
                break;

            case "cast-spell":
                Action onCast = null;
                onCast = () =>
                {
                    DoRemoveEffectTypes(Relic.effect.type, amount);
                    EventBus.Instance.OnSpellCasted -= onCast;
                };
                EventBus.Instance.OnSpellCasted += onCast;
                break;
        }
    }
    
    private void OnScaledPlayer()
    {
        // Re-apply any permanently accumulated effects after a stat reset
        if (permanent_effects > 0)
            DoEffectTypes(relicInfo.effect.type, permanent_effects);
    }
    
    private void OnTakeDamage(RelicInfo Relic)
    {
        RPNDict["wave"] = GameManager.Instance.wave_count;
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        DoEffectTypes(Relic.effect.type, amount);
        RegisterUntil(Relic, amount);
    }
    
    private void OnNotMove(RelicInfo Relic)
    {
        RPNDict["wave"] = GameManager.Instance.wave_count;
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        DoEffectTypes(Relic.effect.type, amount);
        RegisterUntil(Relic, amount);
    }

    private void OnEnemyKilled(RelicInfo Relic)
    {
        RPNDict["wave"] = GameManager.Instance.wave_count;
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        DoEffectTypes(Relic.effect.type, amount);
    }
    private void OnMoved10(RelicInfo Relic)
    {
        RPNDict["wave"] = GameManager.Instance.wave_count;
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        permanent_effects += amount;
        DoEffectTypes(Relic.effect.type, amount);
    }
    
    private void OnReceiveSpell(RelicInfo Relic)
    {
        // Recalculate total from scratch based on spell count, like SpellsGivePower did
        int spellsTaken = player.spellcaster.spells.Count -1;
        int totalSpellpower = spellsTaken * Evaluate(Effects.GetAmount(Relic.name), new Dictionary<string, int>());
        permanent_effects = totalSpellpower;
        // DoEffectTypes is NOT called here — OnScaledPlayer will apply it on next reset
        // If you want an immediate mid-round bonus, add:
        // DoEffectTypes(Relic.effect.type, totalSpellpower - previousTotal);
    }
    
    private void DoEffectTypes(string type ,int amount)
    {
        switch (type)
        {
            case "gain-mana":
                Effects.AddMana(amount, player);
                break;
            case "gain-spellpower":
                Effects.AddSpellPower(amount, player);
                break;
            case "gain-health":
                Effects.GainHealth(amount, player);
                break;
            case "gain-speed":
                Effects.GainSpeed(amount, player);
                break;
            case "gain-invulnerability":
                Effects.GainInvulnerability();
                break;
        }
    }
    
    private void DoRemoveEffectTypes(string type, int amount)
    {
        switch (type)
        {
            case "gain-spellpower":
                Effects.RemoveSpellPower(amount, player);
                break;
        }
    }
}
