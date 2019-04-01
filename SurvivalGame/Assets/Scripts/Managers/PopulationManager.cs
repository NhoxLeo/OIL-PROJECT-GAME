using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Manages the population, their needs and behaviours.
/// </summary>
public class PopulationManager : MonoBehaviour
{
    private GameObject buildings;
    private GameObject informationBox;
    public GameObject humanPrefab;
    public GameObject babyPrefab;
    // private List<GameObject> unemployed, homeless;
    public PopulationCollection populationCollection;
    private GameObject rm;
    private IEnumerator coroutine;
    private float totalMoral;
    List<int> moralBoosts = new List<int>(); //TODO: make dynamic to save space?
    List<int> moralDurations = new List<int>();
    List<int> moralTimers = new List<int>();
    double timer;
    [SerializeField]
    [Header("Debug/Tweaking:")]
    private int startingPop = 25;
    private float updateInterval = 2.0f;
    [SerializeField]
    [Range(-1f, 1f)]
    private float hungerMultiplier = -0.5f, comfortMultiplier = -0.4f, thirstMultiplier = -1f, homeMultiplier = -0.3f;
    [SerializeField]
    [Range(0.5f, 4.0f)]
    private float humanSpeed = 2.4f;

    public void Awake()
    {
        buildings = GameObject.Find("Buildings");
        informationBox = GameObject.Find("InformationBox");
        rm = GameObject.Find("ResourceManager").gameObject;
        populationCollection = new PopulationCollection();
        humanPrefab.GetComponent<NavMeshAgent>().speed = humanSpeed;
        // Spawns initial population
        for (int i = 0; i < startingPop; i++)
        {
            GameObject tempHuman = Instantiate(humanPrefab, new Vector3(20, 1, 50), new Quaternion());
            tempHuman.transform.parent = transform;
            tempHuman.GetComponent<NavMeshAgent>().Warp(new Vector3(20, 1, 50));
        }
        // Populates all humans at the start of the game to the "unemployed"-list.
        foreach (Transform human in this.transform)
        {
            populationCollection.Add(human.gameObject);
        }
        coroutine = UpdatePopulationStatus();
        StartCoroutine(coroutine);
    }

    public short GetHomelessCount()
    {
        return (short)populationCollection.HomelessList.Count;
    }

    public short GetUnemployedCount()
    {
        return (short)populationCollection.UnemployedList.Count;
    }

    /// <summary>
    /// This is only for the debug script to be able to add new humans.
    /// </summary>
    /// <param name="newDebugHuman"></param>
    public void AddHuman(GameObject newDebugHuman)
    {
        populationCollection.Add(newDebugHuman);
    }

