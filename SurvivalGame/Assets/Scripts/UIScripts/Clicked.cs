using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clicked : MonoBehaviour
{

    public EventManager em;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void SendPlayerDecision()
    {
        em.SetDecision(button);
    }
}
