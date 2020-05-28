
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

	// Start is called before the first frame update
	void Start() {
		// Get the text component
		Text text = GetComponent<Text>();
		// Calculate the minutes, seconds, and milliseconds based on the decimal seconds
		int mins = (int)(Manager.time / 60);
		int secs = (int)(Manager.time) % 60;
		int ms = (int)(Manager.time * 1000 + 0.5) % 1000;
		// Add the formatted string representing the time to the UI
		text.text += mins.ToString().PadLeft(2, '0')
			+ ":" + secs.ToString().PadLeft(2, '0') + ":" + ms.ToString().PadLeft(3, '0');
	}
}