    public void AddPopulationRange(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject tempHuman = Instantiate(humanPrefab, new Vector3(20, 1, 50), new Quaternion());
            tempHuman.transform.parent = transform;
            tempHuman.GetComponent<NavMeshAgent>().Warp(new Vector3(20, 1, 50));
        }
        foreach (Transform human in this.transform)
        {
            populationCollection.Add(human.gameObject);
        }
    }

    public void Update()
    {
        //DebugUpdate();
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            UpdateMoralTimers();
            timer = 0;
        }
    }

    public void AgeWorkers()
    {
        foreach (Transform human in transform)
            if (human.GetComponent<Human>().AgeService != null)
            {
                human.GetComponent<Human>().AgeService.UpdateAge();// IncreaseAge();
            }
    }

    void UpdateMoralTimers()//TODO: too clunky, make better
    {
        if (moralBoosts.Count > 0)
        {
            for (int i = 0; i < moralBoosts.Count; i++)
            {
                moralTimers[i] += 1;
                if (moralTimers[i] >= moralDurations[i])
                {
                    moralBoosts.RemoveAt(i);
                    moralTimers.RemoveAt(i);
                    moralDurations.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    /// <summary>
    /// Updates the status of population-related stuff, such as home status etc.
    /// </summary>
    /// <param name="interval">How often the manager update the status.</param>
    /// <returns></returns>
    private IEnumerator UpdatePopulationStatus()
    {
        while (true)
        {
            FindHomeToHomeless();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    /// <summary>
    /// Assign or remove a worker a workplace.
    /// </summary>
    /// <param name="hasJob">hasJob = true means that the worker is now employed here.</param>
    public void AssignWork(bool hasJob)
    {
        // Kör hårt, refactora den här skiten ;) 
        //I like dis:D
        GameObject currentlySelectedBuilding = buildings.GetComponent<BuildingManager>().GetSelectedBuilding().gameObject;

        if (currentlySelectedBuilding.GetComponent<Building>().GetWorkReqs() != GlobalConstants.WorkReqs.NONE)
        {
            // Workplace reqs-----------------------------------------------------------------------------------------
            if (currentlySelectedBuilding.GetComponent<Building>().GetWorkReqs() == GlobalConstants.WorkReqs.SKILLED)
            {//(Skilled)
                if (hasJob)
                {   // Add worker 
                    GameObject human = populationCollection.GetSkilledPerson;
                    if (human != null)
                    {
                        if (!currentlySelectedBuilding.tag.Contains("SpaceShip"))
                        {
                            if (currentlySelectedBuilding.GetComponent<BuildingProduction>().AddWorker(human))
                            {
                                human.GetComponent<HumanStateMachine>().ChangeWorkState(currentlySelectedBuilding);
                                currentlySelectedBuilding.GetComponent<Building>().SetInformationText(GameObject.Find("BuildingInfo").gameObject);
                            }
                        }
                        else
                        {  // If spaceship
                            if (currentlySelectedBuilding.GetComponent<BuildingSpaceShip>().AddScientist(human))
                            {
                                human.GetComponent<HumanStateMachine>().ChangeWorkState(currentlySelectedBuilding);
                                currentlySelectedBuilding.GetComponent<Building>().SetInformationText(GameObject.Find("BuildingInfo").gameObject);
                            }
                        }
                    }
                }

                else
                {  // Remove worker
                    if (!currentlySelectedBuilding.tag.Contains("SpaceShip"))
                    {
                        try
                        {
                            currentlySelectedBuilding.GetComponent<BuildingProduction>().RemoveWorkers();
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {   // If spaceship
                            GameObject tempWorker = currentlySelectedBuilding.GetComponent<BuildingSpaceShip>().RemoveScientist();
                            tempWorker.GetComponent<HumanStateMachine>().ChangeWorkState(null);
                        }
                        catch { }
                    }
                }
            }
            else if (currentlySelectedBuilding.GetComponent<Building>().GetWorkReqs() == GlobalConstants.WorkReqs.UNSKILLED)
            {
                // Unimplemented, this is for hunters etc.
            }
        }
        else
        {  // No workplace reqs---------------------------------------------------------------------------------
           //Any worker..
            if (hasJob)
            {
                var tempUnemployedPerson = populationCollection.GetUnemployedPerson;

                if (tempUnemployedPerson != null)
                {
                    if (currentlySelectedBuilding.GetComponent<BuildingProduction>().AddWorker(tempUnemployedPerson))
                    {
                        tempUnemployedPerson.GetComponent<HumanStateMachine>().ChangeWorkState(currentlySelectedBuilding);
                        currentlySelectedBuilding.GetComponent<Building>().SetInformationText(GameObject.Find("BuildingInfo").gameObject);
                    }
                }
            }
            else
            {
                try
                {
                    currentlySelectedBuilding.GetComponent<BuildingProduction>().RemoveWorkers();
                    //GameObject tempWorker = currentlySelectedBuilding.GetComponent<BuildingProduction>().RemoveWorkers();
                    //tempWorker.GetComponent<HumanStateMachine>().ChangeWorkState(null);
                }
                catch { }
            }
        }
        currentlySelectedBuilding.GetComponent<Building>().SetInformationText(GameObject.Find("BuildingInfo").gameObject);
    }

    public void AssignWorkToWorkSite(string workSiteType)
    {
        GameObject currentlySelectedBuilding = buildings.GetComponent<BuildingManager>().GetSelectedBuilding().gameObject;

        if (workSiteType == "ForestSite")
        {
            currentlySelectedBuilding.GetComponent<BuildingResourceDepot>().SetCurrentWorkSiteType(WorkSiteType.Forest);
        }
        else if (workSiteType == "MetalSite")
        {
            currentlySelectedBuilding.GetComponent<BuildingResourceDepot>().SetCurrentWorkSiteType(WorkSiteType.MetalScraps);
        }
        else if (workSiteType == "HuntingSite")
        {
            currentlySelectedBuilding.GetComponent<BuildingResourceDepot>().SetCurrentWorkSiteType(WorkSiteType.Hunting);
        }
        var tempUnemployedPerson = populationCollection.GetUnemployedPerson;// GetUnemployedPersonByAge(HumanAgeService.AgeCategory.Adult);
        if (tempUnemployedPerson != null)
        {
            if (currentlySelectedBuilding.GetComponent<BuildingProduction>().AddWorker(tempUnemployedPerson))
            {
                tempUnemployedPerson.GetComponent<HumanStateMachine>().ChangeWorkState(currentlySelectedBuilding);
                currentlySelectedBuilding.GetComponent<Building>().SetInformationText(GameObject.Find("BuildingInfo").gameObject);
            }
        }
    }

    public void RemovePlayerFromWorkSite(string workSiteType)
    {
        GameObject currentlySelectedBuilding = buildings.GetComponent<BuildingManager>().GetSelectedBuilding().gameObject;

        try
        {
            currentlySelectedBuilding.GetComponent<BuildingProduction>().RemoveWorkers(workSiteType);
        }
        catch { }
        currentlySelectedBuilding.GetComponent<Building>().SetInformationText(GameObject.Find("BuildingInfo").gameObject);
    }

    /// <summary>
    /// Checks if there are any unoccupied houses, in which case homeless people will be assigned to them.
    /// </summary>
    public void FindHomeToHomeless()
    {
        var tempHomelessList = populationCollection.HomelessList;

        if (tempHomelessList != null && tempHomelessList.Count > 0)
        {
            List<GameObject> tempList = new List<GameObject>();
            foreach (Transform building in buildings.transform)
            {
                if (building.tag.Contains("BuildingResidence"))
                {
                    tempList = building.GetComponent<BuildingHome>().GetOccupantList();
                    if (tempList != null)
                    {
                        int freeSlots = building.GetComponent<BuildingHome>().GetMaxOccupants() - tempList.Count;
                        for (int i = 0; i < freeSlots; i++)
                        {
                            var homeLessPerson = populationCollection.GetHomelessPerson;
                            if (homeLessPerson)
                            {
                                tempList.Add(homeLessPerson);
                                homeLessPerson.GetComponent<Human>().SetHome(building.gameObject);
                            }
                        }
                        building.GetComponent<BuildingHome>().SetOccupantList(tempList);
                        if (informationBox.transform.GetChild(0).gameObject.activeSelf && building.GetComponent<Building>().IsDisplayingInfo())
                            building.GetComponent<BuildingHome>().SetInformationText(informationBox.transform.Find("ObjectInfo").transform.Find("BuildingInfo").gameObject);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Remove/assign a child to a school.
    /// </summary>
    /// <param name="beginEducation">True if the child will start going to school.</param>
    public void AssignPupil(bool beginEducation)
    {
        GameObject currentlySelectedBuilding = GameObject.Find("Buildings").GetComponent<BuildingManager>().GetSelectedBuilding();

        if (currentlySelectedBuilding.tag.Contains("School"))
        {
            if (beginEducation)
            {
                var tempChild = populationCollection.GetUnemployedPersonByAge(HumanAgeService.AgeCategory.Child);
                if (tempChild != null)
                {
                    if (currentlySelectedBuilding.GetComponent<BuildingSchool>().AssignPupil(tempChild))
                    {
                        tempChild.GetComponent<HumanStateMachine>().ChangeWorkState(currentlySelectedBuilding);
                    }
                }
            }
            else
            {
                // GameObject.Find("Buildings").GetComponent<BuildingManager>().GetSelectedBuilding().GetComponent<BuildingSchool>().RemovePupil();
                // TODO: Remove a child in a school here (transform!)
            }
        }
    }
    public void AssignStudent(bool beginEducation)
    {
        GameObject currentlySelectedBuilding = GameObject.Find("Buildings").GetComponent<BuildingManager>().GetSelectedBuilding();

        if (currentlySelectedBuilding.tag.Contains("Academy"))
        {
            if (beginEducation)
            {
                var student = populationCollection.GetUnemployedPersonByAge(HumanAgeService.AgeCategory.Adult);
                if (student != null && !student.GetComponent<Human>().IsSpecialized) //would make no sense to assign already educated people to academy again
                {
                    if (currentlySelectedBuilding.GetComponent<BuildingAcademy>().AssignStudent(student))
                    {
                        student.GetComponent<HumanStateMachine>().ChangeWorkState(currentlySelectedBuilding);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns the total moral of the population.
    /// </summary>
    /// <returns></returns>
    public float TotalMoral()
    {
        totalMoral = 0f;
        foreach (Transform human in transform)
        {
            totalMoral += human.GetComponent<Human>().GetMoral();
        }

        return (totalMoral /= transform.childCount) + GetMoralBoostSum();
    }

    /// <summary>
    /// doesn't have to be positive, can have negative "boosts" aswell
    /// </summary>
    /// <returns></returns>
    private int GetMoralBoostSum()
    {
        int temp = 0;
        foreach (int i in moralBoosts)
        {
            temp += i;
        }
        return temp;
    }

    public void AddNewMoralBoost(int newBoost, int duration)
    {
        moralBoosts.Add(newBoost);
        moralDurations.Add(duration);
        moralTimers.Add(0); // TODO: lookover for more efficiency
    }

    public List<Human> GetHumans(int belowMoralValue)
    {
        List<Human> humans = new List<Human>();
        foreach (Transform human in transform)
        {
            if (human.GetComponent<Human>().GetMoral() <= belowMoralValue)
                humans.Add(human.GetComponent<Human>());
        }
        return humans;
    }
    /// <summary>
    /// old function, gets the human with the lowest moral value
    /// </summary>
    /// <returns></returns>
    public Transform GetHuman()
    {
        Transform humanTransform = transform.GetChild(0).transform;
        Transform result = null;
        foreach (Transform human in transform)
        {
            if (humanTransform != human)
            {
                humanTransform = human;
                if (human.GetComponent<Human>().GetMoral() < humanTransform.GetComponent<Human>().GetMoral())
                {
                    result = human;
                }
            }
        }
        //mostUnhappySOB.GetComponent<Human>().BreakDown();
        return result;
    }


    /// <summary>
    /// Makes the list of occupants homeless.
    /// </summary>
    /// <param name="occupants">The list of occupants.</param>
    public void MakeHomeless(List<GameObject> occupants)
    {
        foreach (GameObject occupant in occupants)
        {
            //homeless.Add(occupant);
            occupant.GetComponent<Human>().SetHome(null);
        }
    }

    /// <summary>
    /// Makes the list of workers unemployed.
    /// </summary>
    /// <param name="workers">The list of workers.</param>
    public void MakeUnemployed(List<GameObject> workers)
    {
        //foreach (GameObject worker in workers)
        //{
        //  unemployed.Add(worker);
        //}
    }

    /// <summary>
    /// Notifies the Population Manager about a death.
    /// </summary>
    /// <param name="deadHuman">The dead human.</param>
    public void NotifyDeath(GameObject deadHuman, bool hasJob, bool hasHome)
    {
        rm.GetComponent<ResourceManager>().ResourceConv("Population", -1);
        //rm.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.POPULATION, -1);
        populationCollection.Remove(deadHuman);
    }

    /// <summary>
    /// Switches on/off raycasting for all humans.
    /// </summary>
    /// <param name="raycastOn">Raycast switches on if raycastOn == true.</param>
    public void SwitchUnitRaycasting(bool raycastOn)
    {
        if (!raycastOn)
        {
            foreach (Transform human in transform)
                human.gameObject.layer = 2;
        }
        else
        {
            foreach (Transform human in transform)
                human.gameObject.layer = 0;
        }
    }

    /// <summary>
    /// Sets a global moral hit
    /// </summary>
    /// <param name="moralMagnitude">The magnitude of the moral hit.</param>
    public void ManipulateGlobalMoral(string buffName, float buffMagnitude, float buffDuration, bool isPermanent = false)
    {
        if (isPermanent)
        {
            foreach (Transform human in transform)
                human.GetComponent<Human>().AddBuff(new Buff(buffName, buffMagnitude, human.gameObject));
        }
        else
        {
            foreach (Transform human in transform)
                human.GetComponent<Human>().AddBuff(new Buff(buffName, buffMagnitude, buffDuration, human.gameObject));
        }

    }

    /// <summary>
    /// Returns the current standards for modifiers.
    /// </summary>
    /// <returns></returns>
    public float[] GetModifiers()
    {
        return new float[4] { hungerMultiplier, comfortMultiplier, thirstMultiplier, homeMultiplier };
    }

    /// <summary>
    /// Upgrades all worker clothes and the std comfort modifier.
    /// </summary>
    public void UpgradeClothing(float amount)
    {
        foreach (Transform human in transform)
        {
            human.GetComponent<Human>().UpgradeClothing(amount);
        }
        comfortMultiplier += amount;
    }
    /// <summary>
    /// Increases maxOccupants in Homes
    /// Gives buff 10 in moral for homeless, decrease 10 in moral for those that has a home
    /// </summary>
    public void ShareBeds(bool buffActivated, float combinedBuffMultiplier)
    {
        foreach (GameObject building in buildings.GetComponent<BuildingManager>().GetBuildingsOfType("BuildingHome(Clone)"))
        {
            building.GetComponent<BuildingHome>().ShareBeds(buffActivated);
        }
        foreach (Transform human in transform)
        {
            if (buffActivated)
            {
                if (human.GetComponent<Human>().HasHome)
                {
                    human.GetComponent<Human>().SetMoralMultiPlier(combinedBuffMultiplier);
                    //human.GetComponent<Human>().AddBuff(new Buff("Share beds", -10, human.gameObject));
                }
                else
                {
                    human.GetComponent<Human>().SetMoralMultiPlier(combinedBuffMultiplier - 0.1f);
                    //human.GetComponent<Human>().AddBuff(new Buff("Share beds", 10, human.gameObject));
                }
            }
            else
            {
                human.GetComponent<Human>().SetMoralMultiPlier(combinedBuffMultiplier);
                //human.GetComponent<Human>().RemoveBuff("Share beds");
            }
        }
    }
    public void EatTrashFood(bool buffActivated, float combinedBuffMultiplier)
    {
        foreach (Transform human in transform)
        {
            if (buffActivated)
            {
                human.GetComponent<Human>().SetMoralMultiPlier(combinedBuffMultiplier);
                human.GetComponent<Human>().EatTrashFood(20);
            }
            else
                human.GetComponent<Human>().SetMoralMultiPlier(combinedBuffMultiplier);

            //human.GetComponent<Human>().EatTrashFood(20); //moral decrease by 20, comfort decrease by 20
            //human.GetComponent<Human>().AddBuff(new Buff("Eat trashfood", -10, human.gameObject));
        }
    }
    /// <summary>
    /// this is.......#sad, looks horrendous, also only kills one human, also creates a nullref except in LogWindow.cs:7
    /// </summary>
    public void EatPeople()
    {
        Human h = populationCollection[0].GetComponent<Human>();
        // populationCollection.RemoveAt(0);
        // h.GetComponent<Human>().EatPeople();
        h.EatPeople();

        foreach (Transform human in transform)
        {
            human.GetComponent<Human>().AddBuff(new Buff("Eats people", -20, human.gameObject));
            human.GetComponent<Human>().DecreaseHunger(20);
        }
    }

    public void ChildLabor(bool buffActivated, float combinedBuffMultiplier)
    {

        foreach (Transform human in transform)
        {
            if (buffActivated)
                human.GetComponent<Human>().SetMoralMultiPlier(combinedBuffMultiplier);
            else
                human.GetComponent<Human>().SetMoralMultiPlier(combinedBuffMultiplier);

            // human.GetComponent<Human>().EatTrashFood(20); //moral decrease by 20, comfort decrease by 20
            //human.GetComponent<Human>().AddBuff(new Buff("Eat trashfood", -10, human.gameObject));
        }
        GlobalConstants.ChildLabor = buffActivated;
    }

    public void CreateChild(GameObject parent)
    {
        var humanService = parent.GetComponent<Human>();
        GameObject child = Instantiate(babyPrefab, parent.transform.position, Quaternion.identity);
        child.transform.parent = transform;
        var childService = child.GetComponent<Human>().AgeService;
        populationCollection.Add(child);
        rm.GetComponent<ResourceManager>().ResourceConv("Population", 1);
        Debug.Log(humanService.GetName());// + " gave birth to " + childService.Name);
    }



    /// <summary>
    /// Upgrades the workers' shoes, increasing their speed.
    /// </summary>
    /// <param name="amount"></param>
    public void UpgradeShoes(float amount)
    {
        foreach (Transform human in transform)
        {
            human.GetComponent<NavMeshAgent>().speed += amount;
        }
        humanPrefab.GetComponent<NavMeshAgent>().speed += amount;
    }

    public List<GameObject> ValidateWorkerList(ref List<GameObject> workerList)
    {
        int tempCounter = 0;
        List<GameObject> tempRemoveList = new List<GameObject>();

        foreach (GameObject item in workerList)
        {
            foreach (Transform human in this.transform)
            {
                if(item == null)
                {                    
                    break;
                }
                else if (item.GetComponent<Human>() == human.GetComponent<Human>())
                {
                    tempCounter++;
                    break;
                }                
            }
            if (tempCounter == 0)
            {
                tempRemoveList.Add(item);
            }
            tempCounter = 0;
        }
        for (int i = 0; i < tempRemoveList.Count; i++)
        {
            workerList.Remove(tempRemoveList[i]);
        }
       
        return workerList;
    }

    //-------------------DEBUG--------------------------------------
    //public void DebugUpdate()
    //{
    //    GameObject debugScreen = Camera.main.transform.Find("Canvas").transform.Find("DebugScreen").gameObject;
    //    string stringToShow = "DEBUG INFORMATION (moral: " + (int)TotalMoral() + ")";
    //    foreach (Transform human in this.transform)
    //    {
    //        stringToShow += "\n\nName: " + human.GetComponent<Human>().GetName() + " (" + human.GetComponent<Human>().AgeService.Age + " yrs) " + human.GetComponent<Human>().IsSKilledWorker + "\nHunger: "
    //            + human.GetComponent<Human>().GetHunger() + ",\tThirst: " + human.GetComponent<Human>().GetThirst() + ", Comfort: "
    //            + human.GetComponent<Human>().GetComfort() + ",\tHome: " + human.GetComponent<Human>().GetHome()
    //                                            + "\nActivity: " + human.GetComponent<HumanStateMachine>().GetState();
    //    }
    //    debugScreen.transform.GetChild(0).GetComponent<Text>().text = stringToShow;
    //}
}


/// <summary>
/// A List class that will be used to get specific humans based on their variables. Not finished.
/// </summary>
public class PopulationCollection : List<GameObject>
{

    class AgeComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            var frst = (int)x.GetComponent<Human>().GetAgeCategory();
            var second = (int)y.GetComponent<Human>().GetAgeCategory();

            return frst.CompareTo(second);
        }
    }
    public List<GameObject> HomelessList { get { return this.Where(o => o.GetComponent<Human>().HasHome == false).ToList(); } }

    public GameObject GetHomelessPerson
    {
        get
        {
            var tempList = this.Where(o => o.GetComponent<Human>().HasHome == false).ToList();
            if (tempList.Count > 0)
            {
                return tempList.OrderBy(o => o.GetComponent<Human>().GetHomeStatus()).First();
            }
            return null;
        }
    }

    public List<GameObject> UnemployedList { get { return this.Where(o => o.GetComponent<Human>().HasJob == false).ToList(); } }

    public GameObject GetUnemployedPerson
    {
        get
        {
            GameObject person;
            Sort(new AgeComparer());
            for (int i = 0; i < Count; i++)
            {
                if (this[i].GetComponent<Human>().HasJob == false)
                {
                    return person = this[i];
                }
            }
            return null;
        }
    }

    public List<GameObject> GetAllUnemployedAdults()
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < Count; i++)
        {
            if (this[i].GetComponent<Human>().GetAgeCategory() == HumanAgeService.AgeCategory.Adult
                && !this[i].GetComponent<Human>().HasJob)
            {
                temp.Add(this[i]);
            }
        }
        return temp;
    }

    public List<GameObject> SkilledWorkerList
    {
        get
        {
            return this.Where(o => o.GetComponent<Human>().HasJob == false &&
            o.GetComponent<Human>().IsSkilledWorker == true).ToList();
        }
    }

    public GameObject GetSkilledPerson
    {
        get
        {
            var tempList = this.Where(o => o.GetComponent<Human>().HasJob == false &&
            o.GetComponent<Human>().IsSkilledWorker == true).ToList();
            if (tempList.Count > 0)
            {
                return tempList[0];
            }
            return null;
        }
    }

    public GameObject GetUnemployedPersonByAge(HumanAgeService.AgeCategory ageCategory)
    {
        //errors here
        var tempList = this.Where(o => o.GetComponent<Human>().GetAgeCategory() == ageCategory &&
        o.GetComponent<Human>().HasJob == false).ToList();
        if (tempList.Count > 0)
        {
            return tempList[0];
        }
        return null;
    }


    public GameObject GetPersonByAge(HumanAgeService.AgeCategory ageCategory)
    {
        var tempList = this.Where(o => o.GetComponent<Human>().GetAgeCategory() == ageCategory).ToList();
        if (tempList.Count > 0)
        {
            return tempList[0];
        }
        return null;
    }

}