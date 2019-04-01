using System;

[Serializable]
public class LogStatus
{
  public LogType LogType { get; set; }
  public int CheckPoint { get; set; }
  public int TimeStampInterval { get; set; }  //logStatus number * 3, logged every 3rd minute;
  public string TimeString { get; set; }
  public int Steel { get; set; }
  public int Oil { get; set; }
  public int Wood { get; set; }
  public int Water { get; set; }
  public int CleanWater { get; set; }
  public int Population { get; set; }
  public int CleanFood { get; set; }
  public int Food { get; set; }

  public LogStatus() { }

  public LogStatus(LogType logType, int gameStage, int timeStampInterval, string time, int steel, int oil, int wood, int water, int population, int cleanWater, int cleanFood, int food)
  {
    LogType = logType;
    CheckPoint = gameStage;
    TimeStampInterval = timeStampInterval;
    TimeString = time;
    Steel = steel;
    Oil = oil;
    Wood = wood;
    Water = water;
    Population = population;
    Food = food;
    CleanFood = cleanFood;
    CleanWater = cleanWater;
  }
}

public enum LogType { EndGame, Regular, StageChange }
