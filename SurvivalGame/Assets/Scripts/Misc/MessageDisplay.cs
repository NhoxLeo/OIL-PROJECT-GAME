using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for displaying different kinds of messages to the player such as tooltips.
/// </summary>
public static class MessageDisplay
{

  /// <summary>
  /// Display a tooltip on the screen for a limited amount of time.
  /// </summary>
  /// <param name="tooltipText">Text to display.</param>
  /// <param name="followMouse">If true, the tooltip will follow the mouse cursor.</param>
  public static void DisplayTooltip(string tooltipText, bool followMouse, bool indefinetely = false, bool isSmallBox = false)
  {
    Camera.main.GetComponentInChildren<Tooltip>().DisplayTooltip(tooltipText, followMouse, indefinetely, isSmallBox);
  }

  /// <summary>
  /// Shows a tooltip as long as the mouse is over the UI element.
  /// </summary>
  /// <param name="displayTooltip">The text to display, building costs for instance.</param>
  public static void DisplayTooltip(bool displayTooltip)
  {
    if (!displayTooltip)
      Camera.main.GetComponentInChildren<Tooltip>().DisableTooltip();
  }

  // Not working
  public static void SpawnFloatingText(string text)
  {
    Camera.main.GetComponentInChildren<Tooltip>().SpawnFloatingText(text);
  }
}
