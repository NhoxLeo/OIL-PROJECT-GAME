using System;

public class LogModelEventArgs : EventArgs
{
  readonly LogStatus newGameStatus;
  public LogStatus NewGameStatus { get { return newGameStatus; } }

  public LogModelEventArgs(LogStatus newGameStatus)
  {
    this.newGameStatus = newGameStatus;
  }

}
