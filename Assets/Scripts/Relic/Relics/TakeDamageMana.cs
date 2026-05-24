using System;
using System.Collections.Generic;
using UnityEngine;
using static RPNEvaluator.RPNEvaluator;
public class TakeDamageMana 
{
    private PlayerController player;
    public Dictionary<string, int> RPNDict = new Dictionary<string, int>();
    public TakeDamageMana(PlayerController player)
    {
        this.player = player;
        Debug.Log("Added OnTakeDamageMana to event bus");
        this.RPNDict =  new Dictionary<string, int>();
        //EventBus.Instance.OnTakeDamage += OnTakeDamage;
    }

    private void OnTakeDamage(Hittable target)
    {
        Debug.Log("You took damage you get 5 mana");
        int number = Evaluate(Effects.GetAmount("Green Gem"), RPNDict);
        Effects.AddSpellPower(number, player);
        Effects.AddMana(number, player);
    }
    
    
}
