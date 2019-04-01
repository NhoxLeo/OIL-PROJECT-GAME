using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private static float timerStart = 2.0f, timer;
    private bool followMouse, indefinetely;
    public GameObject tooltipPanelPrefab, tooltipPanelSlimPrefab;
    private GameObject tooltipPanel;

    public void DisplayTooltip(string tooltip, bool followMouse, bool indefinetely = false, bool isSmallBox = false) {
        if (tooltipPanel)
            Destroy(tooltipPanel);
        if (!isSmallBox)
            tooltipPanel = Instantiate(tooltipPanelPrefab, transform);
        else
            tooltipPanel = Instantiate(tooltipPanelSlimPrefab, transform);
        //tooltipPanel.transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0f);
        tooltipPanel.transform.position = new Vector2(Input.mousePosition.x + 6, Input.mousePosition.y);
        tooltipPanel.transform.GetChild(0).GetComponent<Text>().text = tooltip;
        timer = timerStart;
        this.followMouse = followMouse;
        this.indefinetely = indefinetely;
    }

    public void DisplayTooltip(Transform parentTransform, string toolTip, bool followMouse, bool indefinetely = false) {

        //var newToolTip = tooltipPanel;
        // newToolTip.transform.localPosition

        var wantedPos = Camera.main.WorldToViewportPoint(parentTransform.position);
        transform.position = wantedPos;


        tooltipPanel = Instantiate(tooltipPanelPrefab, parentTransform);

        // tooltipPanel.transform.SetParent(parentTransform);

        //tooltipPanel.transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0f);
        ////tooltipPanel.transform.position = new Vector2(Input.mousePosition.x + 6, Input.mousePosition.y);
        ///
        var s = parentTransform.GetComponent<Text>().text;
        tooltipPanel.GetComponent<Text>().text = toolTip;// transform.GetChild(0).GetComponent<Text>().text = tooltip;

        tooltipPanel.SetActive(true);
        timer = timerStart;
        this.followMouse = followMouse;
        this.indefinetely = indefinetely;
    }

    // Not working
    public void SpawnFloatingText(string text) {
        //GameObject stringObject = new GameObject();
        //stringObject.transform.SetParent(transform);
        //stringObject.AddComponent<Text>();
        //stringObject.GetComponent<Text>().text = text;
        //stringObject.transform.position = Input.mousePosition;
        //stringObject.transform.SetParent(transform);
    }

    public void Update() {
        UpdateTooltip();
        //UpdateFloatingText();       
    }

    private void UpdateTooltip() {
        if (tooltipPanel) {
            if (Input.GetMouseButtonDown(1))
                timer = -1;
            if (tooltipPanel.gameObject.activeSelf) {
                if (!indefinetely)
                    timer -= Time.deltaTime;
                if (followMouse)
                    tooltipPanel.transform.position = new Vector2(Input.mousePosition.x + 6, Input.mousePosition.y);
                if (timer < 0)
                    tooltipPanel.gameObject.SetActive(false);
            }
            //if (timer <= 0.999f)
            //    tooltipPanel.transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(1f);
        }
    }

    // Not working
    private void UpdateFloatingText() {
        //foreach (Transform floatingText in transform) {
        //    floatingText.transform.localPosition = new Vector2(floatingText.transform.position.x, floatingText.transform.position.y - 0.005f);
        //}
    }

    public void DisableTooltip() {
        timer = -1;
        Destroy(tooltipPanel);
    }
}
