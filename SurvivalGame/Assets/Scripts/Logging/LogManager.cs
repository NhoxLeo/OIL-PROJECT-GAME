using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using Assets.Scripts.Logging;

public class LogManager : MonoBehaviour
{
  [SerializeField]
  string _playerName = "";

  [SerializeField]
  bool LoggingActivated;

  [SerializeField]
  float _loggingInterval = 180;

  [SerializeField]
  GameObject ResourceManager = null;

  static LogManager Instance;

  public static LogManager Singleton
  {
    get
    {
      return Instance;
    }
  }

  string _logFileName;
  LogFileModel logFileModel;
  GameSessionModel gameSession;

  float _loggingTimer = 0;

  void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;

    if(_playerName == string.Empty)
    {
      ///Just using a guid instead of actual playername for now.
      _playerName = Guid.NewGuid().ToString();
    }
    _logFileName += _playerName + "Log" + ".xml";
    logFileModel = ReadFromXMLFileIfExists(GetFolderPath(_playerName));
    gameSession = new GameSessionModel
    {
      PlayerName = _playerName,
      SessionNumber = logFileModel.Log.Count,
      SessionDate = DateTime.Today.Date.ToShortDateString()
    };
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.V))
      CreateLogStatusOnInterval(LogType.Regular);

    //if (Input.GetKeyDown(KeyCode.B))
    //  FinalizeLogFile();

    _loggingTimer += Time.deltaTime;
   // Debug.Log(_loggingTimer);
    if(_loggingTimer >= _loggingInterval)
    {
      _loggingTimer = 0;
      CreateLogStatusOnInterval(LogType.Regular);
    }
  }

  public void CreateLogStatusOnSpaceShipUpgrade(LogType logType)
  {
    ResourceManager resourceManager = ResourceManager.GetComponent<ResourceManager>();
    int gameStage = (int)GlobalConstants.currentStage;
    string timePassed = Time.realtimeSinceStartup.ToString();

    LogStatus logStatus = new LogStatus(
      logType,
      gameStage,
      0,
      timePassed,
      resourceManager.Steel,
      resourceManager.Oil,
      resourceManager.Wood,
      resourceManager.Water,
      resourceManager.Population,
      resourceManager.CleanWater,
      resourceManager.CookedFood,
      resourceManager.Food);

    gameSession.NewStageChangeStatus(logStatus);

    Debug.LogWarning("Logg Time " + logStatus.TimeString + " stageChangeLog status count: " + gameSession.StageChangeLogs.Count);
  }

  private void CreateLogStatusOnInterval(LogType logType)
  {


    ResourceManager resourceManager = ResourceManager.GetComponent<ResourceManager>();
    int gameStage = (int)GlobalConstants.currentStage;
    string timePassed = Time.realtimeSinceStartup.ToString();

    LogStatus logStatus = new LogStatus(
      logType,
      gameStage,
      gameSession.RegularIntervalLogs.Count * (int)_loggingInterval,
      timePassed, resourceManager.Steel,
      resourceManager.Oil,
      resourceManager.Wood,
      resourceManager.Water,
      resourceManager.Population,
      resourceManager.CleanWater,
      resourceManager.CookedFood,
      resourceManager.Food);


    gameSession.NewRegularIntervalStatus(logStatus);

    Debug.LogWarning("Logg Time " + logStatus.TimeString + " regularInterval status count: " + gameSession.RegularIntervalLogs.Count);
  }

  /// <summary>
  /// Should be called whenever the player wins or exits the game from the GameManager.cs.
  /// </summary>
  public void FinalizeLogFile()
  {
    if (!LoggingActivated || gameSession.RegularIntervalLogs.Count == 0)
      return;
    logFileModel.AddGameSessionToFile(gameSession);
    logFileModel.SerializeLogToXML(GetFolderPath(_playerName));
  }

  public LogFileModel ReadFromXMLFileIfExists(string folderPath)
  {
    LogFileModel logFile;

    FileInfo fileInfo = new FileInfo(folderPath);
    if (fileInfo.Exists)
    {
      using (var stream = new StreamReader(folderPath))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(LogFileModel));
        logFile = (LogFileModel)serializer.Deserialize(stream);
      }
     // Debug.Log("LogFile was created from file " + Path.GetFileName(folderPath));
      return logFile;

    }
    //Debug.Log("A new logFile was created at path " + Path.GetFileName(folderPath));
    return new LogFileModel
    {
      LogFileName = _logFileName
    };
  }

  string GetFolderPath(string playerName)
  {
    string fileName = playerName + "Log" + ".xml";
    string folderName = "LogFiles";
    string folderPath = Path.Combine(folderName, fileName);

    string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);

    strPath = Path.Combine(strPath, "OilLogFiles");


   
    ////System.IO.File.WriteAllText(file.FullName, fileName);
    ///
    //Directory.CreateDirectory(strPath);
    strPath = Path.Combine(strPath, fileName);

    System.IO.FileInfo file = new System.IO.FileInfo(strPath);
    file.Directory.Create(); // If the directory already exists, this method does nothing.

    return strPath;
  }

  private void OnApplicationQuit()
  {
    if(LoggingActivated)
    {
      Debug.Log("Write to logFile [Logging activated]");
      FinalizeLogFile();
    }
  }
}
