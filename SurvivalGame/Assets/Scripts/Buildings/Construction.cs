using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script only exists as long as a building is being constructed and controls that logic.
/// </summary>
public class Construction : MonoBehaviour {
    private float timer, totalTime;
    private Vector3 targetPos, startPos;
    private Vector2 sliderPosCanvas, sliderScreenPos;
    private GameObject buildTimeSlider;
    private GameObject canvas;

    public void Start() {
        totalTime = gameObject.GetComponent<Building>().GetConstructionTime();
        timer = 0f;
        canvas = GameObject.Find("Canvas");
        buildTimeSlider = Instantiate(Resources.Load("Prefabs/Misc/BuildTimeSlider"), transform.position, new Quaternion()) as GameObject;
        buildTimeSlider.transform.SetParent(canvas.transform);
        targetPos = transform.position;
        startPos = new Vector3(transform.position.x, transform.position.y - gameObject.GetComponent<Collider>().bounds.size.y, transform.position.z);
        transform.position = startPos;
        gameObject.GetComponent<Building>().SetConstructed(false);
        buildTimeSlider.GetComponent<Slider>().maxValue = totalTime;
        MessageDisplay.SpawnFloatingText("Building built");
        SoundManager.PlaySound("construct");
    }

    /// <summary>
    /// Updates the construction process.
    /// </summary>
    public void Update() {
        timer += Time.deltaTime;
        sliderScreenPos = Camera.main.WorldToScreenPoint(targetPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), sliderScreenPos, null, out sliderPosCanvas);
        buildTimeSlider.transform.localPosition = sliderPosCanvas;
        buildTimeSlider.GetComponent<Slider>().value = timer;
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(startPos.y, targetPos.y, timer / totalTime), transform.position.z);
        if (timer >= totalTime) {
            transform.parent = GameObject.Find("Buildings").transform;
            gameObject.GetComponent<Building>().SetConstructed(true);
            if (transform.GetComponent<BuildingResourceDepot>())
                GameObject.Find("Buildings").GetComponent<BuildingManager>().AddResourceDepot(gameObject);
            Destroy(buildTimeSlider);
            LogWindow.Singleton.AddText(gameObject.GetComponent<Building>().GetName() + " constructed.");
            Destroy(this);
        }
    }
}
