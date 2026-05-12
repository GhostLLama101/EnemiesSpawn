using UnityEngine;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public SpellUI spellRewardUI;
    public TextMeshProUGUI damageText;
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
            damageText.text = $"Damage Dealt: {GameManager.Instance.total_damage_dealt}";
            rewardUI.SetActive(true);
            SpellCaster placeholder = new SpellCaster(-1, -1, Hittable.Team.PLAYER);
            //TODO: Finish this
            //spellRewardUI.SetSpell(SpellBuilder.RandomSpell(placeholder, ));
        }
        else
        {
            rewardUI.SetActive(false);
        }
    }
}
