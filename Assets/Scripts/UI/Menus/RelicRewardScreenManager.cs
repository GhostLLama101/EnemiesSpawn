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
    {}

    // Update is called once per frame
    public void OnEnable()
    {
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(Next);

        availableRelics.Clear(); // clear here too, not just in Next()

        Random rng = new Random();
        while (availableRelics.Count < 3)
        {
            RelicInfo newRelic = GameManager.Instance.RelicList[rng.Next(0, GameManager.Instance.Relics.Count)];
            if (!availableRelics.Contains(newRelic))
                availableRelics.Add(newRelic);
        }

        RelicIconManager rico_man = GameManager.Instance.relicIconManager;
        if (rico_man != null)
        {
            GameObject[] relicObjects = { relic1, relic2, relic3 };
            for (int i = 0; i < 3; i++)
            {
                int j = i;
                rico_man.PlaceSprite(availableRelics[j].sprite, relicObjects[j].GetComponent<Image>());
                relicObjects[j].transform.Find("Name(TMP)").GetComponent<TextMeshProUGUI>().text = availableRelics[j].name;
                relicObjects[j].transform.Find("Description").GetComponent<TextMeshProUGUI>().text =
                    availableRelics[j].trigger.description + " " + availableRelics[j].effect.description;

                Button button = relicObjects[j].transform.Find("pickButton").GetComponent<Button>();
                button.onClick.RemoveAllListeners(); // clear before adding
                button.onClick.AddListener(() => {
                    AddRelic(availableRelics[j]);
                    for (int k = 0; k < 3; k++)
                        relicObjects[k].transform.Find("pickButton").GetComponent<Button>().gameObject.SetActive(false);
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
        availableRelics.Clear();
        GameManager.Instance.enemySpawner.NextWave();
    }
    public void AddRelic(RelicInfo relicinfo)
    {
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.PlayerRelics.Add(new RelicBaseClass(player, relicinfo));
        EventBus.Instance.DoRelicPickup();
    }
}
