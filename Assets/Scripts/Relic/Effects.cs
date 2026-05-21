using UnityEngine;
// this is where the effects function will all be called.
public class Effects : MonoBehaviour
{
    public static void AddMana(int amount, PlayerController player)
    {
        player.spellcaster.mana = Mathf.Min(player.spellcaster.mana + amount, player.spellcaster.max_mana);
    }

    public void AddSpellPower(int amount, PlayerController player )
    {
        
    }
}
