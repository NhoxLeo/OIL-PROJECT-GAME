using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugScript : MonoBehaviour
{
    GameObject population;
    public GameObject human;
    public GameObject lazyBone;

    private void Start()
    {
        population = GameObject.Find("Population");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject tempHuman = Instantiate(human);
            tempHuman.transform.parent = population.gameObject.transform;
            NavMeshAgent nv = tempHuman.GetComponent<NavMeshAgent>();
            nv.Warp(new Vector3(20, 1, 50));
            population.GetComponent<PopulationManager>().AddHuman(tempHuman);
        }
        if (Input.GetKeyDown(KeyCode.P))
            GameObject.Find("Sun").GetComponent<Eclipse>().enabled = true;
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (Transform human in population.transform)
                human.GetComponent<Human>().AddBuff(new Buff("Test buff", 20f, 5f, human.gameObject));
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject.Find("Sandstorm").transform.Find("Sandstorm_Particles").gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
        //    EventAIState.CDTimer = 98312;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
         //   EventAIState.ForceThisEvent = GameObject.Find("GameEvents").GetComponent<AddPopulationEvent>();
       //     EventAIState.CDTimer = 98312;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            lazyBone = GameObject.Find("Lazybone").transform.Find("CanvasLazybone").transform.Find("LazybonePanel").gameObject;
            if (lazyBone.activeSelf)
            {
                lazyBone.SetActive(false);
            }
            else
            {
                lazyBone = GameObject.Find("Lazybone").transform.Find("CanvasLazybone").transform.Find("LazybonePanel").gameObject;
                lazyBone.SetActive(true);
            }
        }      
    }
}
