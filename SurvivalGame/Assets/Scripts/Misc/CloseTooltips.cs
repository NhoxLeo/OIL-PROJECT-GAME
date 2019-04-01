using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTooltips : MonoBehaviour {
    public void OnDisable() {
        Camera.main.transform.Find("CanvasTooltips").GetComponent<Tooltip>().DisableTooltip();
    }
}
