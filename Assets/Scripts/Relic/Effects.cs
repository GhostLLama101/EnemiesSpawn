using Unity.VisualScripting;
using UnityEngine;
// this is where the effects function will all be called.
public class Effects : MonoBehaviour
{
    public static void AddMana(int amount, PlayerController player)
    {
        player.spellcaster.mana = Mathf.Min(player.spellcaster.mana + amount, player.spellcaster.max_mana);
    }

    public static void AddSpellPower(int amount, PlayerController player)
    {
        player.spellcaster.power += amount;
    }

    public static void RemoveSpellPower(int amount, PlayerController player)
    {
        player.spellcaster.power -= amount;
    }

    public static void GainHealth(int amount, PlayerController player)
    {
        player.health += amount;
    }

    public static string GetAmount(string name)
    {
        return GameManager.Instance.Relics[name].effect.amount;
    }
}
