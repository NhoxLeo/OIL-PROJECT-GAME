using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandstorm : MonoBehaviour {
    private float sandstormTimer, timer;
    private float fadeOutTimer, fadeOutTime = 5f;
    private float destroyTickInterval, destroyTimer;
    private float maxVolume = 0.75f;   
    private AudioSource audioS;
    public WindZone wind;
    private bool isTransitioning, isIncreasing;
    private int maxBuildingsDestroyed, buildingsDestroyed;    
    [SerializeField]
    float defaultSandstormTimer = 60f;
    [SerializeField]
    float maxBuildingsDestroyedPercentage = 0.25f;
    [SerializeField]
    int destroyBuildingMaxOdds = 100;
    [SerializeField]
    int destroyOdds = 4;

    public void OnEnable() {
        maxBuildingsDestroyed = (int)(GameObject.Find("Buildings").transform.childCount * maxBuildingsDestroyedPercentage);
        buildingsDestroyed = 0;
        destroyTimer = 0;        
        isTransitioning = true;
        isIncreasing = true;
        audioS = GetComponent<AudioSource>();
        timer = 0;
        fadeOutTimer = 0;
        LogWindow.Singleton.AddText("<color=orange>A vicious sandstorm is upon us!</color>");
        GameObject.Find("Buildings").GetComponent<BuildingManager>().TurnOnOffLights(true);      
        audioS.Play();
        gameObject.GetComponent<ParticleSystem>().Play();
        audioS.volume = 0f;
        if(sandstormTimer <= 0)
            SetSandstorm();
        GameObject.Find("Population").GetComponent<PopulationManager>().ManipulateGlobalMoral("Sandstorm", GlobalConstants.sandstormMoral, sandstormTimer);
        destroyTickInterval = sandstormTimer / defaultSandstormTimer;
        wind.windTurbulence *= 3;
        wind.windMain *= 2;
    }

    /// <summary>
    /// Plays a sandstorm effect when active. Disables when finished.
    /// </summary>
    public void Update() {
        timer += Time.deltaTime;
        destroyTimer += Time.deltaTime;
        if (destroyTimer > destroyTickInterval) {
            DestroyBuilding();
            destroyTimer = 0;
        }       
        if (fadeOutTimer < fadeOutTime && isTransitioning && isIncreasing) {
            fadeOutTimer += Time.deltaTime / fadeOutTime;
            audioS.volume = Mathf.Lerp(0f, maxVolume, fadeOutTimer);
        }
        else if (fadeOutTimer >= fadeOutTime && isIncreasing) {
            isTransitioning = false;
            isIncreasing = false;
        }
        else if (timer >= sandstormTimer - fadeOutTime && !isTransitioning) {
            isTransitioning = true;
            fadeOutTimer = 0;
        }
        if (isTransitioning && !isIncreasing) {
            audioS.volume = Mathf.Lerp(maxVolume, 0f, fadeOutTimer);
            fadeOutTimer += Time.deltaTime / fadeOutTime;
            gameObject.GetComponent<ParticleSystem>().Stop();
        }
        if (timer > sandstormTimer) {
            LogWindow.Singleton.AddText("<color=orange>The sandstorm has passed.</color>");
            GameObject.Find("Buildings").GetComponent<BuildingManager>().TurnOnOffLights(false);
            GameObject.Find("Population").GetComponent<PopulationManager>().ManipulateGlobalMoral("Sandstorm", GlobalConstants.sandstormMoral, sandstormTimer);
            audioS.Stop();
            wind.windTurbulence /= 3;
            wind.windMain /= 2;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Destroys a building depending on the probability.
    /// </summary>
    private void DestroyBuilding() {
        if (Random.Range(0, destroyBuildingMaxOdds) > destroyBuildingMaxOdds - destroyOdds && maxBuildingsDestroyed > 0) {
            GameObject.Find("Buildings").GetComponent<BuildingManager>().BreakDownBuilding();
            maxBuildingsDestroyed--;
        }
    }

    /// <summary>
    /// Initiates a sandstorm.
    /// </summary>
    /// <param name="Darude">Has no effect what so ever.</param>
    public void SetSandstorm(bool Darude = false) {
        sandstormTimer = defaultSandstormTimer;
    }

    /// <summary>
    /// Initiates a sandstorm with a custom timer.
    /// </summary>
    /// <param name="overridedTimer">Custom sandstorm time.</param>
    public void SetSandstorm(float overridedTimer) {
        sandstormTimer = overridedTimer;
    }
}
