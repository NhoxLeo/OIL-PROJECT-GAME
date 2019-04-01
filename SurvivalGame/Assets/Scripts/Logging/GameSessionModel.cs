using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Logging
{
  /// <summary>
  /// GameSessionModel is a single gameplay session with statuses regularly
  /// updated and creating during gameplay
  /// </summary>
  [Serializable]
  public class GameSessionModel
  {
    public string PlayerName { get; set; }
    public int TotalCheckPoints { get; set; }
    public int SessionNumber { get; set; }
    public string SessionDate { get; set; }
    public List<LogStatus> StageChangeLogs = new List<LogStatus>();
    public List<LogStatus> RegularIntervalLogs = new List<LogStatus>();

    public void NewStageChangeStatus(LogStatus status)
    {
      StageChangeLogs.Add(status);
    }

    public void NewRegularIntervalStatus(LogStatus status)
    {
      RegularIntervalLogs.Add(status);
      TotalCheckPoints = RegularIntervalLogs.Count;
    }

  }
}
