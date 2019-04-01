using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml.Schema;
using Assets.Scripts.Logging;

/// <summary>
/// The logmodel basically the whole Logfile which contains multiple
/// gameSessions
/// </summary>
[Serializable]
public class LogFileModel
{
  public string LogFileName;
  public int TotalGameSessions;

  public List<GameSessionModel> Log = new List<GameSessionModel>();

  public LogFileModel() { }

  public void AddGameSessionToFile(GameSessionModel gameSession)
  {
    Log.Add(gameSession);
    TotalGameSessions = Log.Count;
  }

  public void SerializeLogToXML(string folderPath)
  {
    try
    {
      using (var stream = new FileStream(folderPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(LogFileModel));
        serializer.Serialize(stream, this);
      }
      Debug.Log("LogModel was serialized to path " + folderPath);
    }
    catch (Exception e)
    {
      Debug.Log(e.Message);
      throw;
    }
   
  }
}


