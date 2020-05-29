using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		// If this obstacle has collided with the car, restart
		if(other.name == "Car") {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
