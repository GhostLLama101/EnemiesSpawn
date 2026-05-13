using UnityEngine;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public SpellUI spellRewardUI;
    public TextMeshProUGUI damageText;
    Spell spellReward;
    private bool rewarded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rewardUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            if (!rewarded)
            {
                damageText.text = $"Damage Dealt: {GameManager.Instance.total_damage_dealt}";
                rewardUI.SetActive(true);
            
                spellReward = SpellBuilder.RandomSpell();
                spellRewardUI.SetSpell(spellReward);
                rewarded = true;
            }
        }
        else
        {
            rewardUI.SetActive(false);
            rewarded = false;
        }
    }
}
