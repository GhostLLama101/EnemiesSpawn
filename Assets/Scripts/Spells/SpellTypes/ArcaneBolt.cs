using UnityEngine;
using System.Collections;


public class ArcaneBolt : Spell
{
    public ArcaneBolt(SpellCaster owner) : base(owner, GameManager.Instance.SpellsDict["arcane_bolt"]) { }
}
