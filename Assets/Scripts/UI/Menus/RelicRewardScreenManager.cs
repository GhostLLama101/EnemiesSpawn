using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Random = System.Random;

public class RelicRewardScreenManager : MonoBehaviour
{
    public GameObject screen;
    public GameObject relic1;
    public GameObject relic2;
    public GameObject relic3;
    public Button nextButton;
    public List<RelicInfo> availableRelics = new List<RelicInfo>();
    //private bool rewarded = false;
    //private bool accepted = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screen.SetActive(false);
    }

    // Update is called once per frame
    public void OnEnable()
    {
        //do rewards
        //Show them
        //Activate the buttons maybe?
        nextButton.onClick.AddListener(Next);

        Random rng = new Random();
        RelicInfo newRelic;

        while (availableRelics.Count < 3)
        {
            newRelic = GameManager.Instance.RelicList[rng.Next(0, GameManager.Instance.Relics.Count)];
            
            if (!availableRelics.Contains(newRelic))
            {
                availableRelics.Add(newRelic);
            }
        }
        RelicIconManager rico_man = GameManager.Instance.relicIconManager;
        if (rico_man != null){ 
            GameObject[] relicObjects = { relic1, relic2, relic3 };
            for (int i = 0; i < 3; i++)
            {
                int j = i;
                rico_man.PlaceSprite(availableRelics[j].sprite, relicObjects[j].GetComponent<Image>());
                relicObjects[j].transform.Find("Name(TMP)").GetComponent<TextMeshProUGUI>().text =
                    availableRelics[j].name;
                relicObjects[j].transform.Find("Description").GetComponent<TextMeshProUGUI>().text =
                    availableRelics[j].trigger.description+" "+availableRelics[j].effect.description;


                Button button = relicObjects[j].transform.Find("pickButton").GetComponent<Button>();
                button.onClick.AddListener(() => {
                    AddRelic(availableRelics[j]);
                    for (int k = 0; k < 3; k++)
                    {
                        relicObjects[k].transform.Find("pickButton").GetComponent<Button>().gameObject.SetActive(false);
                    }
                    //button.RemoveAllListeners(); 
                });
            }
        }
        
    }
    public void Next()
    {
        nextButton.onClick.RemoveListener(Next);
        screen.SetActive(false);
        GameObject[] relicObjects = {relic1, relic2, relic3};
        for (int k = 0; k < 3; k++)
        {
            relicObjects[k].transform.Find("pickButton").GetComponent<Button>().gameObject.SetActive(true);
        }
    }
    public void AddRelic(RelicInfo relicinfo)
    {
        //TODO: Change from adding relicInfos to relics
        GameManager.Instance.player.GetComponent<PlayerController>()
            .PlayerRelics.Add(relicinfo.Duplicate());
    }
}
