using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eclipse : MonoBehaviour
{
    public GameObject temperaturePanel;
    private float timer, eclipseTimer, transitionTimer;
    private Color sunColor, eclipseColor;
    private Color sunFog, eclipseFog;
    private bool isDarkening, isTransitioning;
    private int oldTemperature;
    [SerializeField]
    int eclipseTemperature = -20;
    [SerializeField]
    float defaultEclipseTimer = 60f;

    private Color currentLightingColor, currentFogColor;

    public void OnEnable()
    {
        isDarkening = true;
        isTransitioning = true;
        sunColor = new Color(1.0f, 0.769f, 0.553f, 1.0f);
        eclipseColor = new Color(0.286f, 0.227f, 0.451f, 1.0f);
        sunFog = new Color(0.376f, 0.278f, 0.224f, 1.0f);
        eclipseFog = new Color(0.051f, 0f, 0.114f, 1.0f);
        LogWindow.Singleton.AddText("<color=blue>An eclipse is upon us!</color>");
        GameObject.Find("Buildings").GetComponent<BuildingManager>().TurnOnOffLights(true);
        if (eclipseTimer <= 0)//TODO: Martin jag satte en enkel check som kollar om man redan satt eclipseTimer(genom överlagringen där nere)
             SetEclipse();    //får själv avgöra om det är lämligt, annars kan du ta bort anropet, då P är för debug alla andra fall kommer man kalla på den ena eller andra SetEclipse
        GameObject.Find("Population").GetComponent<PopulationManager>().ManipulateGlobalMoral("Eclipse", GlobalConstants.eclipseMoral, eclipseTimer);
        oldTemperature = temperaturePanel.GetComponent<EnvironmentManager>().Temperature;
        temperaturePanel.GetComponent<EnvironmentManager>().SetTemperature(eclipseTemperature, transitionTimer, true);
    }

    /// <summary>
    /// Runs while the eclipse is active
    /// </summary>
    public void Update()
    {
        timer += Time.deltaTime / transitionTimer;
        eclipseTimer -= Time.deltaTime;
        if (timer >= transitionTimer && isDarkening)
        {
            isDarkening = false;
            isTransitioning = false;
            timer = 0;
        }
        if (eclipseTimer <= transitionTimer && !isDarkening && !isTransitioning)
        {
            temperaturePanel.GetComponent<EnvironmentManager>().SetTemperature(oldTemperature, transitionTimer, true);
            isTransitioning = true;
            timer = 0;
        }
        if (eclipseTimer <= 0)
        {
            LogWindow.Singleton.AddText("<color=orange>The eclipse is over, thank God!</color>");
            gameObject.GetComponent<Eclipse>().enabled = false;
            GameObject.Find("Buildings").GetComponent<BuildingManager>().TurnOnOffLights(false);
        }

        if (isDarkening && isTransitioning)
        {
            currentLightingColor = Color.Lerp(sunColor, eclipseColor, timer);
            currentFogColor = Color.Lerp(sunFog, eclipseFog, timer);
            gameObject.GetComponent<Light>().color = currentLightingColor;
            RenderSettings.fogColor = currentFogColor;
        }
        else if (!isDarkening && isTransitioning)
        {
            currentLightingColor = Color.Lerp(eclipseColor, sunColor, timer);
            currentFogColor = Color.Lerp(eclipseFog, sunFog, timer);
            gameObject.GetComponent<Light>().color = currentLightingColor;
            RenderSettings.fogColor = currentFogColor;
        }
    }

    /// <summary>
    /// Starts an eclipse with its default length (with random offset)
    /// </summary>
    public void SetEclipse()
    {
        eclipseTimer = defaultEclipseTimer;
        transitionTimer = eclipseTimer / 12f;
        timer = 0;
    }

    /// <summary>
    /// Starts an eclipse with custom length and without random offset
    /// </summary>
    /// <param name="overridedTimer"></param>
    public void SetEclipse(float overridedTimer)
    {
        eclipseTimer = overridedTimer;
        transitionTimer = eclipseTimer / 12f;
        timer = 0;
    }
    
}
