
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

	// Start is called before the first frame update
	void Start() {
		// Get the text component
		Text text = GetComponent<Text>();
		text.text += toTime(Manager.time);
	}

	public static string toTime(float time) {
		// Calculate the minutes, seconds, and milliseconds based on the decimal seconds
		int mins = (int)(time / 60);
		int secs = (int)(time) % 60;
		int ms = (int)(time * 1000 + 0.5) % 1000;
		// Add the formatted string representing the time to the UI
		return mins.ToString().PadLeft(2, '0')	+ ":" + secs.ToString().PadLeft(2, '0')
			+ ":" + ms.ToString().PadLeft(3, '0');
	}
}
