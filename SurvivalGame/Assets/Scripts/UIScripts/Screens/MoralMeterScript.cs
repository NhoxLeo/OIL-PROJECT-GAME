using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoralMeterScript : MonoBehaviour {
    private PopulationManager pm;
    private Slider slider;
    private GameObject background;

    public void Start() {
        pm = GameObject.Find("Population").GetComponent<PopulationManager>();
        slider = GetComponent<Slider>();
    
        background = transform.Find("Background").gameObject;
    }

    public void Update() {
        float moral = pm.TotalMoral();
        slider.value = moral;
        GetComponent<MouseOverListener>().tooltipText = "Moral: " + (int)moral;
        background.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, (moral + 100) / 200);     
    }
}
