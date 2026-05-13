using System;
using UnityEngine;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public SpellUI spellRewardUI;
    public TextMeshProUGUI damageText;
    public SpellUIContainer spellUIContainer;
    Spell spellReward;
    private bool rewarded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spellRewardUI == null)
        {
            Debug.LogError("spellRewardUI is not assigned in the Inspector!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            if (!rewarded)
            {
                rewardUI.SetActive(true);
    
                try 
                {
                    spellReward = SpellBuilder.RandomSpell();
                    Debug.Log($"spellReward: {spellReward}");
                    spellRewardUI.SetSpell(spellReward);
                }
                catch (Exception e)
                {
                    Debug.LogError($"RandomSpell failed: {e}");
                }
    
                rewarded = true;
            }
        }
        else
        {
            rewardUI.SetActive(false);
            rewarded = false;
        }
    }
    
    public void AcceptReward()
    {
        if (spellReward == null) return;

        PlayerController player = spellUIContainer.player;
    
        if (!player.spellcaster.IsFull())
        {
            // add to next open slot and update that UI slot
            int newIndex = player.spellcaster.spells.Count; // before adding
            player.spellcaster.AddSpell(spellReward);
            spellUIContainer.AddSpell(newIndex, spellReward);
        
            rewardUI.SetActive(false);
            rewarded = false;
            spellReward = null;
        }
        else
        {
            // all 4 slots full — show swap UI
            // you'll need a separate panel with 4 buttons, one per slot
            ShowSwapUI();
        }
    }

    void ShowSwapUI()
    {
        // TODO: show 4 buttons letting the player pick which slot to replace
        // each button calls SwapSpell(int slotIndex)
        Debug.Log("Spells full — show swap UI");
    }

    public void SwapSpell(int slotIndex)
    {
        spellUIContainer.player.spellcaster.spells[slotIndex] = spellReward;
        spellUIContainer.AddSpell(slotIndex, spellReward);
    
        rewardUI.SetActive(false);
        rewarded = false;
        spellReward = null;
    }
}
