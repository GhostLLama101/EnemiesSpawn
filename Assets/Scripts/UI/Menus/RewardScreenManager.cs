using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public SpellUI spellRewardUI;
    //public TextMeshProUGUI damageText;
    public SpellUIContainer spellUIContainer;
    public Button exchangeButton;
    public Button acceptButton;
    Spell spellReward;
    private bool rewarded = false;
    private bool accepted = false;

    [Header("Swap Panel")] public GameObject swapPanel;
    public SpellUI[] swapSlotUIs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spellRewardUI == null)
        {
            Debug.LogError("spellRewardUI is not assigned in the Inspector!");
            return;
        }
        swapPanel.SetActive(false);
        exchangeButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            if (!rewarded)
            {
                rewardUI.SetActive(true);
                acceptButton.gameObject.SetActive(true);
                exchangeButton.gameObject.SetActive(false);
                swapPanel.SetActive(false);
                
                try
                {
                    spellReward = SpellBuilder.RandomSpell();
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
 
        // Hide accept immediately so it can't be clicked again
        acceptButton.gameObject.SetActive(false);
 
        PlayerController player = spellUIContainer.player;
 
        if (!player.spellcaster.IsFull())
        {
            int newIndex = player.spellcaster.spells.Count;
            spellUIContainer.AddSpell(newIndex, spellReward);
            player.spellcaster.AddSpell(spellReward);
            spellReward = null;
            // Player now clicks Next to start next wave
            Debug.Log("AcceptReward called");
            EventBus.Instance.DoOnReceiveSpell();
        }
        else
        {
            // Spells full — show exchange button
            ShowSwapUI();
        }
    }
    void ShowSwapUI()
    {
        exchangeButton.gameObject.SetActive(true);
    }
    public void SwapSpell(int slotIndex)
    {
        
        spellUIContainer.player.spellcaster.ReplaceSpell(slotIndex, spellReward);
        spellUIContainer.AddSpell(slotIndex, spellReward);
 
        foreach (var slot in swapSlotUIs)
            slot.SetClickable(false, null);
 
        swapPanel.SetActive(false);
        spellReward = null;
        Debug.Log("AcceptReward called");
        EventBus.Instance.DoOnReceiveSpell();
        // Player now clicks Next to start next wave
    }

    

    public void OnExchangeClicked()
    {
        exchangeButton.gameObject.SetActive(false);
        swapPanel.SetActive(true);
 
        SpellCaster caster = spellUIContainer.player.spellcaster;
        for (int i = 0; i < swapSlotUIs.Length; i++)
        {
            if (i >= caster.spells.Count)
            {
                swapSlotUIs[i].gameObject.SetActive(false);
                continue;
            }
            swapSlotUIs[i].gameObject.SetActive(true);
            swapSlotUIs[i].SetSpell(caster.spells[i]);
 
            int slotIndex = i;
            swapSlotUIs[i].SetClickable(true, () => SwapSpell(slotIndex));
        }
    }
}
