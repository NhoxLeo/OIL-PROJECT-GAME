using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGame : MonoBehaviour {
	public void leaveGame () {
        Debug.Log("You left the game");
        Application.Quit();
	}
}
