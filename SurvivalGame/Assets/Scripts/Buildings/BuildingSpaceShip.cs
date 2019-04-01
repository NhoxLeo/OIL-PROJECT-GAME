using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSpaceShip : Building {
    private short maxScientistAmount = 10;
    private int initialTimeUntilDone = 10;
    private float timer;
    private int[] timeUntilDone;
    private GameManager gameManager;
    private List<GameObject> scientists;
    private GameObject rocketInfoPanel;
    private string buildingName;
    private string infoString;
    private bool ableToUpgrade;
    public Slider slider;
    public Button advanceStageButton;

    public override void Start() {
        scientists = new List<GameObject>();
        rocketInfoPanel = GameObject.Find("");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        informationBox = GameObject.Find("InformationBox");
        buildings = GameObject.Find("Buildings");
        resourceManager = GameObject.Find("ResourceManager");
        timeUntilDone = new int[System.Enum.GetNames(typeof(GlobalConstants.Stage)).Length];
        for (int i = 0; i < timeUntilDone.Length; i++)
            timeUntilDone[i] = initialTimeUntilDone * (i + 1);
        buildingName = GlobalConstants.spaceShipNames[0];
        interactionRadius = 1.75f;
        isOperational = false;
        isBroken = false;
        ableToUpgrade = false;
        oilUsage = 0.5f;
        infoString = "";
        timer = timeUntilDone[0];
        slider.maxValue = timeUntilDone[0];
    }

    // TODO: Set a cost for repairing the spaceship.

    public override void Update() {
        //if (GlobalConstants.currentStage != GlobalConstants.Stage.RUINS) {    // Controlled by event in first stage
        if (timer >= 0) {
            timer -= Time.deltaTime /** (scientists.Count / 2)*/;   // When we have skilled workers
            slider.value = slider.maxValue - timer;
        }
        else if (!ableToUpgrade) {
            ableToUpgrade = true;
            advanceStageButton.interactable = true;
        }
        base.Update();
        //}      
    }

    public bool AddScientist(GameObject scientist) {
        if (scientists.Count < maxScientistAmount) {
            scientists.Add(scientist.gameObject);
            return true;
        }
        return false;
    }

    public GameObject RemoveScientist() {
        GameObject tempScientist = null;
        tempScientist = scientists[0];
        scientists.Remove(scientists[0]);
        return tempScientist;
    }

    public void UpgradeStage() {
        if (ableToUpgrade && resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -GlobalConstants.rocketCost[0]) && resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -GlobalConstants.rocketCost[1])) {
            GlobalConstants.currentStage++;
            LogManager.Singleton.CreateLogStatusOnSpaceShipUpgrade(LogType.StageChange);
            oilUsage += GlobalConstants.oilUsageIncrease;
            buildingName = GlobalConstants.spaceShipNames[(int)GlobalConstants.currentStage];
            slider.maxValue = timeUntilDone[(int)GlobalConstants.currentStage];
            advanceStageButton.interactable = false;
            timer = timeUntilDone[(int)GlobalConstants.currentStage];
            ableToUpgrade = false;
            LogWindow.Singleton.AddText("We have developed the " + buildingName + ".");
            SetInformationText(null);
            if (GlobalConstants.currentStage >= GlobalConstants.Stage.FINISHED)
                gameManager.WinGame();
            switch ((int)GlobalConstants.currentStage) {
                case 1:
                    infoString = "\nScientists: " + scientists.Count + "/" + maxScientistAmount + "\n";
                    GameObject.Find("AdvanceStage").transform.GetChild(0).GetComponent<Text>().text = "Develop";
                    break;
                default:
                    infoString = "\nScientists: " + scientists.Count + "/" + maxScientistAmount + "\n";
                    break;
            }
        }
        else if (ableToUpgrade) {
            MessageDisplay.DisplayTooltip("Not enough resources to develop.", true, false, true);
            SoundManager.PlaySound("beepOpen");
        }
        else if (!ableToUpgrade) {
            MessageDisplay.DisplayTooltip("More research is required.", true, false, true);
            SoundManager.PlaySound("beepOpen");
        }
    }


    public override void SetBuildingUIType(GameObject buildingTypes) {
        foreach (Transform item in buildingTypes.transform) {
            var UIbuilding = item.gameObject;
            if (UIbuilding.name != "RocketPanel")
                UIbuilding.SetActive(false);
            else
                UIbuilding.SetActive(true);
        }
    }

    public override void SetInformationText(GameObject objectInfo) {
        GameObject.Find("ClickedObjectInfo").GetComponent<Text>().text = buildingName + infoString;
    }

    public override void SetBrokenStatus(bool isBroken) {
        this.isBroken = isBroken;
        LogWindow.Singleton.AddText("The " + buildingName + " has broken down - it needs repairs!");
    }

    public override void DestroyBuilding() { }
    public override void Upgrade() { }
}
