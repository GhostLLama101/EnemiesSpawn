using UnityEngine;

public class KillEnemyRelic : RelicBaseClass
{
    public KillEnemyRelic(PlayerController player) : base(player) { }
    /*protected override void Subscribe()
    {
        EventBus.Instance.OnEnemyKilled += OnEnemyKilled;
    }
    public override void Unsubscribe()
    {
        EventBus.Instance.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        Debug.Log("Killed enemy +10 mana");
        Effects.AddMana(10, player);
    }*/
}
