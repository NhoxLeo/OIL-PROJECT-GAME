using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWarningFlash : MonoBehaviour
{   
    public EventManager em;
    Button button;
    [Header("Timer")]    
    public int timeUntilAutoChoose = 15;

    double timer = 0;
    double flipTImer = 0;
    double interval = 3, flipInterval = .3f;
    int counter;
    Color oldColor;
    private void Awake()
    {
        button = GetComponent<Button>();
        oldColor = button.colors.normalColor;

        counter = timeUntilAutoChoose;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval && counter > 0)
        {
            if (counter < 6)
                interval = .5f;
            if (flipTImer == 0)
                FlipFlop(Color.yellow);
            flipTImer += Time.deltaTime;
            if (flipTImer >= flipInterval)
            {
                FlipFlop(oldColor);
                timer = 0;
                flipTImer = 0;
                counter--;
            }
        }
        if (counter <= 0)
        {
            counter = timeUntilAutoChoose;
            GetComponent<PanelHandler>().EnableAndDisablePanel();
            em.OpenedEvent();
        }
    }

    void FlipFlop(Color color)
    {
        var colors = GetComponent<Button>().colors;
        colors.normalColor = color;
        GetComponent<Button>().colors = colors;
    }

    public void Reset()
    {
        counter = timeUntilAutoChoose;
        timer = 0;
        Awake();
    }
}
