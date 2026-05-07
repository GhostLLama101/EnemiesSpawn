using System.Data;
using UnityEngine;
using static RPNEvaluator.RPNEvaluator;

public class Damage 
{
    public int amount;
    public enum Type
    {
        PHYSICAL, ARCANE, NATURE, FIRE, ICE, DARK, LIGHT
    }
    public Type type;
    public Damage(string amount, Type type)
    {
        Dictionary<string, int> dictForRPN = new Dictionary<string, int>();
        dictForRPN["power"] = GameManager.Instance.player.power;

        this.amount = Evaluate(amount, dictForRPN);
        this.type = type;
    }

    public static Type TypeFromString(string type)
    {
        string t = type.ToLower();
        if (t == "arcane") return Type.ARCANE;
        if (t == "nature") return Type.NATURE;
        if (t == "fire") return Type.FIRE;
        if (t == "ice") return Type.ICE;
        if (t == "dark") return Type.DARK;
        if (t == "light") return Type.LIGHT;
        return Type.PHYSICAL;
    }

    public static string TypeToString(Type type)
    {
        //string t = type.ToLower();
        if (type == Type.ARCANE) return "arcane";
        if (type == Type.NATURE) return "nature";
        if (type == Type.FIRE) return "fire";
        if (type == Type.ICE) return "ice";
        if (type == Type.DARK) return "dark";
        if (type == Type.LIGHT) return "light";
        return "physical";
    }
    
}
