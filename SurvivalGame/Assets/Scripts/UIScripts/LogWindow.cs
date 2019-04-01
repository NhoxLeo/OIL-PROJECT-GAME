using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogWindow : MonoBehaviour
{
  [SerializeField]
  Text logText;

  [SerializeField]
  ScrollRect scrollRect;

  static LogWindow Instance;

  public static LogWindow Singleton
  {
    get
    {
      return Instance;
    }
  }

  void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private const int maxEntries = 50;
  private List<string> entryList = new List<string>();

  /// <summary>
  /// Add lines to the log window.
  /// </summary>
  /// <param name="textToAdd">The string to be parsed.</param>
  public void AddText(string textToAdd)
  {
    textToAdd = "\n> " + textToAdd;
    entryList.Add(textToAdd);
    logText.text = "";
    foreach (string entry in entryList)
      logText.text += entry;
    if (entryList.Count > maxEntries)
      entryList.Remove(entryList[0]);
    scrollRect.verticalNormalizedPosition = 0f;
  }
}