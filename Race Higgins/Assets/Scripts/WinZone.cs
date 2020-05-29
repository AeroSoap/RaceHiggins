using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour {

	public string TargetScene;

	// Start is called before the first frame update
	void Start() {
		
	}

	private void OnTriggerEnter(Collider other) {
		// If the car enters the win zone, update manager variables with the correct scenes and go to the win screen
		if(other.name == "Car") {
			Manager.curLevel = SceneManager.GetActiveScene().name;
			Manager.newLevel = TargetScene;
			SceneManager.LoadScene("WinScreen");
		}
	}
}
