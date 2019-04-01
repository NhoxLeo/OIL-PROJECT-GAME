using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectPanelManager : MonoBehaviour
{
    private Text homeless, unemployed, hungry, thirsty;
    private short hungryAmount, thirstyAmount;
    private PopulationManager pm;
    private PopulationCollection pc;

    private void Start()
    {
        homeless = transform.GetChild(0).Find("Text2").GetComponent<Text>();
        unemployed = transform.GetChild(1).Find("Text2").GetComponent<Text>();
        hungry = transform.GetChild(2).Find("Text2").GetComponent<Text>();
        thirsty = transform.GetChild(3).Find("Text2").GetComponent<Text>();
        pm = GameObject.Find("Population").GetComponent<PopulationManager>();
    }

    void Update()
    {
        thirstyAmount = 0;
        hungryAmount = 0;
        homeless.text = "" + pm.GetHomelessCount();        
        if (!GlobalConstants.ChildLabor)
            unemployed.text = "" + pm.populationCollection.GetAllUnemployedAdults().Count;
        else
            unemployed.text = "" + pm.GetUnemployedCount();
        foreach (Transform human in pm.transform)
        {
            if (human.GetComponent<Human>().GetHunger() < 0)
                hungryAmount++;
            if (human.GetComponent<Human>().GetThirst() < 0)
                thirstyAmount++;
        }
        hungry.text = "" + hungryAmount;
        thirsty.text = "" + thirstyAmount;
    }
}
