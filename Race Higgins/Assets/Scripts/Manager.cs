using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	// Player race time
	public static float time;
	// The next level to go to after this one
	public static string newLevel;
	// The current level to be reloaded should the player choose to do so
	public static string curLevel;

	public void NextLevel() {
		SceneManager.LoadScene(newLevel);
	}

	public void RestartLevel() {
		SceneManager.LoadScene(curLevel);
	}
}
