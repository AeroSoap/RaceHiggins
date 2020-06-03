
using UnityEngine;

public class CameraEffect : MonoBehaviour {

	public float Smoothing;

	Rigidbody rb;
	Camera cam;

	float fov;

	// Start is called before the first frame update
	void Start() {
		rb = transform.parent.gameObject.GetComponent<Rigidbody>();
		cam = GetComponent<Camera>();
		fov = 60;
	}

	// Update is called once per frame
	void Update() {
		float target = rb.velocity.magnitude / 7 + 60;
		fov = fov * Smoothing + target * (1 - Smoothing);
		cam.fieldOfView = fov;
	}
}
