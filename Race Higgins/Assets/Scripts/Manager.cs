using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	public static float time;
	public static string newLevel;
	public static string curLevel;

	public void NextLevel() {
		SceneManager.LoadScene(newLevel);
	}

	public void RestartLevel() {
		SceneManager.LoadScene(curLevel);
	}
}
