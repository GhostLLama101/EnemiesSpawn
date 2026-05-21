using UnityEngine;

public class KillEnemyRelic : RelicInfo
{
    private PlayerController player;
    public KillEnemyRelic(PlayerController player)
    {
        this.player = player;
        Debug.Log("Added OnEnemyKilled to bus");
        EventBus.Instance.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        Debug.Log("Killed enemy +10 mana");
        
        Effects.AddMana(10, player);
    }
}
