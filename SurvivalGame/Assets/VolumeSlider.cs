using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

    public GameObject slider;    
    public GameObject mm;
    public Text userFeedback;
    void Awake()
    {

    }
    public void UpdateMusicVolume()
    {
        mm.GetComponent<AudioSource>().volume =
        slider.GetComponent<Scrollbar>().value;
        userFeedback.text = mm.GetComponent<AudioSource>().volume.ToString("N2");
    }
    //void Update()
    //{
    //    if (em.HasEventRunning)
    //    {
          
    //        slider.GetComponent<Slider>().value;
    //        if (slider.GetComponent<Slider>().value >= decisionTimer)
    //        {
    //            slider.GetComponent<Slider>().value = 0;
    //            slider.transform.localScale = new Vector3(0, 0, 0);
    //            em.DecisionTimerRunDown();
    //        }
    //    }
    //}

    //public void StartDecisionTimer()
    //{
    //    slider.GetComponent<Slider>().maxValue = decisionTimer;
    //    slider.transform.localScale = new Vector3(1, 1, 1);
    //}

    //public void ResetDecisionTimer()
    //{
    //    slider.GetComponent<Slider>().value = 0;
    //    slider.transform.localScale = new Vector3(0, 0, 0);
    //}
}
