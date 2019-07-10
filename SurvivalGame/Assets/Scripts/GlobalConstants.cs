using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstants {
    // Enums
    public enum Stage { RUINS = 0, STAGE1 = 1, STAGE2 = 2, STAGE3 = 3, FINISHED = 4, }
    public enum Resources { WATER, FOOD, OIL, STEEL, WOOD, POPULATION, CLEANWATER, COOKEDFOOD, KNICKKNACK, }
    public enum Needs { THIRST, HUNGER, COMFORT, HOME, }
    public enum Activities { WORKING, WORKINGOVERTIME, COLLECTINGWORK, RESTING, IDLE, REVOLTING, }
    public enum WorkReqs { SKILLED, UNSKILLED, NONE, }

    public static bool advGoalEnabled = true;

    public static bool ChildLabor;

    // Special Rocket Variables
    public static float oilUsageIncrease = 0.25f;
    public static int[] rocketCost = new int[2] { 30, 25 };
    public static int[] rocket2Cost = new int[2] { 30, 35 };
    public static int[] rocket3Cost = new int[2] { 30, 40 };
    public static int[] rocket4Cost = new int[2] { 35, 45 };
    public static Stage currentStage = Stage.RUINS;
    public static string[] spaceShipNames = new string[5] { "Strange ruins", "Old space rocket launcher", "Space rocket launcher", "Rocket of Salvation", "Rocket of Salvation", };

    // Humans
    public static int[] initialAdultAge = new int[2] { 13, 28 };
    public static int[] newRefugeeAge = new int[2] { 3, 45 };

    // Building costs (index 0 = wood, index 1 = steel)
    public static int[] homeCost = new int[2] { 15, 0 };
    public static int[] steelBuildingCost = new int[2] { 15, 0 };
    public static int[] woodBuildingCost = new int[2] { 10, 0 };
    public static int[] waterBuildingCost = new int[2] { 5, 15 };
    public static int[] canteenBuildingCost = new int[2] { 10, 0 };
    public static int[] resourceDepotCost = new int[2] { 25, 15 };
    public static int[] knickknackStoreCost = new int[2] { 20, 40 };
    public static int[] schoolCost = new int[2] { 15, 15 };
    public static int[] barCost = new int[2] { 75, 60 };

    // Building upgrade 2 costs (index 0 = wood, index 1 = steel)
    public static int[] home2Cost = new int[2] { 30, 0 };
    public static int[] steelBuilding2Cost = new int[2] { 15, 20 };
    public static int[] woodBuilding2Cost = new int[2] { 30, 10 };
    public static int[] waterBuilding2Cost = new int[2] { 10, 30 };
    public static int[] canteenBuilding2Cost = new int[2] { 20, 10 };
    public static int[] oilBuilding2Cost = new int[2] { 50, 75 };
    public static int[] resourceDepot2Cost = new int[2] { 35, 25 };
    public static int[] school2Cost = new int[2] { 100, 100 };
    public static int[] academyCost = new int[2] { 130, 180 };

    // Building upgrade 3 costs (index 0 = wood, index 1 = steel)
    public static int[] home3Cost = new int[2] { 40, 20 };
    public static int[] steelBuilding3Cost = new int[2] { 25, 30 };
    public static int[] woodBuilding3Cost = new int[2] { 40, 20 };
    public static int[] waterBuilding3Cost = new int[2] { 20, 40 };
    public static int[] canteenBuilding3Cost = new int[2] { 30, 20 };
    public static int[] oilBuilding3Cost = new int[2] { 90, 115 };
    public static int[] resourceDepot3Cost = new int[2] { 45, 45 };

    // Building upgrade 4 costs (index 0 = wood, index 1 = steel)
    public static int[] home4Cost = new int[2] { 60, 30 };
    public static int[] steelBuilding4Cost = new int[2] { 35, 45 };
    public static int[] woodBuilding4Cost = new int[2] { 50, 30 };
    public static int[] waterBuilding4Cost = new int[2] { 30, 50 };
    public static int[] canteenBuilding4Cost = new int[2] { 45, 30 };
    public static int[] oilBuilding4Cost = new int[2] { 130, 180 };
    public static int[] resourceDepot4Cost = new int[2] { 60, 60 };

    // Temperature thresholds
    public static int thirstThreshold = 30;
    public static float thirstDebuff = 0.1f;
    public static int comfortThreshold = 25;
    public static float comfortDebuff = 0.03f;

    // Oil production values
    public static float oilProduction1 = 1f;
    public static float oilProduction2 = 2f;
    public static float oilProduction3 = 3f;
    public static float oilProduction4 = 4f;

    // Global moral hits
    public static float eclipseMoral = -10f;
    public static float sandstormMoral = -15f;

    /// <summary>
    /// Gets the price of this building.
    /// </summary>
    /// <param name="building">The requested building.</param>
    /// <returns></returns>
    public static int[] GetBuildingCost(GameObject building) {
        string name = building.gameObject.name;
        if (name.Contains("Clone"))  // Workaround in order to get rid of the *(clone)-suffix on a duplication.
            name = name.Remove(name.Length - 7);
        switch (name) {
            case "BuildingWoodProduction":
                return woodBuildingCost;
            case "BuildingSteelProduction":
                return steelBuildingCost;
            case "BuildingWaterProduction":
                return waterBuildingCost;
            case "BuildingHome":
                return homeCost;
            case "BuildingCanteen":
                return canteenBuildingCost;
            case "BuildingResourceDepot":
                return resourceDepotCost;
            case "BuildingKnickknackStore":
                return knickknackStoreCost;
            case "BuildingSchool":
                return schoolCost;
            case "BuildingBar":
                return barCost;
            case "BuildingAcademy":
                return academyCost;
            default:
                return null;
        }
    }

    // Names
    private static string[] maleNames = new string[100] {
        "John", "Jorgie", "Woody", "Dominique", "Nabeel", "Niall", "Roman", "Kishan", "Calvin", "Alejandro", "Keane", "Fraser", "Bobby", "Yasin", "Oscar", "Lance", "Dane", "Philip", "Ryan", "Regan", "Robin",
        "Scott", "Piers", "Graham", "Andreas", "Caleb", "Yusuf", "Shay", "Brendon", "Uriel", "Davis", "Rehan", "Peter", "Tom", "Corey", "Piotr", "Blake", "Pierce", "Bellamy", "Usman", "Finn", "Toby",
        "Luke", "Malcolm", "Pike", "Drake", "Joshua", "Ethan", "Jack", "Nick", "Martin", "Danny", "Thor", "Trevor", "Jace", "Mustafa", "Ricardo", "Yao", "Jay", "Nicodemus", "Jim", "Zak", "Johnny", "Anton",
        "Callum", "Garfield", "Lee", "Casper", "Andy", "Miyazaki", "Tomaso", "Hauro", "Achilles", "Jaromir", "Mohammed", "Abdul", "Shigeru", "Eirik", "Torvald", "Stefan", "Eiligur", "Toralf", "Leif", "Harald",
        "Quintus", "Dong", "Shen", "Vladislav", "Joe", "Danyaal", "Wang", "Alfredo", "Joachim", "Erich", "Thomas", "Tobias", "Julian", "Ola", "Aapo", "Adolf"
    };

    private static string[] femaleNames = new string[100] {
        "Jenny", "Sigrid", "Misa", "Hennako", "Da-Yung", "Yi", "Sasha", "Natalia", "Octavia", "Liina", "River", "Tanja", "Moira", "Ariadne", "Jane", "Audrey", "Katarina", "Karin", "Adel", "Madison", "Jennifer",
        "Adel", "Abigail", "Inari", "Leena", "Anneli", "Eevi", "Alexandra", "Danica", "Tierney", "Fanya", "Heidi", "Mira", "Emilia", "Agustina", "Agnes", "Cecilia", "Zoe", "Renata", "Mia", "Juliette", "Emma",
        "Sarah", "Hannah", "Tina", "Alice", "Mary", "Jazmin", "Julia", "Clara", "Bertha", "Gertrud", "Frieda", "Edith", "Hildegard", "Lillemor", "Stina", "Christa", "Monika", "Ursula", "Anja", "Barbara", "Belinda",
        "Diana", "Dagny", "Ahana", "Anika", "Alisha", "Ira", "Jiya", "Olivia", "Ava", "Charlotte", "Susan", "Sophie", "Sofia", "Amalia", "Tracy", "Michelle", "Megan", "Samantha", "Bethany", "Kara", "Cleo", "Gaia",
        "Ebba", "Dorothea", "Dolly", "Doe", "Kimberly", "Zelda", "Kitty", "Katie", "Kay", "Pamela", "Kali", "Lulu", "Amina", "Yaakova", "Ylva"
    };

    /// <summary>
    /// Generate a name.
    /// </summary>
    /// <param name="isMale">Is the human a male or female?</param>
    /// <returns></returns>
    public static string Baptize(bool isMale) {
        int index = UnityEngine.Random.Range((int)0, (int)100);
        if (isMale)
            return maleNames[index];
        else if (!isMale)
            return femaleNames[index];
        else
            return "Tempson";
    }
}
