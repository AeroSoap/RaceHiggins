using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityCalculator : MonoBehaviour {

	public static Vector3 GetGravity(Vector3 pos) {
		string name = SceneManager.GetActiveScene().name;
		if(name == "Level1") {
			return Level1(pos);
		}
		if(name == "Level3") {
			return Level3(pos);
		}
		return SampleGravity(pos);
	}

	static Vector3 Level1(Vector3 pos) {
		return new Vector3(0, -50, 0);
	}
	static Vector3 Level3(Vector3 pos) {
		return new Vector3(0, -50, 0);
	}

	static Vector3 SampleGravity(Vector3 pos) {
		Vector3 rel = pos - new Vector3(395, 200, -226);
		if(rel.magnitude < 155) {
			return rel.normalized * 50;
		}
		return new Vector3(0, -50, 0);
	}
}
