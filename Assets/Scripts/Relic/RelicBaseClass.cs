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
        if (relicInfo.name == "Red Pendant")
        {
            this.relicInfo = new MoveGainSpellpower();
        }
        else if (relicInfo.name == "Ancient Spellbook")
        {
            this.relicInfo = new SpellsGivePower();
        }
        else
        {
            this.player = player;
            this.RPNDict = new Dictionary<string, int>();
            this.relicInfo = relicInfo.Duplicate();
            AddToEventBus(this.relicInfo, this.relicInfo.trigger.type);
            //ReadRelics(GameManager.Instance.Relics);
        }
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
                EventBus.Instance.OnTakeDamage  += () => OnTakeDamage(Relic);
                break;
            case "stand-still":
                Debug.Log("Added OnNotMove to bus");
                EventBus.Instance.OnNotMove += ()=> OnNotMove(Relic);
                break;
            case "on-kill":
                EventBus.Instance.OnEnemyKilled += ()=> OnEnemyKilled(Relic);
                break;
            case "move-x-units":
                EventBus.Instance.OnMoved10 += ()=> OnMoved10(Relic);
                break;
        }
    }
    
    private void OnTakeDamage(RelicInfo Relic)
    {
        RPNDict["wave"] = GameManager.Instance.wave_count;
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        DoEffectTypes(Relic.effect.type, amount);
    }
    
    private void OnNotMove(RelicInfo Relic)
    {
        RPNDict["wave"] = GameManager.Instance.wave_count;
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        DoEffectTypes(Relic.effect.type, amount);
    }

    private void OnEnemyKilled(RelicInfo Relic)
    {
        RPNDict["wave"] = GameManager.Instance.wave_count;
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        DoEffectTypes(Relic.effect.type, amount);
    }
    private void OnMoved10(RelicInfo Relic)
    {
        int amount = Evaluate(Effects.GetAmount(Relic.name),  RPNDict);
        DoEffectTypes(Relic.effect.type, amount);
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
        }
    }
}
