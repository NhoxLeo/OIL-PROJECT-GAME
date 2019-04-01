using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventTimeSliderScript : MonoBehaviour
{

    public GameObject slider;
    public EventManager em;
    public float decisionTimer;
    void Awake()
    {

    }

    void Update()
    {
        if (em.HasEventRunning)
        {
            slider.GetComponent<Slider>().value += Time.deltaTime;
            if (slider.GetComponent<Slider>().value >= decisionTimer)
            {
                slider.GetComponent<Slider>().value = 0;
                slider.transform.localScale = new Vector3(0, 0, 0);
                em.DecisionTimerRunDown();
            }
        }
    }

    public void StartDecisionTimer()
    {
        slider.GetComponent<Slider>().maxValue = decisionTimer;
        slider.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ResetDecisionTimer()
    {
        slider.GetComponent<Slider>().value = 0;
        slider.transform.localScale = new Vector3(0, 0, 0);
    }
}
