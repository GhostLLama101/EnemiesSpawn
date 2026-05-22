using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RelicRewardScreenManager : MonoBehaviour
{
    public GameObject screen;
    public GameObject relic1;
    public GameObject relic2;
    public GameObject relic3;
    public Button nextButton;
    public List<RelicInfo> availableRelics = new List<RelicInfo>();
    private bool rewarded = false;
    //private bool accepted = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screen.SetActive(false);
        
        //nextButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            if (!rewarded)
            {
                /*
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
                */
                rewarded = true;
            }
        }
        else
        {
            //rewardUI.SetActive(false);
            rewarded = false;
        }
    }
}
