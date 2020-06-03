using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text text;

	// Start is called before the first frame update
	void Start() {
		// Reset time
		Manager.time = 0;
	}

	void FixedUpdate() {
		// Increase time
		Manager.time += Time.fixedDeltaTime;
		// Update the UI
		text.text = ScoreText.toTime(Manager.time);
	}
}
