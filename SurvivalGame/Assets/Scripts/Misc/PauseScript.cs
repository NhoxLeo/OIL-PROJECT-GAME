using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

    /// <summary>
    /// Pauses the game.
    /// </summary>
    /// <param name="pause">The game will pause if pause = true.</param>
	public void PauseGame(bool pause) {
        if (pause)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }
}
