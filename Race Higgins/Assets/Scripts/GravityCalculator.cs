using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityCalculator : MonoBehaviour {

	// Gets the gravity vector for the given location
	public static Vector3 GetGravity(Vector3 pos) {
		// Fetch the scene name
		string name = SceneManager.GetActiveScene().name;
		// Go through the scenes it could be to determine which gravity function to use
		if(name == "Level1") {
			return Level1(pos);
		}
		if(name == "Level3") {
			return Level3(pos);
		}
		// Return the default if there is no found scene
		return SampleGravity(pos);
	}

	static Vector3 Level1(Vector3 pos) {
		return new Vector3(0, -50, 0);
	}

	static Vector3 Level3(Vector3 pos) {
		return new Vector3(0, -50, 0);
	}

	static Vector3 SampleGravity(Vector3 pos) {
		return new Vector3(0, -50, 0);
	}
}
