using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour {

	// Start is called before the first frame update
	void Start() {

	}

	private void OnTriggerEnter(Collider other) {
		if(other.name == "Car") {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
