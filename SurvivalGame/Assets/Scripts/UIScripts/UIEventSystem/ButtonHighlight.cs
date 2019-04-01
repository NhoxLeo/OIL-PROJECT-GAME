using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour {

    public Button button;

    public void HiglightOff()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
