using Unity.VisualScripting;
using UnityEngine;
// this is where the effects function will all be called.
public class Effects : MonoBehaviour
{
    float invincibleTimer;
    static bool playerInvincible;

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
        player.hp.hp += amount;
        if (player.hp.hp > 2*player.hp.max_hp)
        {
            player.hp.hp = player.hp.max_hp;
        }
    }

    public static void GainSpeed(int amount, PlayerController player)
    {
        player.speed += amount;
    }
    public static void GainInvulnerability()
    {
        playerInvincible = true;
    }

    public static string GetAmount(string name)
    {
        return GameManager.Instance.Relics[name].effect.amount;
    }

    void Update()
    {
        if (playerInvincible)
        {
            invincibleTimer += Time.deltaTime;

            if (invincibleTimer >= 5f)
            {
                playerInvincible = false;
            }
        }
    }
}
