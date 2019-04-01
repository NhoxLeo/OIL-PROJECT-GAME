using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public int year;
  public int Year { get { return year; } set { year = value; } }
  private float timescale; // Years per minute
  private float yearInterval, yearTimer;
  private PopulationManager pm;
  private GameObject canvasGameOver;

  public void Awake()
  {
    pm = GameObject.Find("Population").GetComponent<PopulationManager>();
    year = 3;
    timescale = 3f;
    yearInterval = 60 / timescale;
    yearTimer = 0f;
  }

  public void Update()
  {
    ManageYearProgression();
    CheckVictoryConditions();
    CheckDefeatConditions();
  }

  private void ManageYearProgression()
  {
    yearTimer += Time.deltaTime;
    if (yearTimer >= yearInterval)
    {
      yearTimer = 0f;
      year++;
      pm.AgeWorkers();
    }
  }

  private void CheckVictoryConditions()
  {

  }

  public void WinGame()
  {
    Time.timeScale = 0f;
    Debug.Log("VICTORY");
    LogManager.Singleton.FinalizeLogFile();
    canvasGameOver = Camera.main.transform.Find("CanvasGameOver").gameObject;
    canvasGameOver.SetActive(true);
    canvasGameOver.transform.GetChild(0).Find("Panel").transform.Find("TextDefeat").gameObject.SetActive(false);
  }

  private void CheckDefeatConditions()
  {
    if (pm.transform.childCount <= 0)
      GameOver();
  }

  public void GameOver()
  {
    Time.timeScale = 0f;
    Debug.Log("GAME OVER");
    LogManager.Singleton.FinalizeLogFile();
    canvasGameOver = Camera.main.transform.Find("CanvasGameOver").gameObject;
    canvasGameOver.SetActive(true);
    canvasGameOver.transform.GetChild(0).Find("Panel").transform.Find("TextVictory").gameObject.SetActive(false);
  }

  public void LeaveGame()
  {
    LogManager.Singleton.FinalizeLogFile();
    SceneManager.LoadScene("Mainmenu", LoadSceneMode.Single);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
